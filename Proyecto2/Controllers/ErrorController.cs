using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto2.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        [HttpGet]
        public ActionResult UnauthorizedOperation(string operacion, string modulo, string messageException)
        {
            ViewBag.Operacion = operacion;
            ViewBag.Modulo = modulo;
            ViewBag.MessageException = messageException;
            return View();
        }
    }
}