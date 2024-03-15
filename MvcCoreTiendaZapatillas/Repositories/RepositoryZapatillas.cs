using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCoreTiendaZapatillas.Data;
using MvcCoreTiendaZapatillas.Models;
using System.Diagnostics.Metrics;

#region PROCEDIMIENTOS
//CREATE PROCEDURE SP_ZAPATILLAS_INDIVIDUAL
//(@posicion INT)
//AS
//	SELECT IDPRODUCTO, NOMBRE, DESCRIPCION, PRECIO FROM
//	(SELECT 
//	CAST(
//		ROW_NUMBER() OVER (ORDER BY NOMBRE) AS INT) AS POSICION,
//    IDPRODUCTO, NOMBRE, DESCRIPCION, PRECIO FROM ZAPASPRACTICA) AS QUERY
//	WHERE QUERY.POSICION=@posicion
//GO

//ALTER PROCEDURE SP_IMAGES_ZAPATILLAS_INDIVIDUAL
//(@posicion INT, @idzapatilla INT)
//AS
//	SELECT IDIMAGEN, IDPRODUCTO, IMAGEN FROM
//	(SELECT 
//	CAST(
//		ROW_NUMBER() OVER (ORDER BY IMAGEN) AS INT) AS POSICION,
//    IDIMAGEN, IDPRODUCTO, IMAGEN FROM IMAGENESZAPASPRACTICA
	
//	WHERE IDPRODUCTO=@idzapatilla
//	) AS QUERY
//	WHERE QUERY.POSICION=@posicion
//GO
#endregion

namespace MvcCoreTiendaZapatillas.Repositories
{
    public class RepositoryZapatillas
    {
        private ZapatillasContext context;

        public RepositoryZapatillas(ZapatillasContext context)
        {
            this.context = context;
        }

        public async Task<int> GetNumeroRegistros()
        {
            List<Zapatilla> zapatillas = await this.context.Zapatillas.ToListAsync();
            int cantidaa = zapatillas.Count;
            return cantidaa;
        }
        public async Task<List<Zapatilla>> GetZapatillasPaginacion(int posicion)
        {
            string sql = "SP_ZAPATILLAS_INDIVIDUAL @posicion";
            SqlParameter paramPosicion = new SqlParameter("@posicion", posicion);
            var consulta = this.context.Zapatillas.FromSqlRaw(sql, paramPosicion);

            return await consulta.ToListAsync();
        }
        public async Task<List<Zapatilla>> GetAllZapatillasAsync()
        {
            return await this.context.Zapatillas.ToListAsync();
        }
        public async Task<Zapatilla> GetZapatillaByIdAsync(int id)
        {
            return await this.context.Zapatillas.FirstOrDefaultAsync(x => x.IdProducto == id);
        }

        public async Task<List<ImagenesZapatilla>> GetAllImagenesByZapatoAsync(int idZapatillo)
        {
            return await this.context.Imagenes.Where(x => x.IdProducto == idZapatillo).ToListAsync();
        }

        public async Task<ImagenesZapatilla> GetImagenesZapatillaPaginacionAsync(int posicion, int zapatilla)
        {
            string sql = "SP_IMAGES_ZAPATILLAS_INDIVIDUAL @posicion, @idzapatilla";
            SqlParameter paramPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter paramZapatilla = new SqlParameter("@idzapatilla", zapatilla);

            var consulta = this.context.Imagenes.FromSqlRaw(sql, paramPosicion, paramZapatilla);
            var data = await consulta.ToListAsync();
            ImagenesZapatilla imagen = data.FirstOrDefault();

            return imagen;
        }

        internal async Task<List<ImagenesZapatilla>> GetAllImagenesAsync()
        {
            return await this.context.Imagenes.ToListAsync();
        }

        public async Task InsertarImagenZapato(List<string> imagenes, int zapatilla)
        {
            foreach(string img in imagenes)
            {
                var consulta = from datos in this.context.Imagenes
                               select datos;
                int nextId = (consulta.Max(x => x.IdImagen)) + 1;

                await this.context.Imagenes.AddAsync(new ImagenesZapatilla
                {
                    IdImagen = nextId,
                    IdProducto = zapatilla,
                    Imagen = img
                });
                await this.context.SaveChangesAsync(); 
            }
        }
    }
}
