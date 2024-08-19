using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto2.Models
{
    public class Genero
    {
        public int id { get; set; }
        public string nombre { get; set; }

        public Genero() { }

        public Genero(int id, string nombre)
        {
            this.id = id;
            this.nombre = nombre;
        }
    }

}