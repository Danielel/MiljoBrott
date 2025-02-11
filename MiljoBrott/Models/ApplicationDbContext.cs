﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiljoBrott.Models
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<Errand> Errands { get; set; }
		public DbSet<Department> Departments { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<ErrandStatus> ErrandStatuses { get; set; }
		public DbSet<Picture> Pictures { get; set; }
		public DbSet<Sample> Samples { get; set; }
		public DbSet<Sequence> Sequences { get; set; }
		
	}
}
