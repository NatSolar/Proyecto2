﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BONIFICACIONE> BONIFICACIONES { get; set; }
        public virtual DbSet<DEDUCCIONE> DEDUCCIONES { get; set; }
        public virtual DbSet<DEPARTAMENTO> DEPARTAMENTO { get; set; }
        public virtual DbSet<EVALUACION> EVALUACION { get; set; }
        public virtual DbSet<EVENTO> EVENTO { get; set; }
        public virtual DbSet<METODO_PAGO> METODO_PAGO { get; set; }
        public virtual DbSet<MODULO> MODULO { get; set; }
        public virtual DbSet<NOMINA> NOMINA { get; set; }
        public virtual DbSet<OBJETIVO> OBJETIVOS { get; set; }
        public virtual DbSet<OPERACIONE> OPERACIONES { get; set; }
        public virtual DbSet<PERIODO> PERIODO { get; set; }
        public virtual DbSet<PUESTO> PUESTO { get; set; }
        public virtual DbSet<ROL> ROL { get; set; }
        public virtual DbSet<ROL_OPERACION> ROL_OPERACION { get; set; }
        public virtual DbSet<TURNO> TURNO { get; set; }
        public virtual DbSet<USUARIO> USUARIO { get; set; }
        public virtual DbSet<USUARIO_OPERACIONES> USUARIO_OPERACIONES { get; set; }
    }
}
