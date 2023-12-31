﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MVCBasic.Models;
using MVCBasico.Context;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MVCBasic.Controllers
{
    public class ClienteController : Controller
    {
        private const string SessionID = "_UserID";
        private readonly EscuelaDatabaseContext _context;

        public ClienteController(EscuelaDatabaseContext context)
        {
            _context = context;
        }

        // GET: BUSQUEDA DE CLIENTES
        public async Task<IActionResult> Busqueda(String busquedaPersona)
        {
            var legajoDeSesion = HttpContext.Session.GetInt32(SessionID);
            if (legajoDeSesion.HasValue && legajoDeSesion != 0)
            {
                var resultadoBusqueda = await _context.Clientes.Where(t =>
                t.Nombre.Contains(busquedaPersona) ||
                t.Apellido.Contains(busquedaPersona) ||
                t.Email.Contains(busquedaPersona)).ToListAsync();

                return View(resultadoBusqueda);
            }
            return RedirectToAction("Login", "Empleado");
        }

        // GET: Cliente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var legajoDeSesion = HttpContext.Session.GetInt32(SessionID);
            if (legajoDeSesion.HasValue && legajoDeSesion != 0)
            {
                if (id == null || _context.Clientes == null)
                {
                    return NotFound();
                }

                var cliente = await _context.Clientes
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (cliente == null)
                {
                    return NotFound();
                }

                return View(cliente);
            }
            return RedirectToAction("Login", "Empleado");
        }

        // GET: REGISTRAR CLIENTE
        public IActionResult Register()
        {
            return View();
        }

        // POST: Cliente/Registrar
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Email,Id,Nombre,Apellido,password,Telefono")] Cliente cliente)
        {

            if(verificarExistenciaUsuario(cliente.Email))
            {
                ViewData["Mensaje"] = "El usuario ya existe, por favor seleccione un nuevo email para su registro";
                return View(cliente);
            }

            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Dashboard));
            }
            return View(cliente);
        }

        // GET: Cliente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var legajoDeSesion = HttpContext.Session.GetInt32(SessionID);
            if (legajoDeSesion.HasValue && legajoDeSesion != 0)
            {
                if (id == null || _context.Clientes == null)
                {
                    return NotFound();
                }

                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }
                return View(cliente);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Email,Id,Nombre,Apellido,password,Telefono")] Cliente cliente)
        {
            var legajoDeSesion = HttpContext.Session.GetInt32(SessionID);
            if (legajoDeSesion.HasValue && legajoDeSesion != 0)
            {
                if (id != cliente.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(cliente);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ClienteExists(cliente.Id))
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
                return View(cliente);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Cliente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var legajoDeSesion = HttpContext.Session.GetInt32(SessionID);
            if (legajoDeSesion.HasValue && legajoDeSesion != 0)
            {
                if (id == null || _context.Clientes == null)
                {
                    return NotFound();
                }

                var cliente = await _context.Clientes
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (cliente == null)
                {
                    return NotFound();
                }

                return View(cliente);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var legajoDeSesion = HttpContext.Session.GetInt32(SessionID);
            if (legajoDeSesion.HasValue && legajoDeSesion != 0)
            {
                if (_context.Clientes == null)
                {
                    return Problem("Entity set 'EscuelaDatabaseContext.Clientes'  is null.");
                }
                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente != null)
                {
                    _context.Clientes.Remove(cliente);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard", "Empleado");
            }
            return RedirectToAction("Login", "Empleado");
        }
        // GET: LOGIN DE USUARIO
        public IActionResult Login()
        {
            return View();
        }
        // POST: LOGIN DE USUARIO
        [HttpPost]
        public IActionResult Login(Cliente model)
        {
            var usuario = _context.Clientes.FirstOrDefault(u => u.Email == model.Email);

            if (usuario != null && model.password == usuario.password)
            {
                HttpContext.Session.SetInt32(SessionID, usuario.Id);
                return RedirectToAction(nameof(Dashboard));
            }

            ViewData["LoginError"] = "Credenciales inválidas, intente nuevamente"; 

            return View(model);
        }
        // GET: DASHBOARD USUARIO
        public IActionResult Dashboard()
        {
            var legajoDeSesion = HttpContext.Session.GetInt32(SessionID);
            if (legajoDeSesion.HasValue && legajoDeSesion != 0)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }



        private bool verificarExistenciaUsuario(String email)
        {
            var usuario = _context.Clientes.FirstOrDefault(u => u.Email == email);
            return usuario != null;
        }

        private bool ClienteExists(int id)
        {
          return (_context.Clientes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
