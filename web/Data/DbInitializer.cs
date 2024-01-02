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

            var roles = new IdentityRole[] {
                new IdentityRole{Id="1", Name="Administrator", NormalizedName="ADMINISTRATOR"},
                new IdentityRole{Id="2", Name="Manager", NormalizedName="MANAGER"},
                new IdentityRole{Id="3", Name="Staff", NormalizedName="STAFF"}
            };

            foreach (IdentityRole r in roles)
            {
                context.Roles.Add(r);
            }

            var users = new User[]
            {
                new User{Ime="Denis", Priimek="Žonta", Email="deniszonta@gmail.com"},
                new User{Ime="Jaka", Priimek="Marinsek", Email="jakamarinsek@gmail.com"},
                new User{Ime="Janez", Priimek="Novak", Email="janeznovak@gmail.com"},
                new User{Ime="Branko", Priimek="Kastelic", Email="branko@gmail.com"},
                new User{Ime="Nika", Priimek="Pika", Email="nikapika@gmail.com"}
            };

            context.Uporabniki.AddRange(users);
            context.SaveChanges();

            foreach (User u in users){
                var appuser2 = new ApplicationUser
            {
                Ime = u.Ime,
                Priimek = u.Priimek,
                Email = u.Email,
                NormalizedEmail = "XXXX@GMAIL.COM",
                UserName = u.Email,
                NormalizedUserName = u.Email,
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            if (!context.Users.Any(us => us.Email == appuser2.Email)){
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(appuser2,"SpremeniMe123!");
                appuser2.PasswordHash = hashed;
                context.Users.Add(appuser2);
            }

                u.appUser = appuser2;
                 var UserRoles2 = new IdentityUserRole<string>[]{
                     new IdentityUserRole<string>{RoleId = "3", UserId=appuser2.Id},
                 };

             foreach (IdentityUserRole<string> r in UserRoles2){
                 context.UserRoles.Add(r);
             }
             context.SaveChanges();
            }


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
                new Ticket{Naslov="Kreiriaj novo funkcionalnost", Opis="Kreiriaj novo funkcionalnost v aplikaciji", Stanje=StanjeTicketa.Reported, UserID=1, ProjectID=1, cas=0},
                new Ticket{Naslov="Dodaj nov button", Opis="Dodaj nov button v start page", Stanje=StanjeTicketa.Working, UserID=2, ProjectID=2, cas=0},
                new Ticket{Naslov="Dodaj nov text color", Opis="Dodaj nov text color v config.tailwind", Stanje=StanjeTicketa.Working, UserID=3, ProjectID=3, cas=0.5f},
                new Ticket{Naslov="Spremeni podatkovni tip", Opis="Spremeni podatkovni tip v tabeli Popravilo", Stanje=StanjeTicketa.Resolved, UserID=4, ProjectID=4, cas=1.5f},
                new Ticket{Naslov="Zamenjaj monitor", Opis="Zamenjaj monitor v sejni sobi", Stanje=StanjeTicketa.OnHold, UserID=5, ProjectID=5, cas=0.5f},
                new Ticket{Naslov="Pripravi predstavitev", Opis="Pripravi predstavitev o letnem načrtu", Stanje=StanjeTicketa.Reported, UserID=2, ProjectID=3, cas=0},
                new Ticket{Naslov="Sprintaj dokument", Opis="Sprintaj dokument in ga odnesi v HR", Stanje=StanjeTicketa.Reported, UserID=1, ProjectID=4, cas=0},
                new Ticket{Naslov="Dodaj userja v mailbox", Opis="Dodaj userja v mailbox, da bo prejemal pomembno pošto", Stanje=StanjeTicketa.Working, UserID=2, ProjectID=2, cas=0},
                new Ticket{Naslov="Uvajanje", Opis="Uvajanje novo zaposlenega, ki pride v petek.", Stanje=StanjeTicketa.Working, UserID=4, ProjectID=1, cas=1},
                new Ticket{Naslov="Posodobi poslovni proces", Opis="Posodobi poslovni proces in spremeni shemo.", Stanje=StanjeTicketa.Reported, UserID=5, ProjectID=5, cas=0}
            };

            context.Tickets.AddRange(tickets);

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