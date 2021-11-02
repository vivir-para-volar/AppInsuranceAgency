namespace InsuranceAgency.Struct
{
    public class Connection
    {
        public string PolicyholderPassport { get; }
        public string DrivingLicence { get; }

        public Connection(string policyholderPassport, string drivingLicence)
        {
            PolicyholderPassport = policyholderPassport;
            DrivingLicence = drivingLicence;
        }
    }
}
