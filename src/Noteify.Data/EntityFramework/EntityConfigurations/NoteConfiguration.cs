using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Noteify.Data.Models;

namespace Noteify.Data.EntityFramework.EntityConfigurations
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.ToTable("Note");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Designation)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("This is the title.");

            builder.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.TimeStamp)
                .IsRequired()
                .HasDefaultValueSql("now()");
            // Attention: gets local time of the server!
            // MSSQL = getdate()

            builder.Property(x => x.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(x => x.User)
                .WithMany(b => b.Notes)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}