namespace UlmApi.Domain.Dtos
{
    public class CountLicensesExpiringDto
    {
        public int Quantity { get; set; }
        public int Total { get; set; }
        
        public CountLicensesExpiringDto(int quantity, int total)
        {
            Quantity = quantity;
            Total = total;
        }
    }
}
