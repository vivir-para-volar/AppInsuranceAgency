using System;

namespace InsuranceAgency.Struct
{
    public class InsuranceEvent
    {
        public string ID { get; }
        public DateTime Date { get; }
        public int InsurancePayment { get; }
        public string PolicyID { get; }

        public InsuranceEvent(DateTime date, int insurancePayment, string policyID)
        {
            Date = date;
            InsurancePayment = insurancePayment;
            PolicyID = policyID;
        }
        public InsuranceEvent(string id, DateTime date, int insurancePayment, string policyID)
        {
            ID = id;
            Date = date;
            InsurancePayment = insurancePayment;
            PolicyID = policyID;
        }
    }
}
