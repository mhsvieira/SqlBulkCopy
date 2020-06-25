namespace ApiZero.Modelo
{
    public class Quota
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Amount { get; set; }
        public string StartDate { get; set; }
        public string FinalDate { get; set; }
        public string OwnerName { get; set; }
        public string Username { get; set; }
        public string Percentage { get; set; }
        public string Mailed { get; set; }
        public string PathFileName { get; set; }
    }
}