namespace OrderProcessing.Domain.DTOs
{
    public class OrderItemDto
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Discount { get; set; }
    }
}
