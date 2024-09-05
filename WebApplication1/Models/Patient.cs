namespace WebApplication1.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }

        public int AreaId { get; set; }

        public virtual Area Area { get; set; }
    }
}
