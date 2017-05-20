using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using PrintStore.Models;

namespace PrintStore.Infrastructure.Contexts
{
    /// <summary>
    /// Logging context for Entity Framework
    /// </summary>
    public class EFLoggingContext : DbContext
    {
        public DbSet<ExceptionDetail> ExceptionDetails { get; set; }
        public DbSet<ActionDetail> ActionDetails { get; set; }
    }
}
