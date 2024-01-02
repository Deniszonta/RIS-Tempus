using web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace web.Data{
    public class TempusContext : IdentityDbContext<ApplicationUser>{
        public TempusContext(DbContextOptions<TempusContext> options) : base(options)
        {
        }
        public DbSet<User> Uporabniki { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<UserProject> userProject { get; set; }


        //public DbSet<web.Models.User> Users { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             base.OnModelCreating(modelBuilder);
             // Configuring one-to-one relationship
            // modelBuilder.Entity<ApplicationUser>()
            // .HasOne(au => au.User)
            // .WithOne(u => u.ApplicationUser)
            // .HasForeignKey<ApplicationUser>(au => au.UserID); // If you're using a foreign key
            modelBuilder.Entity<User>()
            .HasMany(t => t.Ticketi)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserID); // SomeIntProperty is the new int principal key in ApplicationUser
        }
    }
    
}