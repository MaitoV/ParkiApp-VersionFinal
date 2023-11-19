using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCBasic.Models;
using MVCBasico.Context;

namespace MVCBasic.Controllers
{
    public class EmpleadoController : Controller
    {
        private const string SessionID = "_UserID";
        private const string SessionName = "_UserName";
        private readonly EscuelaDatabaseContext _context;

        public EmpleadoController(EscuelaDatabaseContext context)
        {
            _context = context;
        }

        // GET: Empleado
        public async Task<IActionResult> Index()
        {
              return _context.Empleados != null ? 
                          View(await _context.Empleados.ToListAsync()) :
                          Problem("Entity set 'EscuelaDatabaseContext.Empleados'  is null.");
        }

        // GET: Empleado/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleado/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleado/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumeroLegajo,Turno,Id,Nombre,Apellido,password,Telefono")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        // GET: Empleado/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleado/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumeroLegajo,Turno,Id,Nombre,Apellido,password,Telefono")] Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.Id))
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
            return View(empleado);
        }

        // GET: Empleado/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empleados == null)
            {
                return Problem("Entity set 'EscuelaDatabaseContext.Empleados'  is null.");
            }
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Login()
        {
            return View();
        }
        // POST: LOGIN DE USUARIO
        [HttpPost]
        public IActionResult Login(Empleado model)
        {
            var usuario = _context.Empleados.FirstOrDefault(u => u.NumeroLegajo == model.NumeroLegajo);

            if (usuario != null && model.password == usuario.password)
            {
                //HttpContext.Session.SetString(SessionName, "Jarvik");
                //HttpContext.Session.SetInt32(SessionID, 24);
                /*string usuarioJson = JsonConvert.SerializeObject(model);
                HttpContext.Session.SetString("usuario", usuarioJson);*/
                return RedirectToAction(nameof(Dashboard));
            }

            ViewData["LoginError"] = "Credenciales inválidas, intente nuevamente";

            return View(model);
        }
        // GET: DASHBOARD EMPLEADO
        public IActionResult Dashboard()
        {
            var cantidadCocheras = _context.Cocheras.Count();
            if(cantidadCocheras != 0)
            {
                var cocherasPorTipo = _context.Cocheras.GroupBy(c => new { c.TipoVehiculo, c.TipoCochera }).Select(g => new
                {
                    TipoVehiculo = g.Key.TipoVehiculo,
                    TipoCochera = g.Key.TipoCochera,
                    Cantidad = g.Count()
                }).ToList();
                var mensaje = "Hay ";
                foreach (var grupo in cocherasPorTipo)
                {
                    mensaje += $"{grupo.Cantidad} cocheras {grupo.TipoCochera.ToString().ToLower()} para {grupo.TipoVehiculo.ToString().ToLower()}, ";
                }

                ViewBag.Mensaje = mensaje.TrimEnd(',', ' ') + ".";
            }
            ViewBag.TotalCocheras = cantidadCocheras;
            return View();
        }


        private bool EmpleadoExists(int id)
        {
          return (_context.Empleados?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
