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
    
    public partial class USUARIO_OPERACIONES
    {
        public int id { get; set; }
        public Nullable<int> idUsuario { get; set; }
        public Nullable<int> idRol { get; set; }
        public Nullable<int> idOperacion { get; set; }
        public Nullable<bool> hasIt { get; set; }
    
        public virtual OPERACIONE OPERACIONE { get; set; }
        public virtual ROL ROL { get; set; }
        public virtual USUARIO USUARIO { get; set; }
    }
}
