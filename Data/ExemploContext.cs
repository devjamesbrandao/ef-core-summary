using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ef_core.Data
{
    public class ExemploContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conexao = "Data source=(localdb)\\mssqllocaldb; Initial Catalog = Estudos; Integrated Security=true; pooling=true;";
            optionsBuilder
                .UseSqlServer(conexao)
                .EnableSensitiveDataLogging() // Exibe os dados sensíveis das queries no console
                .LogTo(Console.WriteLine, LogLevel.Information); // Log das queries no console da aplicação
        }
    }
}