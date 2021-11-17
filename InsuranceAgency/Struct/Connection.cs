namespace InsuranceAgency.Struct
{
    public class Connection
    {
        public int PolicyID { get; }
        public int PersonAllowedToDriveID { get; }

        public Connection(int policyID, int personAllowedToDriveID)
        {
            PolicyID = policyID;
            PersonAllowedToDriveID = personAllowedToDriveID;
        }
    }
}
