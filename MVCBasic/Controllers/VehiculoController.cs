﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCBasic.Migrations;
using MVCBasic.Models;
using MVCBasico.Context;

namespace MVCBasic.Controllers
{
    public class VehiculoController : Controller
    {
        private const string SessionID = "_UserID";
        private readonly EscuelaDatabaseContext _context;

        public VehiculoController(EscuelaDatabaseContext context)
        {
            _context = context;
        }

        // GET: BUSQUEDA VEHICULOS
        public async Task<IActionResult> Busqueda(String busquedaVehiculo)
        {
            var legajoDeSesion = HttpContext.Session.GetInt32(SessionID);
            if (legajoDeSesion.HasValue && legajoDeSesion != 0)
            {
                var vehiculos = await _context.Vehiculos.Where(v => v.Patente.Contains(busquedaVehiculo)).ToListAsync();

                return View(vehiculos);
            }
            return RedirectToAction("Index", "Home");
        }
        // GET: REGISTRAR VEHICULO
        public IActionResult Registrar(int? cocheraId)
        {
            var legajoDeSesion = HttpContext.Session.GetInt32(SessionID);
            if (legajoDeSesion.HasValue && legajoDeSesion != 0)
            {
                ViewBag.CocheraId = cocheraId;

                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        // POST: REGISTRAR VEHICULO Y COCHERA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarVehiculoCochera(String Patente, TipoVehiculo Tipo, int cocheraId )
        {
            var legajoDeSesion = HttpContext.Session.GetInt32(SessionID);
            if (legajoDeSesion.HasValue && legajoDeSesion != 0)
            {
                var vehiculoExistente = _context.Vehiculos.FirstOrDefault(v => v.Patente == Patente);
                int vehiculoId;
                if (vehiculoExistente != null)
                {
                    vehiculoId = vehiculoExistente.Id;
                }
                else
                {
                    var nuevoVehiculo = new Vehiculo
                    {
                        Patente = Patente,
                        Tipo = Tipo
                    };
                    _context.Vehiculos.Add(nuevoVehiculo);
                    _context.SaveChanges();
                    vehiculoId = nuevoVehiculo.Id;
                }
                var cochera = _context.Cocheras.FirstOrDefault(c => c.Id == cocheraId);
                if (cochera != null)
                {
                    cochera.VehiculoId = vehiculoId;
                    _context.SaveChanges();
                }
                ViewBag.NumeroCochera = cochera.NumeroCochera;
                ViewBag.NumeroPatente = Patente;
                return View("Exito");
            }
            return RedirectToAction("Index", "Home");
        }
        // POST: REGISTRAR VEHICULO
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Patente,Tipo")] Vehiculo vehiculo)
        {
            var legajoDeSesion = HttpContext.Session.GetInt32(SessionID);
            if (legajoDeSesion.HasValue && legajoDeSesion != 0)
            {
                var vehiculoExistente = await _context.Vehiculos.FirstOrDefaultAsync(v => v.Patente == vehiculo.Patente);
                if (vehiculoExistente != null)
                {
                    ViewBag.ErrorMessage = "No se pudo crear. Ya existe un vehículo con esa patente";
                    return View();
                }
                else if (ModelState.IsValid)
                {
                    _context.Add(vehiculo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Dashboard", "Empleado");
                }
                return RedirectToAction("Dashboard", "Empleado");
            }
            return RedirectToAction("Index", "Home");

        }



        // GET: Vehiculo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var legajoDeSesion = HttpContext.Session.GetInt32(SessionID);
            if (legajoDeSesion.HasValue && legajoDeSesion != 0)
            {
                if (id == null || _context.Vehiculos == null)
                {
                    return NotFound();
                }

                var vehiculo = await _context.Vehiculos
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (vehiculo == null)
                {
                    return NotFound();
                }

                return View(vehiculo);
            }
            return RedirectToAction("Index", "Home");
        }


        // GET: Vehiculo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var legajoDeSesion = HttpContext.Session.GetInt32(SessionID);
            if (legajoDeSesion.HasValue && legajoDeSesion != 0)
            {
                if (id == null || _context.Vehiculos == null)
                {
                    return NotFound();
                }

                var vehiculo = await _context.Vehiculos.FindAsync(id);
                if (vehiculo == null)
                {
                    return NotFound();
                }
                return View(vehiculo);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Vehiculo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Patente,Tipo")] Vehiculo vehiculo)
        {
            var legajoDeSesion = HttpContext.Session.GetInt32(SessionID);
            if (legajoDeSesion.HasValue && legajoDeSesion != 0)
            {
                if (id != vehiculo.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(vehiculo);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!VehiculoExists(vehiculo.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Dashboard", "Empleado");
                }
                return View(vehiculo);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Vehiculo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var legajoDeSesion = HttpContext.Session.GetInt32(SessionID);
            if (legajoDeSesion.HasValue && legajoDeSesion != 0)
            {
                if (id == null || _context.Vehiculos == null)
                {
                    return NotFound();
                }

                var vehiculo = await _context.Vehiculos
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (vehiculo == null)
                {
                    return NotFound();
                }

                return View(vehiculo);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Vehiculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var legajoDeSesion = HttpContext.Session.GetInt32(SessionID);
            if (legajoDeSesion.HasValue && legajoDeSesion != 0)
            {
                if (_context.Vehiculos == null)
                {
                    return Problem("Entity set 'EscuelaDatabaseContext.Vehiculos'  is null.");
                }
                var vehiculo = await _context.Vehiculos.FindAsync(id);
                if (vehiculo != null)
                {
                    _context.Vehiculos.Remove(vehiculo);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard", "Empleado");
            }
            return RedirectToAction("Index", "Home");
        }

        private bool VehiculoExists(int id)
        {
          return (_context.Vehiculos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
