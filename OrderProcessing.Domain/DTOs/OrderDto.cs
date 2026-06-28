namespace OrderProcessing.Domain.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int OrderStatus { get; set; }
        public DateOnly OrderDate { get; set; }
        public DateOnly RequiredDate { get; set; }
        public DateOnly? ShippedDate { get; set; }
    }
}
