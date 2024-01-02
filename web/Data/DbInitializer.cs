using web.Data;
using web.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TempusContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
                new User{Ime="NekiDruzga", Priimek="Druzga", Email="Druzga@gmail.com"},
                new User{Ime="Tomaž", Priimek="Jeglič", Email="tomi@gmail.com"}
            };

            context.Uporabniki.AddRange(users);
            context.SaveChanges();

            var projects = new Project[]
            {
                new Project{Ime="Projekt A", Opis="Podroben opis projekta z imenom A"},
                new Project{Ime="Projekt B", Opis="Podroben opis projekta z imenom B"},
                new Project{Ime="Projekt C", Opis="Podroben opis projekta z imenom C"},
                new Project{Ime="Projekt D", Opis="Podroben opis projekta z imenom D"},
                new Project{Ime="Projekt E", Opis="Podroben opis projekta z imenom E"}
            };

            context.Projects.AddRange(projects);
            context.SaveChanges();

            var tickets = new Ticket[]
            {
                new Ticket{Naslov="Neki", Opis="Naredi nekaj", Stanje=StanjeTicketa.Reported, UserID=1, ProjectID=1, cas=0},
                new Ticket{Naslov="Neki2", Opis="Naredi nekaj2", Stanje=StanjeTicketa.Reported, UserID=2, ProjectID=2, cas=0}
            };

            context.Tickets.AddRange(tickets);

                        var roles = new IdentityRole[] {
                new IdentityRole{Id="1", Name="Administrator", NormalizedName="ADMINISTRATOR"},
                new IdentityRole{Id="2", Name="Manager", NormalizedName="MANAGER"},
                new IdentityRole{Id="3", Name="Staff", NormalizedName="STAFF"}
            };

            foreach (IdentityRole r in roles)
            {
                context.Roles.Add(r);
            }

            var appuser = new ApplicationUser
            {
                Ime = "Admin",
                Priimek = "Admin",
                Email = "admin@gmail.com",
                NormalizedEmail = "XXXX@GMAIL.COM",
                UserName = "admin@gmail.com",
                NormalizedUserName = "admin@gmail.com",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };


            if (!context.Users.Any(u => u.Email == appuser.Email))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(appuser,"Admin1!");
                appuser.PasswordHash = hashed;
                context.Users.Add(appuser);
            }

            context.SaveChanges();
            

            var UserRoles = new IdentityUserRole<string>[]
            {
                new IdentityUserRole<string>{RoleId = roles[0].Id, UserId=appuser.Id},
                new IdentityUserRole<string>{RoleId = roles[1].Id, UserId=appuser.Id}
            };

            foreach (IdentityUserRole<string> r in UserRoles)
            {
                context.UserRoles.Add(r);
            }

            context.SaveChanges();
        }
    }
}