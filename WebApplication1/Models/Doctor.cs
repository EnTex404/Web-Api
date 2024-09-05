namespace WebApplication1.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string? FullName { get; set; }

        public int CabinetId { get; set; }
        public int SpecializationId { get; set; }
        public int AreaId { get; set; }

        public virtual Cabinet Cabinet { get; set; }
        public virtual Specialization Specialization { get; set; }
        public virtual Area Area { get; set; }
    }
}
