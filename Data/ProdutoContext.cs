using Curso.Domain;
using Microsoft.EntityFrameworkCore;

namespace Curso.Data
{
    public class ProdutoContext : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("CONNECTION_STRING");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>().HasQueryFilter(p => p.Ativo == true);
        }
    }
}
