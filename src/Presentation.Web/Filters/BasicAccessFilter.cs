using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Core.Domain.Model.Users;
using System.Collections.Generic;
using Infrastructure.IoC.NHibernate;
using Infrastructure.NHibernate;
using Infrastructure.NHibernate.Repositories;
using NHibernate;

namespace Presentation.Web.Filters
{
    public class BasicAccessFilter : AuthorizationFilterAttribute
    {
        private ISessionFactory _factory;

        public static IDictionary<string, long> UserCache = new Dictionary<string, long>();

        public BasicAccessFilter()
        {
            var factoryFunc = NHibernateModule.DefaultFactory;
            var builder = new SessionFactoryBuilder();
            _factory = builder.GetFactory("Default", factoryFunc);
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (ShouldSkipAuth(actionContext)) return;

            var request = actionContext.Request;
            var xAuth = string.Empty;
            if (request.Headers.Contains("X-AUTH-TOKEN"))
            {
                xAuth = request.Headers.GetValues("X-AUTH-TOKEN").FirstOrDefault();
            }
            var cookie = request.Headers.GetCookies().Select(c => c["UserAuthenticationToken"]).FirstOrDefault();
            if (cookie != null && !string.IsNullOrEmpty(xAuth))
            {
                // as long as value is the correct one and the user cache still has a referance to it (ie not logged out) we are good
                if (cookie.Value == xAuth && UserCache.ContainsKey(cookie.Value))
                {
                    SetPrincipal(UserCache[cookie.Value]);
                    base.OnAuthorization(actionContext);
                    return;
                }
            }

            var user = GetUser(actionContext);
            if (user == null)
            {
                Unauthorized(actionContext);
                return;
            }
            SetPrincipal(user);
            base.OnAuthorization(actionContext);
        }

        private static void Unauthorized(HttpActionContext actionContext)
        {
            var resp = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response = resp;
        }

        public bool ShouldSkipAuth(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
               || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }

        private User GetUser(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            if (request.Headers.Authorization == null) return null;
            var user = UnpackUser(request);
            return user;
        }

        private User UnpackUser(HttpRequestMessage request)
        {
            var decoded = GetBase64DecodedString(request.Headers.Authorization.Parameter).Split(':');
            string email = decoded[0], password = decoded[1];
            using (var session = _factory.OpenSession())
            {
                var repo = new NHibernateRepository<User>(session);
                var user = repo.FindOneBy(u => u.Email == email);
                if (!user.IsAuthenticated(password)) return null;
                return user;
            }
        }

        private static string GetBase64DecodedString(string toDecode)
        {
            var data = System.Convert.FromBase64String(toDecode);
            return System.Text.Encoding.UTF8.GetString(data);
        }

        private static void SetPrincipal(User user)
        {
            SetPrincipal(user.Id);
        }

        private static void SetPrincipal(long id)
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(id.ToString()), null);
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = Thread.CurrentPrincipal;
            }
        }
    }
}