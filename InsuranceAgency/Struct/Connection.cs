namespace InsuranceAgency.Struct
{
    public class Connection
    {
        public string PolicyID { get; }
        public string PersonAllowedToDriveID { get; }

        public Connection(string policyID, string personAllowedToDriveID)
        {
            PolicyID = policyID;
            PersonAllowedToDriveID = personAllowedToDriveID;
        }
    }
}
