namespace InsuranceAgency.Struct
{
    public class Photo
    {
        public int ID { get; }
        public string EncodedPhoto { get; }
        public int CarID { get; }

        public Photo(int id, string encodedPhoto, int carID)
        {
            ID = id;
            EncodedPhoto = encodedPhoto;
            CarID = carID;
        }
    }
}
