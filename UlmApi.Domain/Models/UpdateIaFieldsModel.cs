namespace UlmApi.Domain.Models
{
    public class UpdateIaFieldsModel
    {
        public int RequestLicenseId { get; set; }
        public int Percentage { get; set; }
        public string Prediction { get; set; }
        public string Message { get; set; }
    }
}
