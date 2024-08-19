using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto2.Models
{
    public class ObjetivosMdl
    {
        public int id { get; set; }
        public int idEmpleado { get; set; }
        public int idPeriodo { get; set; }
        public string detalle { get; set; }
        public int valorReferencia { get; set; }
        public int peso { get; set; }
        public int nota { get; set; }
        public string retroalimentacion { get; set; }
        public System.DateTime fechaCreacion { get; set; }
        public int idEvaluador { get; set; }

        public ObjetivosMdl() { }

        public ObjetivosMdl(int id, int idEmpleado, int idPeriodo, string detalle, int valorReferencia, int peso, int nota, string retroalimentacion, DateTime fechaCreacion, int idEvaluador)
        {
            this.id = id;
            this.idEmpleado = idEmpleado;
            this.idPeriodo = idPeriodo;
            this.detalle = detalle;
            this.valorReferencia = valorReferencia;
            this.peso = peso;
            this.nota = nota;
            this.retroalimentacion = retroalimentacion;
            this.fechaCreacion = fechaCreacion;
            this.idEvaluador = idEvaluador;
        }

    }
}