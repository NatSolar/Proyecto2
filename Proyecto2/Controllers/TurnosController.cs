﻿using Proyecto2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Proyecto2.Controllers
{
    public class TurnosController : Controller
    {
        public ActionResult Agenda()
        {
            return View();
        }

        public ActionResult Index()
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    var turnosList = (from turno in db.TURNO
                                            orderby turno.id descending
                                            select turno).ToList();

                    List<TurnoMdl> list = new List<TurnoMdl>();

                    if (turnosList.Count > 0)
                    {
                        foreach (var n in turnosList)
                        {

                            CultureInfo provider = new CultureInfo("es-ES");
                            var inicio = "04/10/2021 " + n.horaEntrada.Substring(0, 2) + ":" + n.horaEntrada.Substring(3, 5);
                            var salida = "04/10/2021 " + n.horaSalida.Substring(0, 2) + ":" + n.horaSalida.Substring(3, 5);


                            DateTime fecha = DateTime.ParseExact(inicio, "dd/MM/yyyy HH:mm:ss", provider);
                            DateTime fecha2 = DateTime.ParseExact(salida, "dd/MM/yyyy HH:mm:ss", provider);
                            DateTime fecha3 = DateTime.ParseExact("04/10/2021 12:00:00", "dd/MM/yyyy HH:mm:ss", provider);

                            var horas = (fecha2 - fecha).TotalHours;

                            if(fecha2 > fecha3) { horas--; }

                            var newTurno = new TurnoMdl()
                            {
                                id = n.id,
                                idEmpleado = n.id,
                                dias = n.dias,
                                horaEntrada = n.horaEntrada,
                                horaSalida = n.horaSalida,
                                fechaRegistro = n.fechaRegistro,
                                idEncargado = n.idEncargado,
                                horas = horas
                            };

                            var empleadoVar = (from u in db.USUARIO
                                               where u.id == n.idEmpleado
                                               select u).FirstOrDefault();

                            var encargadoVar = (from u in db.USUARIO
                                                where u.id == n.idEncargado
                                                select u).FirstOrDefault();

                            newTurno.USUARIO = empleadoVar;
                            newTurno.USUARIO1 = encargadoVar;

                            list.Add(newTurno);
                        }
                    }

                    return View(list);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult CrearTurno()
        {
            return View();
        }

        public ActionResult RegistrarFeriado()
        {
            return View();
        }


        public ActionResult GuardarTurno(TURNO oTurno)
        {
            DateTime sysdate = DateTime.Now;

            var idUserLog = (USUARIO)Session["Usuario"];
            var dias = "";

            if (!((string)Request["chk_dias"]).IsEmpty())
            {
                dias = ((string)Request["chk_dias"]);
            }


            var newTurno = new TURNO()
            {
                idEmpleado = oTurno.idEmpleado,
                dias = dias,
                horaEntrada = oTurno.horaEntrada,
                horaSalida = oTurno.horaSalida,
                fechaRegistro = sysdate,
                idEncargado = idUserLog.id,
            };

            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    var insertTurno = db.TURNO.Add(newTurno);
                    db.SaveChanges();

                    if (insertTurno == null)
                    {
                        ViewBag.Error = "No se pudo registrar el turno.";
                        return View();
                    }

                    ViewBag.Msg = "Se ha registrado el turno.";
                    return RedirectToAction("Index", "Turnos");

                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

        }


        public ActionResult EliminarTurno(int id)
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {

                    var deleteTurno = (from d in db.TURNO
                                          where d.id == id
                                          select d).FirstOrDefault();

                    db.TURNO.Remove(deleteTurno);
                    db.SaveChanges();

                    if (deleteTurno == null)
                    {
                        ViewBag.Error = "No se pudo eliminar el turno.";
                        return View();
                    }

                    ViewBag.Msg = "Se ha eliminado el turno.";
                    return RedirectToAction("Index", "Turnos");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }


        /**************************/

        public ActionResult GetEventData()
        {
            using (Models.Entities db = new Models.Entities())
            {
                var eventos = (from e in db.EVENTO
                               select e).ToList();

                return new JsonResult { Data = eventos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public ActionResult GetEventDetailByEventId(int id)
        {
            using (Models.Entities db = new Models.Entities())
            {
                var eventos = (from e in db.EVENTO
                               where e.id == id
                               select e).ToList();

                return new JsonResult { Data = eventos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public void UpdateEventDetails(int id, DateTime startDt, DateTime endDt)
        {
            using (Models.Entities db = new Models.Entities())
            {
                var evento = (from e in db.EVENTO
                               where e.id == id
                               select e).FirstOrDefault();

                evento.horaEntrada = startDt;
                evento.horaSalida = endDt;

                db.SaveChanges();

            }
        }



    }
}