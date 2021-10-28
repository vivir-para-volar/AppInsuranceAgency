using System;

namespace InsuranceAgency.Struct
{
    public struct Policyholder
    {
        public string FullName { get; }
        public DateTime Birthday { get; }
        public string Telephone { get; }
        public string Passport { get; }
        

        public Policyholder(string fullName, DateTime birthday, string telephone, string passport)
        {
            FullName = fullName;
            Birthday = birthday;
            Telephone = telephone;
            Passport = passport;
        }
    }
}
