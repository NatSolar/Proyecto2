using Proyecto2.Controllers;
using Proyecto2.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace Proyecto2.Filters
{
    public class VerificaSesion : ActionFilterAttribute
    {
        private USUARIO oUsuario;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);

                oUsuario = (USUARIO)HttpContext.Current.Session["Usuario"];
                if(oUsuario == null)
                {
                    if(filterContext.Controller is AccesoController == false)
                    {
                        filterContext.HttpContext.Response.Redirect("/Acceso/InicioSesion");
                    }
                }
            } catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Acceso/InicioSesion");
            }
        }
    }
}