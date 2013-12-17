using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using AttributeRouting.Helpers;
using AttributeRouting.Web.Http;
using System.Net.Http;
using System.Web.Http;
using Core.Domain.Model;

using Core.Domain.Model.Users;
using Presentation.Web.Filters;
using Presentation.Web.Models.Display;
using Presentation.Web.Models.Input;

namespace Presentation.Web.Controllers
{
    public class UsersController : ApiController
    {
        protected IRepository<User> Users;

        public UsersController(IRepository<User> users)
        {
            Users = users;
        }
        
        [POST("api/register")]
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage Register(RegisterInput input)
        {
            if (!ModelState.IsValid) return Request.CreateResponse(HttpStatusCode.OK);

            var user = new User()
            {
                Email = input.Email,
                Name = input.Name,
                Password = input.Password
            };

            user.HashPassword();
            Users.Store(user);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [POST("api/login")]
        [System.Web.Http.AllowAnonymous]
        public HttpResponseMessage Login(LoginInput input)
        {

            var token = System.Guid.NewGuid().ToString();
            long id = 0;
            long.TryParse(User.Identity.Name, out id);
            BasicAccessFilter.UserCache[token] = id;
            var response = Request.CreateResponse(HttpStatusCode.OK, new UserDisplay() { Id = id, Token = token });
            var cookie = new CookieHeaderValue("UserAuthenticationToken", token);
            cookie.Expires = DateTimeOffset.Now.AddMinutes(30);
            response.Headers.AddCookies(new[] { cookie });
            return response;
        }

        [POST("api/logout")]
        public HttpResponseMessage Logout()
        {
            // get user auth cookie so we log off the right user/device
            var cookie = Request.Headers.GetCookies().Select(c => c["UserAuthenticationToken"]).FirstOrDefault();
            if (cookie != null)
            {
                BasicAccessFilter.UserCache.Remove(cookie.Value);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [GET("api/users/{Id}")]
        public HttpResponseMessage Get(long Id)
        {
            var user = Users.Get(Id);

            var userDisplay = new UserDisplay()
            {
                Name = user.Name,
                Email = user.Email
            };

            var request = Request.CreateResponse(HttpStatusCode.OK, userDisplay);

            return request;
        }
    }
}