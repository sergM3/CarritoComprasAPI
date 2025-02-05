using Microsoft.EntityFrameworkCore;
using CarritoComprasAPI.Models;
namespace CarritoComprasAPI.Data
{
    public class CarritoContext : DbContext
    {
        public CarritoContext(DbContextOptions<CarritoContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<CarritoProducto> CarritoProductos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarritoProducto>()
                .HasKey(cp => new { cp.CarritoId, cp.ProductoId });

            modelBuilder.Entity<CarritoProducto>()
                .HasOne(cp => cp.Carrito)
                .WithMany(c => c.ProductosAgregados)
                .HasForeignKey(cp => cp.CarritoId);

            modelBuilder.Entity<CarritoProducto>()
                .HasOne(cp => cp.Producto)
                .WithMany()
                .HasForeignKey(cp => cp.ProductoId);
        }
    }

}


