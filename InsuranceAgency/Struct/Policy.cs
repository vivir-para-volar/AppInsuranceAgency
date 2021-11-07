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
        public int InsurancePremium { get; }
        public int InsuranceAmount { get; }
        public DateTime DateOfConclusion { get; }
        public DateTime ExpirationDate { get; }
        public string PolicyholderID { get; }
        public string CarID { get; }
        public string EmployeeID { get; }

        public Policy(string insuranceType, int insurancePremium, int insuranceAmount, DateTime dateOfConclusion, DateTime expirationDate, string policyholderID, string carID, string employeeID)
        {
            InsuranceType = insuranceType;
            InsurancePremium = insurancePremium;
            InsuranceAmount = insuranceAmount;
            DateOfConclusion = dateOfConclusion;
            ExpirationDate = expirationDate;
            PolicyholderID = policyholderID;
            CarID = carID;
            EmployeeID = employeeID;
        }
        public Policy(string id, string insuranceType, int insurancePremium, int insuranceAmount, DateTime dateOfConclusion, DateTime expirationDate, string policyholderID, string carID, string employeeID)
        {
            ID = id;
            InsuranceType = insuranceType;
            InsurancePremium = insurancePremium;
            InsuranceAmount = insuranceAmount;
            DateOfConclusion = dateOfConclusion;
            ExpirationDate = expirationDate;
            PolicyholderID = policyholderID;
            CarID = carID;
            EmployeeID = employeeID;
        }
    }
}
