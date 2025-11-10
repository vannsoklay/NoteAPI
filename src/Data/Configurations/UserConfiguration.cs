using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotesAPI.Domain.Entities;

namespace NotesAPI.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Email)
            .HasColumnName("email")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Phone)
            .HasColumnName("phone")
            .HasMaxLength(100);

        builder.Property(e => e.Password)
            .HasColumnName("password")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(e => e.DeletedAt)
            .HasColumnName("deleted_at");

        // Indexes
        builder.HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("idx_user_name");

        builder.HasIndex(e => e.Phone)
            .IsUnique()
            .HasDatabaseName("idx_user_phone");

        builder.HasIndex(e => e.Email)
            .IsUnique()
            .HasDatabaseName("idx_user_email");
    }
}