namespace Domain.Helpers
{
    public class ProceedingData
    {
        public string ProceedingCity { get; set; }
        public string ProceedingStreet { get; set; }
        public string ProceedingHouseNumber { get; set; }
        public string ProceedingPhoneNumber { get; set; }
        public string ProceedingCustomerId { get; set; }

        public ProceedingData(string proceedingCity, string proceedingStreet, string proceedingHouseNumber, string proceedingPhoneNumber, string customerId)
        {
            this.ProceedingCity = proceedingCity;
            this.ProceedingStreet = proceedingStreet;
            this.ProceedingHouseNumber = proceedingHouseNumber;
            this.ProceedingPhoneNumber = proceedingPhoneNumber;
            this.ProceedingCustomerId = customerId;
        }

        public ProceedingData()
        {
            this.ProceedingCity = string.Empty;
            this.ProceedingStreet = string.Empty;
            this.ProceedingHouseNumber = string.Empty;
            this.ProceedingPhoneNumber = string.Empty;
            this.ProceedingCustomerId = string.Empty;
        }

    }
}
