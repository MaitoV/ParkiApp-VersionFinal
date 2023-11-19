using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCBasic.Models;
using MVCBasico.Context;

namespace MVCBasic.Controllers
{
    public class CocheraController : Controller
    {
        private readonly EscuelaDatabaseContext _context;

        public CocheraController(EscuelaDatabaseContext context)
        {
            _context = context;
        }

        // GET: Cochera
        public async Task<IActionResult> Index()
        {
              return _context.Cocheras != null ? 
                          View(await _context.Cocheras.ToListAsync()) :
                          Problem("Entity set 'EscuelaDatabaseContext.Cocheras'  is null.");
        }

        // GET: Cochera/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cocheras == null)
            {
                return NotFound();
            }

            var cochera = await _context.Cocheras
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cochera == null)
            {
                return NotFound();
            }

            return View(cochera);
        }

        // GET: Cochera/Create
        public IActionResult Create()
        {
            var vehiculos = _context.Vehiculos.Select(v => new { v.Id, v.Patente}).ToList();
            var listaVehiculos = new SelectList(vehiculos, "Id", "Patente");

            ViewBag.ListaVehiculos = listaVehiculos;

            return View();
        }

        // POST: Cochera/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroCochera,Piso,FechaIngreso,TipoCochera,VehiculoId")] Cochera cochera)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cochera);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cochera);
        }

        // GET: Cochera/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cocheras == null)
            {
                return NotFound();
            }

            var cochera = await _context.Cocheras.FindAsync(id);
            if (cochera == null)
            {
                return NotFound();
            }
            return View(cochera);
        }

        // POST: Cochera/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroCochera,Piso,FechaIngreso,TipoCochera,VehiculoId")] Cochera cochera)
        {
            if (id != cochera.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cochera);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CocheraExists(cochera.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cochera);
        }

        // GET: Cochera/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cocheras == null)
            {
                return NotFound();
            }

            var cochera = await _context.Cocheras
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cochera == null)
            {
                return NotFound();
            }

            return View(cochera);
        }

        // POST: Cochera/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cocheras == null)
            {
                return Problem("Entity set 'EscuelaDatabaseContext.Cocheras'  is null.");
            }
            var cochera = await _context.Cocheras.FindAsync(id);
            if (cochera != null)
            {
                _context.Cocheras.Remove(cochera);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: FORMULARIO CARGA DE COCHERAS
        public IActionResult Carga()
        {
            return View();
        }
        // POST - CREAR COCHERAS
        [HttpPost]
        public IActionResult CrearCocheras(IFormCollection form)
        {
            Random rnd = new Random();

            foreach (Piso piso in Enum.GetValues(typeof(Piso)))
            {
                foreach (TipoVehiculo tipoVehiculo in Enum.GetValues(typeof(TipoVehiculo)))
                {
                    foreach (TipoCochera tipoCochera in Enum.GetValues(typeof(TipoCochera)))
                    {

                        var inputName = $"{piso}-{tipoVehiculo}-{tipoCochera}";
                        if (form.ContainsKey(inputName) && int.TryParse(form[inputName], out int cantidadEspacios))
                        {
                            for (int i = 0; i < cantidadEspacios; i++)
                            {
                                
                                 var nuevaCochera = new Cochera
                                {
                                    NumeroCochera = rnd.Next(1, 101),
                                    Piso = piso,
                                    TipoVehiculo = tipoVehiculo,
                                    TipoCochera = tipoCochera,
                                };

                                _context.Cocheras.Add(nuevaCochera);
                            }
                    }
                    }
                }
            }
            _context.SaveChanges();

            return RedirectToAction("Dashboard", "Empleado");
        }

         private bool CocheraExists(int id)
        {
          return (_context.Cocheras?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
