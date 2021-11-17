using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceAgency.Struct
{
    public class Policy
    {
        public int ID { get; }
        public string InsuranceType { get; }
        public int InsurancePremium { get; }
        public int InsuranceAmount { get; }
        public DateTime DateOfConclusion { get; }
        public DateTime ExpirationDate { get; }
        public int PolicyholderID { get; }
        public int CarID { get; }
        public int EmployeeID { get; }

        public Policy(string insuranceType, int insurancePremium, int insuranceAmount, DateTime dateOfConclusion, DateTime expirationDate, int policyholderID, int carID, int employeeID)
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
        public Policy(int id, string insuranceType, int insurancePremium, int insuranceAmount, DateTime dateOfConclusion, DateTime expirationDate, int policyholderID, int carID, int employeeID)
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
