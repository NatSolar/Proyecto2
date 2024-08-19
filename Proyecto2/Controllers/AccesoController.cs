using Proyecto2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace Proyecto2.Controllers
{
    public class AccesoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InicioSesion()
        {
            return View();
        }


        [HttpPost]
        public ActionResult InicioSesion(string Email, string Contrasena)
        {

            try
            {
                using(Models.Entities db = new Models.Entities()) {

                    var oUsuario = (from d in db.USUARIO where d.email == Email.Trim() && d.contrasena == Contrasena.Trim()
                                    select d).FirstOrDefault();

                    if (oUsuario == null)
                    {
                        ViewBag.Error = "Usuario o contraseña inválida";
                        return View();
                    }

                    Session["Usuario"] = oUsuario;
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}