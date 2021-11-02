namespace InsuranceAgency.Struct
{
    public class PersonAllowedToDrive
    {
        public string ID { get; }
        public string FullName { get; }
        public string DrivingLicence { get; }

        public PersonAllowedToDrive(string fullName, string drivingLicence)
        {
            FullName = fullName;
            DrivingLicence = drivingLicence;
        }
        public PersonAllowedToDrive(string id, string fullName, string drivingLicence)
        {
            ID = id;
            FullName = fullName;
            DrivingLicence = drivingLicence;
        }
    }
}
