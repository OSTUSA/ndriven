using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Presentation.Web.Controllers
{
    public class TemplatesController : Controller
    {
        [AllowAnonymous]
        public ActionResult GetTemplate(string template)
        {
            return View(template);
        }

    }
}
