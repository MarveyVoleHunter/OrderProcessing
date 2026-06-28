namespace OrderProcessing.Domain.Entities;

public class OrderItem
{
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public decimal ListPrice { get; set; }
    public decimal Discount { get; set; }
}
