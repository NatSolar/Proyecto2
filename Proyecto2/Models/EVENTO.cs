//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Proyecto2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EVENTO
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string informacion { get; set; }
        public System.DateTime horaEntrada { get; set; }
        public Nullable<System.DateTime> horaSalida { get; set; }
    }
}
