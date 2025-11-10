namespace NotesAPI.Domain.Entities
{

    public class NoteEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        // Foreign key
        public Guid? UserId { get; set; }
    }
}