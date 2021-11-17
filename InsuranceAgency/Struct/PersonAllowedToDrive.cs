namespace InsuranceAgency.Struct
{
    public class PersonAllowedToDrive
    {
        public int ID { get; }
        public string FullName { get; }
        public string DrivingLicence { get; }

        public PersonAllowedToDrive(string fullName, string drivingLicence)
        {
            FullName = fullName;
            DrivingLicence = drivingLicence;
        }
        public PersonAllowedToDrive(int id, string fullName, string drivingLicence)
        {
            ID = id;
            FullName = fullName;
            DrivingLicence = drivingLicence;
        }
    }
}
