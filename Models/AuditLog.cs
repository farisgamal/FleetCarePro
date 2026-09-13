namespace MVCDemo.Models
{
    // Module 6: written to by AuditLogAttribute
    public class AuditLog
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string ActionDetails { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
