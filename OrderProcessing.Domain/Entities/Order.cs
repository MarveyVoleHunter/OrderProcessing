using OrderProcessing.Domain.Enums;

namespace OrderProcessing.Domain.Entities;

public class Order
{
    private readonly List<OrderItem> _orderItems = new();
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public int OrderStatus { get; set; }
    public DateOnly OrderDate { get; set; }
    public DateOnly RequiredDate { get; set; }
    public DateOnly? ShippedDate { get; set; }
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public DiscountRule DiscountRule { get; set; } = DiscountRule.None;
    public decimal Total { get; set; }

    public void AddItem(OrderItem item)
    {
        _orderItems.Add(item);
        RecalculateTotal();
    }

    public void AddItems(IEnumerable<OrderItem> items)
    {
        _orderItems.AddRange(items);
        RecalculateTotal();
    }

    public void RemoveItem(OrderItem item)
    {
        _orderItems.Remove(item);
        RecalculateTotal();
    }

    private void RecalculateTotal()
    {
        if (_orderItems.Count == 0)
        {
            Total = 0;
            return;
        }

        Total = _orderItems.Sum(i => i.ListPrice - i.Discount);
    }
}
