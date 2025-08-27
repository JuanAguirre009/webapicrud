
using Microsoft.EntityFrameworkCore;
using WebApiProducto.Models;

namespace WebApiProducto.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        } 
       public DbSet<Producto> Productos { get; set; }
    }
}