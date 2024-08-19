using Proyecto2.Models;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Proyecto2.Controllers
{
    public class NominasController : Controller
    {
        // GET: Nominas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GestionNomina()
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    var nominasList = (from n in db.NOMINA
                                       orderby n.fechaRegistro descending
                                       select n).ToList();

                    if (nominasList.Count > 0)
                    {
                        foreach (var n in nominasList)
                        {
                            var usuarioVar = (from u in db.USUARIO
                                          where u.id == n.idUsuario
                                          select u).FirstOrDefault(); 

                            n.USUARIO = usuarioVar;
                        }
                    }

                    return View(nominasList);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult CrearNomina()
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {

                    var departamentosList = (from d in db.DEPARTAMENTO
                                       select d).ToList();

                    ViewBag.DepartamentosList = departamentosList;

                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GenerarNomina(NOMINA oNomina)
        {
            USUARIO usuario = new USUARIO();

            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    var usuarioInfo = (from u in db.USUARIO
                                       where u.id == oNomina.idUsuario
                                       select u).FirstOrDefault();

                    if(usuarioInfo != null)
                    {
                        oNomina.USUARIO = usuarioInfo;
                        oNomina.USUARIO.nombre = usuarioInfo.nombre + " " + usuarioInfo.apellido;
                    }

                    

                    var oDepartamentosList = (from d in db.DEPARTAMENTO
                                              where d.id == usuarioInfo.idDepartamento
                                              select d).ToList();

                    var oUsuariosList = (from ul in db.USUARIO
                                              where ul.idDepartamento == usuarioInfo.idDepartamento
                                              select ul).ToList();

                    ViewBag.DepartamentosList = oDepartamentosList;
                    ViewBag.UsuariosPerDepartList = oUsuariosList;

                    return View(oNomina);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult DetalleNomina(int id)
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {

                    var nominaInfo = (from nomina in db.NOMINA
                                      where nomina.id == id
                                      select nomina).FirstOrDefault();

                    if (nominaInfo == null)
                    {
                        ViewBag.Error = "No existe la nomina con el id: " + id;
                        return View();
                    }

                    var oMetodoPago = (from mp in db.METODO_PAGO
                                       where mp.id == nominaInfo.idMetodoPago
                                       select mp).FirstOrDefault();

                    var oUsuario = (from u in db.USUARIO
                                    where u.id == nominaInfo.idUsuario
                                    select u).FirstOrDefault();

                    var oBonificaciones = (from b in db.BONIFICACIONES
                                           where b.idNomina == nominaInfo.id
                                           select b).ToList();

                    var oDeducciones = (from b in db.DEDUCCIONES
                                        where b.idNomina == nominaInfo.id
                                        select b).ToList();

                    nominaInfo.USUARIO = oUsuario;
                    nominaInfo.METODO_PAGO = oMetodoPago;

                    ViewBag.BonificacionesList = oBonificaciones;
                    ViewBag.DeduccionesList = oDeducciones;

                    return View(nominaInfo);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }


        //Operaciones

        public ActionResult RegistrarNomina(NOMINA oNomina)
        {
            DateTime sysdate = DateTime.Now;

            string[] bonificacionesDetalle = new string[] { };
            string[] bonificacionesMonto = new string[] { };
            string[] deduccionesDetalle = new string[] { };
            string[] deduccionesMonto = new string[] { };

            if (!((string)Request["bonificaciones-d"]).IsEmpty())
            {
                bonificacionesDetalle = ((string)Request["bonificaciones-d"]).Split(',');
            }

            if (!((string)Request["bonificaciones-m"]).IsEmpty())
            {
                bonificacionesMonto = ((string)Request["bonificaciones-m"]).Split(',');
            }

            if (!((string)Request["deducciones-d"]).IsEmpty())
            {
                deduccionesDetalle = ((string)Request["deducciones-d"]).Split(',');
            }

            if (!((string)Request["deducciones-m"]).IsEmpty())
            {
                deduccionesMonto = ((string)Request["deducciones-m"]).Split(',');
            }

            if(oNomina.comentario == null) { oNomina.comentario = " "; }

            var nominaData = new NOMINA()
            {
                idUsuario = oNomina.idUsuario,
                ano = oNomina.ano,
                mes = oNomina.mes,
                idMetodoPago = oNomina.idMetodoPago,
                salarioBase = oNomina.salarioBase,
                bonificacionesTotal = oNomina.bonificacionesTotal,
                deduccionesTotal = oNomina.deduccionesTotal,
                salarioNeto = oNomina.salarioNeto,
                comentario = (string)oNomina.comentario,
                fechaRegistro = sysdate,
                estado = 1
            };

            try
            {
                using(Models.Entities db = new Models.Entities())
                {
                    var insertNomina = db.NOMINA.Add(nominaData);
                    db.SaveChanges();

                    if(insertNomina != null)
                    {
                        for (int i = 0; i < bonificacionesDetalle.Count(); i++)
                        {
                            var bonificacion = new BONIFICACIONE()
                            {
                                idUsuario = insertNomina.idUsuario,
                                idNomina = insertNomina.id,
                                detalle = bonificacionesDetalle[i],
                                monto = Decimal.Parse(bonificacionesMonto[i])
                            };

                            var insertBonificacion = db.BONIFICACIONES.Add(bonificacion);
                            db.SaveChanges();

                        }

                        for (int b = 0; b < deduccionesDetalle.Count(); b++)
                        {
                            var deduccion = new DEDUCCIONE()
                            {
                                idUsuario = insertNomina.idUsuario,
                                idNomina = insertNomina.id,
                                detalle = deduccionesDetalle[b],
                                monto = Decimal.Parse(deduccionesMonto[b])
                            };

                            var insertDeduccion = db.DEDUCCIONES.Add(deduccion);
                            db.SaveChanges();

                        }

                    }
                    return RedirectToAction("GestionNomina", "Nominas");
                }
            }
            catch (Exception ex)
            {
            }

            return View();
        }

        [HttpPost]
        public JsonResult ObtenerDepartamentos()
        {
            List<DepartamentoMdl> olista = new List<DepartamentoMdl>();

            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    var oDepartamentos = (from departamentos in db.DEPARTAMENTO
                                          select departamentos).ToList();

                    if (oDepartamentos != null)
                    {
                        foreach (var item in oDepartamentos)
                        {
                            DepartamentoMdl dep = new DepartamentoMdl();
                            dep.id = item.id;
                            dep.nombre = item.nombre;
                            olista.Add(dep);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                olista = new List<DepartamentoMdl>();
            }

            return Json(new { lista = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerEmpleadosPorDepartamento(int idDepartamento)
        {
            List<EmpleadoMdl> olista = new List<EmpleadoMdl>();

            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    var oEmpleados = (from emp in db.USUARIO
                                      where emp.idDepartamento == idDepartamento
                                      select emp).ToList();

                    if (oEmpleados != null)
                    {
                        foreach (var item in oEmpleados)
                        {
                            EmpleadoMdl empleado = new EmpleadoMdl();
                            empleado.id = item.id;
                            empleado.nombre = item.nombre;
                            empleado.apellido = item.apellido;
                            olista.Add(empleado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                olista = new List<EmpleadoMdl>();
            }

            return Json(new { lista = olista }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EliminarNomina(int id)
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {

                    var deleteDeducciones = (from d in db.DEDUCCIONES
                                             where d.idNomina == id
                                             select d).ToList();

                    foreach(var d in deleteDeducciones)
                    {
                        db.DEDUCCIONES.Remove(d);
                        db.SaveChanges();
                    }

                    var deleteBonificaciones = (from d in db.BONIFICACIONES
                                             where d.idNomina == id
                                             select d).ToList();

                    foreach (var b in deleteBonificaciones)
                    {
                        db.BONIFICACIONES.Remove(b);
                        db.SaveChanges();
                    }


                    var deleteNomina = (from nomina in db.NOMINA
                                         where nomina.id == id
                                         select nomina).FirstOrDefault();
                    db.NOMINA.Remove(deleteNomina);
                    db.SaveChanges();

                    if (deleteNomina == null)
                    {
                        ViewBag.Error = "No se pudo eliminar la nomina.";
                        return View();
                    }

                    ViewBag.Msg = "Se ha eliminado la nomina.";
                    return RedirectToAction("GestionNomina", "Nominas");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }


    }
}