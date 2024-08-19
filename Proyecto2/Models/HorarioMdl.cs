using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto2.Models
{
    public class HorarioMdl
    {
        public int idEmpleado {  get; set; }

        public string dia {  get; set; }

        public string horaEntrada { get; set; }
        public string horaSalida { get; set; }

        public virtual USUARIO USUARIO { get; set; }
    }
}