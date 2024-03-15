using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MvcCoreTiendaZapatillas.Models;
using MvcCoreTiendaZapatillas.Repositories;


namespace MvcCoreTiendaZapatillas.Controllers
{
    public class ZapatillasController : Controller
    {
        private RepositoryZapatillas repo;

        public ZapatillasController(RepositoryZapatillas repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Zapatilla> zapatillas = await this.repo.GetAllZapatillasAsync();
            return View(zapatillas);
        }

        public async Task<IActionResult> Details(int? posicion, int zapatilla)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            Zapatilla zapato = await this.repo.GetZapatillaByIdAsync(zapatilla);
            ImagenesZapatilla imagenes = await this.repo.GetImagenesZapatillaPaginacionAsync(posicion.Value, zapato.IdProducto);
            List<ImagenesZapatilla> total = await this.repo.GetAllImagenesByZapatoAsync(zapatilla);
            int numeroRegistros = total.Count;

            int siguiente = posicion.Value + 1;
            if (siguiente > numeroRegistros)
            {
                siguiente = numeroRegistros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }

            ViewData["ÚLTIMO"] = numeroRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["imagen"] = imagenes;
            ViewData["POSICION"] = posicion;

            return View(zapato);
        }

        public async Task<IActionResult> _Zapatilla(int? posicion, int zapatilla)
        {
            if (posicion == null)
            {
                posicion = 1;
            }

            Zapatilla zapato = await this.repo.GetZapatillaByIdAsync(zapatilla);
            ImagenesZapatilla imagenes = await this.repo.GetImagenesZapatillaPaginacionAsync(posicion.Value, zapato.IdProducto);
            List<ImagenesZapatilla> total = await this.repo.GetAllImagenesByZapatoAsync(zapatilla);
            int numeroRegistros = total.Count;

            int siguiente = posicion.Value + 1;
            if (siguiente > numeroRegistros)
            {
                siguiente = numeroRegistros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }


            ViewData["ÚLTIMO"] = numeroRegistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["imagen"] = imagenes;
            ViewData["numregistro"] = siguiente;
            ViewData["POSICION"] = posicion;

            return PartialView("_Zapatilla", zapato);
        }

        public async Task<IActionResult> CrearImagenZapato()
        {
            List<Zapatilla> zapatillas = await this.repo.GetAllZapatillasAsync();
            return View(zapatillas);
        }

        [HttpPost]
        public async Task<IActionResult> CrearImagenZapato(List<string> imagenes, int zapatilla)
        {
            await this.repo.InsertarImagenZapato(imagenes, zapatilla);
            return RedirectToAction("Details", new { zapatilla = zapatilla });
        }
    }
}
