using Proyecto2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto2.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AutorizarUsuario : AuthorizeAttribute
    {
        private USUARIO oUsuario;
        private Entities db = new Entities();
        private int idOperacion;

        public AutorizarUsuario(int idOperacion = 0)
        {
            this.idOperacion = idOperacion;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string nombreOperacion = "";
            string nombreModulo = "";

            try
            {
                oUsuario = (USUARIO)HttpContext.Current.Session["Usuario"];

                if (oUsuario != null)
                {
                    var listOperaciones = (from m in db.USUARIO_OPERACIONES
                                           where m.idRol == oUsuario.idRol && m.idOperacion == idOperacion
                                           select m).ToList();

                    if (listOperaciones != null)
                    {
                        if (listOperaciones.Count == 0)
                        {
                            var oOperacion = db.OPERACIONES.Find(idOperacion);
                            int? idModulo = oOperacion.idModulo;
                            nombreOperacion = getNombreDeOperacion(idOperacion);
                            nombreModulo = getNombreModulo(idModulo);
                            filterContext.Result = new RedirectResult("~/Error/UnauthorizedOperation?operacion=" + nombreOperacion + "&modulo=" + nombreModulo + "&messageException=");
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                filterContext.Result = new RedirectResult("~/Error/UnauthorizedOperation?operacion=" + nombreOperacion + "&modulo=" + nombreModulo + "&messageException=");
            }
        }

        public string getNombreDeOperacion(int idOperacion)
        {
            string nombreOperacion;
            var oOperacion = from op in db.OPERACIONES where op.id == idOperacion
                             select op.nombre;

            try
            {
                nombreOperacion = oOperacion.First();
            }
            catch (Exception ex)
            {
                nombreOperacion = "";
            }

            return nombreOperacion;
        }

        public string getNombreModulo(int? idModulo)
        {
            string nombreModulo;
            var oModulo = from m in db.MODULO where m.id == idModulo
                          select m.nombre;

            try
            {
                nombreModulo = oModulo.First();
            }
            catch (Exception ex)
            {
                nombreModulo = "";
            }

            return nombreModulo;
        }

    }
}