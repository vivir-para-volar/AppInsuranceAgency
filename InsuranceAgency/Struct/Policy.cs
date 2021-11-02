using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceAgency.Struct
{
    public class Policy
    {
        public string ID { get; }
        public string InsuranceType { get; }
        public int InsuranceAmount { get; }
        public int InsurancePremium { get; }
        public DateTime DateOfConclusion { get; }
        public DateTime ExpirationDate { get; }
        public string PolicyholderPassport { get; }
        public string VIN { get; }
        public string EmployeePassport { get; }

        public Policy(string insuranceType, int insuranceAmount, int insurancePremium, DateTime dateOfConclusion, DateTime expirationDate, string policyholderPassport, string vin, string employeePassport)
        {
            InsuranceType = insuranceType;
            InsuranceAmount = insuranceAmount;
            InsurancePremium = insurancePremium;
            DateOfConclusion = dateOfConclusion;
            ExpirationDate = expirationDate;
            PolicyholderPassport = policyholderPassport;
            VIN = vin;
            EmployeePassport = employeePassport;
        }
        public Policy(string id, string insuranceType, int insuranceAmount, int insurancePremium, DateTime dateOfConclusion, DateTime expirationDate, string policyholderPassport, string vin, string employeePassport)
        {
            ID = id;
            InsuranceType = insuranceType;
            InsuranceAmount = insuranceAmount;
            InsurancePremium = insurancePremium;
            DateOfConclusion = dateOfConclusion;
            ExpirationDate = expirationDate;
            PolicyholderPassport = policyholderPassport;
            VIN = vin;
            EmployeePassport = employeePassport;
        }
    }
}
