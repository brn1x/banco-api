using Microsoft.EntityFrameworkCore;

namespace BancoWeb.Model.Entity
{
  public class DbConn : DbContext
  {
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseNpgsql("Host=localhost;Database=banco;Username=postgres;Port=5432;Password=root");
      optionsBuilder.UseLazyLoadingProxies();
    }

    public DbSet<Agencia> Agencias { get; set; }
    public DbSet<Conta> Contas { get; set; }
    public DbSet<Cliente> Clientes { get; set; }

  }
}