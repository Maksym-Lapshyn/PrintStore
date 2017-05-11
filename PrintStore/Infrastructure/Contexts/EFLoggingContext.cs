namespace PrintStore.Infrastructure.Contexts
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using PrintStore.Models;

    public partial class EFLoggingContext : DbContext
    {
        public EFLoggingContext()
            : base("name=EFLoggingContext")
        {
            
        }

        public DbSet<ExceptionDetail> ExceptionDetails { get; set; }
        public DbSet<ActionDetail> ActionDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
