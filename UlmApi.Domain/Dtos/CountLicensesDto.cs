namespace UlmApi.Domain.Dtos
{
    public class CountLicensesDto
    {
        public int Quantity { get; set; }
        public int Total { get; set; }

        public CountLicensesDto(int quantity, int total)
        {
            Quantity = quantity;
            Total = total;
        }
    }
}