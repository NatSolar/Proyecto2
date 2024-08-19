using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto2.Models
{
    public class OperacionesMdl
    {
        public int idRol {  get; set; }
        public int idOperacion { get; set; }
        public string nombreOperacion { get; set; }
        public int idModulo { get; set; }
        public string nombreModulo { get; set; }

        public bool selected { get; set; }

        public OperacionesMdl() { }

        public OperacionesMdl(int id_rol, int id_operacion, string nombre_operacion, int id_modulo, string nombre_modulo, bool selected)
        {
            this.idModulo = id_modulo;
            this.idRol = id_rol;
            this.idOperacion = id_operacion;
            this.nombreOperacion = nombre_operacion;
            this.nombreModulo = nombre_modulo;
            this.selected = selected;
        }
    }
}