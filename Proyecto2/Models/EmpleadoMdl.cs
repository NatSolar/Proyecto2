using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto2.Models
{
    public class EmpleadoMdl
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }

        public EmpleadoMdl() { }

        public EmpleadoMdl(int id, string nombre, string apellido)
        {
            this.id = id;
            this.nombre = nombre;
            this.apellido = apellido;
        }

    }
}