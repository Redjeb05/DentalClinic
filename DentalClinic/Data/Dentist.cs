namespace DentalClinic.Data
{
    public class Dentist
    {
        public string FullName { get { return $"{Name} {LastName}"; } }
        public int DentistId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public virtual List<Patient> RegisteredPatients { get; set; }

        public Dentist()
        {
            RegisteredPatients = new List<Patient>();
        }
    }
}
