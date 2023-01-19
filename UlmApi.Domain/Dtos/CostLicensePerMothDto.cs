namespace UlmApi.Domain.Dtos
{
    public class CostLicensePerMothDto
    {
        public string Month { get; set; }
        public double Total { get; set; }

        public CostLicensePerMothDto(string month, double total)
        {
            Month = month;
            Total = total;
        }
    }
}