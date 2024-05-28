namespace DentalClinic.Data
{
    public class Patient
    {
        public string FullName { get { return $"{Name} {LastName}"; } }
        public int PatientId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual List<Examination> ExaminationsPerformeds { get; set; }
        public virtual Dentist? Dentist { get; set; }
        public Patient()
        {
            ExaminationsPerformeds = new List<Examination>();
        }
    }
}
