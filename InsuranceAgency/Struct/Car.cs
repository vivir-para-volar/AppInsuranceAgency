namespace InsuranceAgency.Struct
{
    public class Car
    {
        public string ID { get; }
        public string Model { get; }
        public string VIN { get; }
        public string RegistrationPlate { get; }
        public string VehiclePassport { get; }
        public string Image { get; }


        public Car(string model, string vin, string registrationPlate, string vehiclePassport, string image)
        {
            Model = model;
            VIN = vin;
            RegistrationPlate = registrationPlate;
            VehiclePassport = vehiclePassport;
            Image = image;
        }
        public Car(string id, string model, string vin, string registrationPlate, string vehiclePassport, string image)
        {
            ID = id;
            Model = model;
            VIN = vin;
            RegistrationPlate = registrationPlate;
            VehiclePassport = vehiclePassport;
            Image = image;
        }
    }
}
