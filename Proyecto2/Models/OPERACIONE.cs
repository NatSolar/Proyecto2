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
    
    public partial class OPERACIONE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OPERACIONE()
        {
            this.ROL_OPERACION = new HashSet<ROL_OPERACION>();
            this.USUARIO_OPERACIONES = new HashSet<USUARIO_OPERACIONES>();
        }
    
        public int id { get; set; }
        public string nombre { get; set; }
        public Nullable<int> idModulo { get; set; }
    
        public virtual MODULO MODULO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ROL_OPERACION> ROL_OPERACION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO_OPERACIONES> USUARIO_OPERACIONES { get; set; }
    }
}
