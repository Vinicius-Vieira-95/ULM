namespace UlmApi.Domain.Dtos
{
    public class CountRequestsPendingAprovalDto
    {
        public int Quantity { get; set; }
        public int Total { get; set; }
        
        public CountRequestsPendingAprovalDto(int quantity, int total)
        {
            Quantity = quantity;
            Total = total;
        }
    }
}
