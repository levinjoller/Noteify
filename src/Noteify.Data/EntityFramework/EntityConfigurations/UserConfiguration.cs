using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Noteify.Data.Models.Auth;

namespace Noteify.Data.EntityFramework.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
        }
    }
}