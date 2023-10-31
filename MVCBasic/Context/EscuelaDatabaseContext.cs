﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCBasic.Models;
using System.Collections.Generic;
using MVCBasic.Models;

namespace MVCBasico.Context
{
    public class EscuelaDatabaseContext : DbContext
    {
        public EscuelaDatabaseContext(DbContextOptions<EscuelaDatabaseContext>options) : base(options)
        {
        }
        public DbSet<Estudiante> Estudiantes { get; set; }
    }
}

