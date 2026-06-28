namespace OrderProcessing.Domain.Entities;

public class OrderItem
{
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public decimal ListPrice { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal DiscountedPrice
    {
        get
        {
            return ListPrice - DiscountValue;        
        }
    }
}
