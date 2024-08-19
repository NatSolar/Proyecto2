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
    public class UsuariosController : Controller
    {
        // GET: Usuarios
        public ActionResult Index()
        {
            return View();
        }

        /***************** VISTAS ***************/

        [AutorizarUsuario(idOperacion: 2)]
        public ActionResult GestionUsuarios()
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {

                    var empleadosList = (from usuario in db.USUARIO
                                         orderby usuario.fechaCreacion
                                         select usuario).ToList();

                    if (empleadosList == null)
                    {
                        ViewBag.Error = "No hay empleados registrados.";
                        return View();
                    }

                    foreach (USUARIO empleado in empleadosList)
                    {
                        var oRol = (from rol in db.ROL
                                    where rol.id == empleado.idRol
                                    select rol).FirstOrDefault();
                        empleado.ROL = oRol;
                    }

                    return View(empleadosList);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        
        [AutorizarUsuario(idOperacion: 1)]
        public ActionResult RegistrarUsuario()
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    var oRoles = (from roles in db.ROL
                                  select roles).ToList();

                    if (oRoles == null)
                    {
                        ViewBag.Error = "No hay roles registrados.";
                        return RedirectToAction("GestionUsuarios", "Usuarios");
                    }

                    return View(oRoles);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

       
        [AutorizarUsuario(idOperacion: 3)]
        public ActionResult EditarUsuario(int id)
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    USUARIO userLog = (USUARIO)Session["Usuario"];

                    var usuarioInfo = (from usuario in db.USUARIO
                                       where usuario.id == id
                                       select usuario).FirstOrDefault();

                    if (usuarioInfo == null)
                    {
                        ViewBag.Error = "No existe el empleado con el id: " + id;
                        return View();
                    }

                    var oRol = (from rol in db.ROL
                                where rol.id == usuarioInfo.idRol
                                select rol).FirstOrDefault();

                    var oUsuarioOperaciones = (from op in db.USUARIO_OPERACIONES
                                               where op.idUsuario == usuarioInfo.id
                                               select op).ToList();

                    var oOperaciones = (from a in db.ROL_OPERACION
                                        join b in db.OPERACIONES on a.idOperacion equals b.id
                                        join c in db.MODULO on b.idModulo equals c.id
                                        where a.idRol == usuarioInfo.idRol
                                        select new OperacionesMdl()
                                        {
                                            idRol = a.idRol.Value,
                                            idOperacion = a.idOperacion.Value,
                                            nombreOperacion = b.nombre,
                                            idModulo = b.idModulo.Value,
                                            nombreModulo = c.nombre,
                                            selected = false

                                        }).ToList();

                    var oDepartamento = (from departamento in db.DEPARTAMENTO
                                         where departamento.id == usuarioInfo.idDepartamento
                                         select departamento).FirstOrDefault();

                    var oPuesto = (from puesto in db.PUESTO
                                   where puesto.id == usuarioInfo.idPuesto
                                   select puesto).FirstOrDefault();

                    usuarioInfo.ROL = oRol;
                    usuarioInfo.DEPARTAMENTO = oDepartamento;
                    usuarioInfo.PUESTO = oPuesto;

                    /** DATA Referencia **/

                    var rolesList = (from roles in db.ROL
                                     select roles).ToList();

                    var departamentosList = (from departamentos in db.DEPARTAMENTO
                                     select departamentos).ToList();

                    var puestosList = (from puestos in db.PUESTO where puestos.idDepartamento == usuarioInfo.idDepartamento
                                             select puestos).ToList();


                    List<Genero> generoList = new List<Genero>();
                    generoList.Add(new Genero(1, "Femenino"));
                    generoList.Add(new Genero(2, "Masculino"));


                    var oUsuarioLogueadoOperaciones = (from op in db.USUARIO_OPERACIONES
                                               where op.idUsuario == userLog.id
                                               select op).ToList();

                    var has4Rol = "N";
                    var has5Rol = "N";

                    foreach (var op in oUsuarioLogueadoOperaciones)
                    {
                        if(op.idOperacion == 4)
                        {
                            has4Rol = "Y";
                            continue;
                        }

                        if (op.idOperacion == 5)
                        {
                            has5Rol = "Y";
                            continue;
                        }
                    }

                    ViewBag.RolesList = rolesList;
                    ViewBag.DepartamentosList = departamentosList;
                    ViewBag.PuestosList = puestosList;
                    ViewBag.GeneroList = generoList;

                    ViewBag.OperacionesList = oOperaciones;
                    ViewBag.UsuarioOperacionesList = oUsuarioOperaciones;
                    ViewBag.Has4Rol = has4Rol;
                    ViewBag.Has5Rol = has5Rol;

                    return View(usuarioInfo);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

       
        [AutorizarUsuario(idOperacion: 2)]
        public ActionResult DetallesUsuario(int id)
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {

                    var usuarioInfo = (from usuario in db.USUARIO
                                       where usuario.id == id
                                       select usuario).FirstOrDefault();

                    if (usuarioInfo == null)
                    {
                        ViewBag.Error = "No existe el empleado con el id: " + id;
                        return View();
                    }

                    var oRol = (from rol in db.ROL
                                where rol.id == usuarioInfo.idRol
                                select rol).FirstOrDefault();

                    var oDepartamento = (from departamento in db.DEPARTAMENTO
                                       where departamento.id == usuarioInfo.idDepartamento
                                         select departamento).FirstOrDefault();

                    var oPuesto = (from puesto in db.PUESTO
                                         where puesto.id == usuarioInfo.idPuesto
                                         select puesto).FirstOrDefault();

                    var oOperacionesUsuario = (from op in db.USUARIO_OPERACIONES
                                   where op.idUsuario == usuarioInfo.id
                                   select op).ToList();

                    var oOperaciones = (from a in db.ROL_OPERACION
                                        join b in db.OPERACIONES on a.idOperacion equals b.id
                                        join c in db.MODULO on b.idModulo equals c.id
                                        where a.idRol == usuarioInfo.idRol
                                        select new OperacionesMdl()
                                        {
                                            idRol = a.idRol.Value,
                                            idOperacion = a.idOperacion.Value,
                                            nombreOperacion = b.nombre,
                                            idModulo = b.idModulo.Value,
                                            nombreModulo = c.nombre,
                                            selected = false

                                        }).ToList();

                    usuarioInfo.ROL = oRol;
                    usuarioInfo.DEPARTAMENTO = oDepartamento;
                    usuarioInfo.PUESTO = oPuesto;

                    ViewBag.OperacionesUsuarioList = oOperacionesUsuario;
                    ViewBag.OperacionesGeneral = oOperaciones;

                    return View(usuarioInfo);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }


        /*************** OPERACIONES *************/
        public ActionResult CrearUsuario(USUARIO oUsuario)
        {
            DateTime sysdate = DateTime.Now;

            var usuarioData = new USUARIO()
            {
                nombre = oUsuario.nombre,
                apellido = oUsuario.apellido,
                email = oUsuario.email,
                contrasena = oUsuario.contrasena,
                direccion = oUsuario.direccion,
                telefono = oUsuario.telefono,
                fechaCreacion = sysdate,
                idRol = oUsuario.idRol,
                fechaNacimiento = oUsuario.fechaNacimiento,
                idGenero = oUsuario.idGenero,
                nacionalidad = oUsuario.nacionalidad,
                idDepartamento = oUsuario.idDepartamento,
                idPuesto = oUsuario.idPuesto,
                fechaUnion = oUsuario.fechaUnion,
                salarioBase = oUsuario.salarioBase
            };

            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    var insertUsuario = db.USUARIO.Add(usuarioData);
                    db.SaveChanges();

                    string[] operacionesRol = new string[] { };
                    if (!((string)Request["operaciones_chks"]).IsEmpty())
                    {
                        operacionesRol = ((string)Request["operaciones_chks"]).Split(',');

                        foreach (var nuevosRoles in operacionesRol)
                        {
                            USUARIO_OPERACIONES uop = new USUARIO_OPERACIONES()
                            {
                                idUsuario = insertUsuario.id,
                                idRol = insertUsuario.idRol,
                                idOperacion = Convert.ToInt32(nuevosRoles),
                                hasIt = true,
                            };

                            var insertUsuarioOp = db.USUARIO_OPERACIONES.Add(uop);
                            db.SaveChanges();

                        }

                    }

                    if (insertUsuario == null)
                    {
                        ViewBag.Error = "No se pudo registrar el empleado.";
                        return View();
                    }

                    ViewBag.Msg = "Se ha registrado el usuario.";
                    return RedirectToAction("GestionUsuarios", "Usuarios");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

        }

        public ActionResult EliminarUsuario(int id)
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    var deleteUsuario = (from usuario in db.USUARIO
                                         where usuario.id == id
                                         select usuario).FirstOrDefault();
                    db.USUARIO.Remove(deleteUsuario);
                    db.SaveChanges();

                    if (deleteUsuario == null)
                    {
                        ViewBag.Error = "No se pudo registrar el empleado.";
                        return View();
                    }

                    ViewBag.Msg = "Se ha eliminado el usuario.";
                    return RedirectToAction("GestionUsuarios", "Usuarios");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public ActionResult ModificarUsuario(USUARIO oUsuario)
        {
            try
            {
                using (Models.Entities db = new Models.Entities())
                {

                    var usuarioInfo = (from usuario in db.USUARIO
                                       where usuario.id == oUsuario.id
                                       select usuario).FirstOrDefault();

                    usuarioInfo.nombre = oUsuario.nombre;
                    usuarioInfo.apellido = oUsuario.apellido;
                    usuarioInfo.email = oUsuario.email;
                    usuarioInfo.direccion = oUsuario.direccion;
                    usuarioInfo.contrasena = oUsuario.contrasena;
                    usuarioInfo.telefono = oUsuario.telefono;
                    usuarioInfo.idRol = oUsuario.idRol;
                    usuarioInfo.fechaNacimiento = oUsuario.fechaNacimiento;
                    usuarioInfo.idGenero = oUsuario.idGenero;
                    usuarioInfo.nacionalidad = oUsuario.nacionalidad;
                    usuarioInfo.idDepartamento = oUsuario.idDepartamento;
                    usuarioInfo.idPuesto = oUsuario.idPuesto;
                    usuarioInfo.fechaUnion = oUsuario.fechaUnion;
                    usuarioInfo.salarioBase = oUsuario.salarioBase;

                    db.SaveChanges();

                    string[] operacionesRol = new string[] { };
                    if (!((string)Request["operaciones_chks"]).IsEmpty())
                    {
                        operacionesRol = ((string)Request["operaciones_chks"]).Split(',');

                        var deleteUsuarioOperaciones = (from uo in db.USUARIO_OPERACIONES
                                                  where uo.idUsuario == oUsuario.id
                                                  select uo).ToList();

                        foreach(var operacion in deleteUsuarioOperaciones)
                        {
                            db.USUARIO_OPERACIONES.Remove(operacion);
                            db.SaveChanges();
                        }

                        foreach(var nuevosRoles in operacionesRol)
                        {
                            USUARIO_OPERACIONES uop = new USUARIO_OPERACIONES()
                            {
                                idUsuario = oUsuario.id,
                                idRol = oUsuario.idRol,
                                idOperacion = Convert.ToInt32(nuevosRoles),
                                hasIt = true,
                            };

                            var insertUsuarioOp = db.USUARIO_OPERACIONES.Add(uop);
                            db.SaveChanges();

                        }

                    }

                    if (usuarioInfo == null)
                    {
                        ViewBag.Error = "No existe el empleado con el id: " + oUsuario.id;
                        return View();
                    }

                    return RedirectToAction("EditarUsuario", "Usuarios", new { id = usuarioInfo.id });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index", "Home");
            }
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
                        foreach(var item in oDepartamentos)
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
        public JsonResult ObtenerPuestos(int idDepartamento)
        {
            List<PuestoMdl> olista = new List<PuestoMdl>();

            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    var oPuestos = (from puestos in db.PUESTO where puestos.idDepartamento==idDepartamento
                                          select puestos).ToList();

                    if (oPuestos != null)
                    {
                        foreach (var item in oPuestos)
                        {
                            PuestoMdl puesto = new PuestoMdl();
                            puesto.id = item.id;
                            puesto.idDepartamento = item.idDepartamento.GetValueOrDefault();
                            puesto.nombre = item.nombre;
                            olista.Add(puesto);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                olista = new List<PuestoMdl>();
            }

            return Json(new { lista = olista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult TraerOperaciones(int idRol)
        {
            List<OperacionesMdl> olista = new List<OperacionesMdl>();

            try
            {
                using (Models.Entities db = new Models.Entities())
                {
                    var oOperaciones = (from op in db.ROL_OPERACION
                                    where op.idRol == idRol
                                    select op).ToList();

                    if (oOperaciones != null)
                    {
                        foreach (var item in oOperaciones)
                        {

                            var operacionInfo = (from o in db.OPERACIONES
                                                 where o.id == item.idOperacion
                                                 select o).FirstOrDefault();

                            OperacionesMdl operacion = new OperacionesMdl();
                            operacion.idRol = item.idRol.Value;
                            operacion.idOperacion = item.idOperacion.Value;
                            operacion.nombreOperacion = operacionInfo.nombre;
                            olista.Add(operacion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                olista = new List<OperacionesMdl>();
            }

            return Json(new { lista = olista }, JsonRequestBehavior.AllowGet);
        }


    }
}