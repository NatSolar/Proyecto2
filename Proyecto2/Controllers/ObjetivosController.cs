using Proyecto2.Filters;
using Proyecto2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Proyecto2.Controllers
{
    public class ObjetivosController : Controller
    {
        // GET: Objetivos
        public ActionResult Index()
        {
            return View();
        }

        [AutorizarUsuario(idOperacion: 8)]
        public ActionResult ListaObjetivos()
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    var objetivosList = (from obj in db.OBJETIVOS
                                         where obj.nota == 0
                                         orderby obj.fechaCreacion descending
                                         select obj).ToList();

                    if (objetivosList.Count > 0)
                    {
                        foreach (var n in objetivosList)
                        {
                            var empleadoVar = (from u in db.USUARIO
                                               where u.id == n.idEmpleado
                                               select u).FirstOrDefault();

                            var evaluadorVar = (from u in db.USUARIO
                                                where u.id == n.idEvaluador
                                                select u).FirstOrDefault();

                            var periodoVar = (from u in db.PERIODO
                                              where u.id == n.idPeriodo
                                              select u).FirstOrDefault();

                            n.USUARIO = empleadoVar;
                            n.USUARIO1 = evaluadorVar;
                            n.PERIODO = periodoVar;
                        }
                    }

                    return View(objetivosList);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }


        [AutorizarUsuario(idOperacion: 8)]
        public ActionResult RegistrarObjetivo()
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    var periodosList = (from eval in db.PERIODO
                                        orderby eval.desde descending
                                        select eval).ToList();

                    if (periodosList.Count > 0)
                    {
                        ViewBag.PeriodosList = periodosList;
                    }

                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        

        /*************** OPERACIONES *****************/

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

        [HttpPost]
        public JsonResult ListarPeriodos()
        {
            List<PeriodoMdl> olista = new List<PeriodoMdl>();

            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    var oPeriodos = (from periodos in db.PERIODO
                                     select periodos).ToList();

                    if (oPeriodos != null)
                    {
                        foreach (var item in oPeriodos)
                        {
                            PeriodoMdl dep = new PeriodoMdl();
                            dep.id = item.id;
                            dep.detalle = item.detalle;
                            dep.desde = item.desde;
                            dep.hasta = item.hasta;
                            dep.ano = item.ano;
                            olista.Add(dep);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                olista = new List<PeriodoMdl>();
            }

            return Json(new { lista = olista }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuardarObjetivo(OBJETIVO oObjetivo)
        {
            DateTime sysdate = DateTime.Now;

            string[] objetivosDetalle = new string[] { };
            string[] objetivosValorReferencia = new string[] { };
            string[] objetivosPeso = new string[] { };

            if (!((string)Request["objetivos-d"]).IsEmpty())
            {
                objetivosDetalle = ((string)Request["objetivos-d"]).Split(',');
            }

            if (!((string)Request["objetivos-r"]).IsEmpty())
            {
                objetivosValorReferencia = ((string)Request["objetivos-r"]).Split(',');
            }

            if (!((string)Request["objetivos-p"]).IsEmpty())
            {
                objetivosPeso = ((string)Request["objetivos-p"]).Split(',');
            }

            var idUserLog = (USUARIO)Session["Usuario"];

            try
            {

                using (Models.Entities db = new Models.Entities())
                {
                    for (int i = 0; objetivosDetalle.Length > 0; i++)
                    {
                        var newObjetivo = new OBJETIVO()
                        {
                            idEmpleado = oObjetivo.idEmpleado,
                            idPeriodo = oObjetivo.idPeriodo,
                            detalle = objetivosDetalle[i],
                            valorReferencia = Convert.ToInt32(objetivosValorReferencia[i]),
                            peso = Convert.ToInt32(objetivosPeso[i]),
                            nota = 0,
                            retroalimentacion = "",
                            fechaCreacion = sysdate,
                            idEvaluador = idUserLog.id
                        };

                        var insertObjetivo = db.OBJETIVOS.Add(newObjetivo);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("ListaObjetivos", "Objetivos");

        }

        public ActionResult EliminarObjetivo(int id)
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {

                    var deleteObjetivo = (from d in db.OBJETIVOS
                                             where d.id == id
                                             select d).FirstOrDefault();

                    db.OBJETIVOS.Remove(deleteObjetivo);
                    db.SaveChanges();

                    if (deleteObjetivo == null)
                    {
                        ViewBag.Error = "No se pudo eliminar el objetivo.";
                        return View();
                    }

                    ViewBag.Msg = "Se ha eliminado el objetivo.";
                    return RedirectToAction("ListaObjetivos", "Objetivos");
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