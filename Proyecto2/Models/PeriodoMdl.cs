using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto2.Models
{
    public class PeriodoMdl
    {
        public int id { get; set; }
        public string detalle { get; set; }
        public System.DateTime desde { get; set; }
        public System.DateTime hasta { get; set; }
        public int ano { get; set; }

        public PeriodoMdl() { }

        public PeriodoMdl(int id, string detalle, DateTime desde, DateTime hasta, int ano)
        {
            this.id = id;
            this.detalle = detalle;
            this.desde = desde;
            this.hasta = hasta;
            this.ano = ano;
        }

    }
}