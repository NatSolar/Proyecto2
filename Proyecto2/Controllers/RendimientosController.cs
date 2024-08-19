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
    public class RendimientosController : Controller
    {
        // GET: Rendimientos
        public ActionResult Index()
        {
            return View();
        }


        [AutorizarUsuario(idOperacion: 10)]
        public ActionResult ListaRendimientos()
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    var evaluacionesList = (from eval in db.EVALUACION
                                       orderby eval.fechaEvaluacion descending
                                       select eval).ToList();

                    if (evaluacionesList.Count > 0)
                    {
                        foreach (var n in evaluacionesList)
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

                    return View(evaluacionesList);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }


        [AutorizarUsuario(idOperacion: 9)]
        public ActionResult RealizarEvaluacion()
        {
            return View();
        }

        [AutorizarUsuario(idOperacion: 9)]
        public ActionResult EvaluarEmpleado(EVALUACION oEvaluacion)
        {
            DateTime sysdate = DateTime.Now;

            string[] notaObtenida = new string[] { };

            if (!((string)Request["notaObtenida"]).IsEmpty())
            {
                notaObtenida = ((string)Request["notaObtenida"]).Split(',');
            }

            var idUserLog = (USUARIO)Session["Usuario"];
            var notaPromedio = 0;
            List<int> promediosObjetivos = new List<int>();

            try
            {

                using (Models.Entities db = new Models.Entities())
                {
                    var objetivosEmpleado = (from o in db.OBJETIVOS
                                             where o.idEmpleado == oEvaluacion.idEmpleado
                                             && o.idPeriodo == oEvaluacion.idPeriodo
                                             orderby o.id
                                             select o).ToList();

                    if (objetivosEmpleado!=null && objetivosEmpleado.Count() > 0)
                    {
                        if(objetivosEmpleado.Count() == notaObtenida.Length)
                        {
                            for (int i = 0; i < objetivosEmpleado.Count(); i++)
                            {
                                var idTemp = objetivosEmpleado[i].id;

                                var objetivoUpdate = (from o in db.OBJETIVOS
                                                      where o.id == idTemp
                                                      select o).FirstOrDefault();

                                objetivoUpdate.nota = Convert.ToInt32(notaObtenida[i]);

                                db.SaveChanges();

                                //Promedio según peso
                                var promedioObj = (Convert.ToInt32(notaObtenida[i]) * objetivosEmpleado[i].peso) / 100;
                                promediosObjetivos.Add(promedioObj);

                            }

                            foreach(var promedio in promediosObjetivos)
                            {
                                notaPromedio += promedio;
                            }


                            //Registro de evaluacion
                            var newEvaluacion = new EVALUACION()
                            {
                                idEmpleado = oEvaluacion.idEmpleado,
                                idPeriodo = oEvaluacion.idPeriodo,
                                notaPromedio = notaPromedio,
                                retroalimentacion = oEvaluacion.retroalimentacion,
                                fechaEvaluacion = sysdate,
                                idEvaluador = idUserLog.id
                            };

                            var insertEvaluacion = db.EVALUACION.Add(newEvaluacion);
                            db.SaveChanges();

                        }
                    }
                }
                return RedirectToAction("ListaRendimientos", "Rendimientos");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }

        }


        [AutorizarUsuario(idOperacion: 10)]
        public ActionResult DetallesEvaluacion(int id)
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {

                    var evaluacionInfo = (from eval in db.EVALUACION
                                          where eval.id == id
                                          select eval).FirstOrDefault();

                    if (evaluacionInfo == null)
                    {
                        ViewBag.Error = "No existe la evaluación con el id: " + id;
                        return RedirectToAction("Index", "Home");
                    }

                    var oEmpleado = (from u in db.USUARIO
                                     where u.id == evaluacionInfo.idEmpleado
                                     select u).FirstOrDefault();

                    var oEvaluador = (from u in db.USUARIO
                                      where u.id == evaluacionInfo.idEvaluador
                                      select u).FirstOrDefault();

                    var oPeriodo = (from u in db.PERIODO
                                      where u.id == evaluacionInfo.idPeriodo
                                      select u).FirstOrDefault();

                    var oObjetivos = (from u in db.OBJETIVOS
                                    where u.idEmpleado == evaluacionInfo.idEmpleado
                                    && u.idPeriodo == evaluacionInfo.idPeriodo
                                    orderby u.id
                                    select u).ToList();

                    evaluacionInfo.USUARIO = oEmpleado;
                    evaluacionInfo.USUARIO1 = oEvaluador;
                    evaluacionInfo.PERIODO = oPeriodo;

                    var oDepartamento = (from u in db.DEPARTAMENTO
                                         where u.id == evaluacionInfo.USUARIO.idDepartamento
                                         select u).FirstOrDefault();

                    evaluacionInfo.USUARIO.DEPARTAMENTO = oDepartamento;

                    ViewBag.ObjetivosList = oObjetivos;
                    //ViewBag.DeduccionesList = oDeducciones;

                    return View(evaluacionInfo);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }



        /************** OPERACIONES ********/


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


        [HttpPost]
        public JsonResult ListarObjetivos(int idEmpleado, int idPeriodo)
        {
            List<ObjetivosMdl> olista = new List<ObjetivosMdl>();

            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    var objetivosList = (from obj in db.OBJETIVOS
                                     where obj.idEmpleado == idEmpleado
                                     && obj.idPeriodo == idPeriodo
                                     orderby obj.id
                                     select obj).ToList();

                    if (objetivosList != null)
                    {
                        foreach (var item in objetivosList)
                        {
                            if(item.nota == 0)
                            {
                                ObjetivosMdl dep = new ObjetivosMdl();
                                dep.id = item.id;
                                dep.idEmpleado = item.idEmpleado;
                                dep.idPeriodo = item.idPeriodo;
                                dep.detalle = item.detalle;
                                dep.valorReferencia = item.valorReferencia;
                                dep.peso = item.peso;
                                dep.nota = item.nota;
                                dep.retroalimentacion = item.retroalimentacion;
                                dep.fechaCreacion = item.fechaCreacion;
                                dep.idEvaluador = item.idEvaluador;
                                olista.Add(dep);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                olista = new List<ObjetivosMdl>();
            }

            return Json(new { lista = olista }, JsonRequestBehavior.AllowGet);
        }

    }
}