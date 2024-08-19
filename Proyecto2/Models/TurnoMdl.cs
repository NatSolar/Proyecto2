using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto2.Models
{
    public class TurnoMdl
    {
        public int id { get; set; }
        public int idEmpleado { get; set; }
        public string dias { get; set; }
        public string horaEntrada { get; set; }
        public string horaSalida { get; set; }
        public System.DateTime fechaRegistro { get; set; }
        public int idEncargado { get; set; }

        public double horas { get; set; }

        public virtual USUARIO USUARIO { get; set; }
        public virtual USUARIO USUARIO1 { get; set; }

    }
}