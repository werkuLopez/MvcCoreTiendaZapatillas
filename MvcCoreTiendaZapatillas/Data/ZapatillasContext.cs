using Microsoft.EntityFrameworkCore;
using MvcCoreTiendaZapatillas.Models;

namespace MvcCoreTiendaZapatillas.Data
{
    public class ZapatillasContext: DbContext
    {
        public ZapatillasContext(DbContextOptions<ZapatillasContext> options) : base(options) { }

        public DbSet<Zapatilla> Zapatillas { get; set; }
        public DbSet<ImagenesZapatilla> Imagenes { get; set; }

    }
}
