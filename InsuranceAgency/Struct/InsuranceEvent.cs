using System;

namespace InsuranceAgency.Struct
{
    public class InsuranceEvent
    {
        public int ID { get; }
        public DateTime Date { get; }
        public int InsurancePayment { get; }
        public int PolicyID { get; }

        public InsuranceEvent(DateTime date, int insurancePayment, int policyID)
        {
            Date = date;
            InsurancePayment = insurancePayment;
            PolicyID = policyID;
        }
        public InsuranceEvent(int id, DateTime date, int insurancePayment, int policyID)
        {
            ID = id;
            Date = date;
            InsurancePayment = insurancePayment;
            PolicyID = policyID;
        }
    }
}
