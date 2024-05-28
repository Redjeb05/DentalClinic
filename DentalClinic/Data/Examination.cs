using System.ComponentModel.DataAnnotations.Schema;

namespace DentalClinic.Data
{
    public class Examination
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient? Patient { get; set; }

        public DateTime DateAndHour { get; set; }
        public int DentistId { get; set; }
        [ForeignKey("DentistId")]
        public virtual Dentist? Dentist { get; set; }   
        public string Description { get; set; } 

    }
}
