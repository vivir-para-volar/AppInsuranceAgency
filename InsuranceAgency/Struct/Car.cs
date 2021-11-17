namespace InsuranceAgency.Struct
{
    public class Car
    {
        public int ID { get; }
        public string Model { get; }
        public string VIN { get; }
        public string RegistrationPlate { get; }
        public string VehiclePassport { get; }

        public Car(string model, string vin, string registrationPlate, string vehiclePassport)
        {
            Model = model;
            VIN = vin;
            RegistrationPlate = registrationPlate;
            VehiclePassport = vehiclePassport;
        }
        public Car(int id, string model, string vin, string registrationPlate, string vehiclePassport)
        {
            ID = id;
            Model = model;
            VIN = vin;
            RegistrationPlate = registrationPlate;
            VehiclePassport = vehiclePassport;
        }
    }
}
