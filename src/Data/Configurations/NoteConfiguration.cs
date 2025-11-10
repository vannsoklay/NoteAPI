using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotesAPI.Domain.Entities;

namespace NotesAPI.Data.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<NoteEntity>
{
    public void Configure(EntityTypeBuilder<NoteEntity> builder)
    {
        builder.ToTable("notes");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Title)
            .HasColumnName("title")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Content)
            .HasColumnName("content")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired(false);

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(e => e.DeletedAt)
            .HasColumnName("deleted_at");

        // Relationships
        builder.HasOne<UserEntity>()
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(e => e.Title)
            .HasDatabaseName("idx_note_title");

        builder.HasIndex(e => e.UserId)
            .HasDatabaseName("idx_note_user");

        builder.HasIndex(e => e.DeletedAt)
            .HasDatabaseName("idx_note_deleted");

        builder.HasIndex(e => e.CreatedAt)
            .HasDatabaseName("idx_note_created");
    }
}