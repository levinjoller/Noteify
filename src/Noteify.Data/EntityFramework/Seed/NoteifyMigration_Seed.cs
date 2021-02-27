using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Noteify.Data.EntityFramework.Options;
using Noteify.Data.Models;
using Noteify.Data.Models.Auth;

namespace Noteify.Data.EntityFramework.Seed
{
    public static class NoteifyMigration_Seed
    {
        private static NoteifyContext _context;
        private static UserManager<User> _userManager;
        private static SeedUserOptions _options;

        public static void EnsureDbCreatedAndSeededDev(this NoteifyContext context, UserManager<User> userManager, IOptionsSnapshot<SeedUserOptions> options)
        {
            _context = context;
            _userManager = userManager;
            _options = options.Get(SeedUserOptions.NormalUser);

            _context.Database.Migrate();

            SeedUser();
            SeedNotes();

            DetachAllEntities();
        }

        public static void EnsureDbCreatedProd(this NoteifyContext context)
        {
            _context = context;
            _context.Database.Migrate();
        }

        // Release entities after seed from dbcontext to allow further interactions.
        private static void DetachAllEntities()
        {
            var unchangedEntities = _context.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Unchanged)
                .ToList();

            foreach (var entity in _context.ChangeTracker.Entries())
            {
                entity.State = EntityState.Detached;
            }
        }

        private static void SeedUser()
        {
            if (_context.Users.Any())
            {
                Console.WriteLine("Already have Users - not seeding");
                return;
            }

            Console.WriteLine("Adding data (Users) - seeding...");

            var user = new User()
            {
                Id = new Guid("384a3bf1-bc99-4c94-93bf-9a37d4de348f").ToString(),
                UserName = _options.UserName,
                Email = _options.Email
            };

            _userManager.CreateAsync(user, _options.Password).Wait();
        }

        private static void SeedNotes()
        {
            if (_context.Notes.IgnoreQueryFilters().Any())
            {
                Console.WriteLine("Already have Notes - not seeding");
                return;
            }

            Console.WriteLine("Adding data (Notes) - seeding...");

            var notes = new Note[]
            {
                new Note(){
                    Id = new Guid("afcde351-5dae-4653-8b3d-c119632f3bd2"),
                    Designation = "Super Note",
                    Message = "This is my content",
                    TimeStamp = DateTime.Now.AddDays(-12),
                    UserId = "384a3bf1-bc99-4c94-93bf-9a37d4de348f"
                },
                new Note(){
                    Designation = "hiking with Bob",
                    Message = "* Don't forget water bottle.\\* Umbrella",
                    UserId = "384a3bf1-bc99-4c94-93bf-9a37d4de348f"
                }
            };
            _context.AddRange(notes);
            _context.SaveChanges();
        }
    }
}
