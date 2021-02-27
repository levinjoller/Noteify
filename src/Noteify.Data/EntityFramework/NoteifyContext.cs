using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Noteify.Data.EntityFramework.EntityConfigurations;
using Noteify.Data.EntityFramework.Providers;
using Noteify.Data.Models;
using Noteify.Data.Models.Auth;

namespace Noteify.Data.EntityFramework
{
    public class NoteifyContext : IdentityDbContext<User, IdentityRole<string>, string>
    {
        private readonly UserIdProvider _userIdProvider;

        // To be able to accept the configuration from the startup.
        public NoteifyContext(DbContextOptions<NoteifyContext> options, UserIdProvider userIdProvider) : base(options)
        {
            _userIdProvider = userIdProvider;
        }

        public DbSet<Note> Notes { get; private set; }

        /// <summary>
        ///     Method to configure the models to tables conversion.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Identity
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new UserConfiguration());

            builder.ApplyConfiguration(new NoteConfiguration());

            // Global query filter
            builder.Entity<Note>().HasQueryFilter(x => x.UserId == _userIdProvider.GetUserId());
        }
    }
}