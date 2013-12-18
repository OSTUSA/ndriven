using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Core.Domain.Model;
using CoreUsers = Core.Domain.Model.Users;

namespace Presentation.Web.Validation.User
{
    abstract public class UserValidationAttributeBase : ValidationAttribute
    {
        public virtual IRepository<CoreUsers.User> Repo { get; set; }

        protected string Message { get; set; }

        protected UserValidationAttributeBase(string message = "")
        {
            Repo = DependencyResolver.Current.GetService<IRepository<CoreUsers.User>>();
            if (!string.IsNullOrEmpty(message))
                Message = message;
        }
    }
}