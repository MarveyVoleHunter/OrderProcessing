using OrderProcessing.Domain.Enums;

namespace OrderProcessing.Domain.Entities;

public class Order
{
    private readonly List<DiscountRule> _appliedDiscounts = [];
    private readonly List<OrderItem> _orderItems = [];
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public int OrderStatus { get; set; }
    public DateOnly OrderDate { get; set; }
    public DateOnly RequiredDate { get; set; }
    public DateOnly? ShippedDate { get; set; }
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public IReadOnlyCollection<DiscountRule> AppliedDiscounts => _appliedDiscounts.AsReadOnly();
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

    public void AddDiscountRule(DiscountRule discountRule)
    {
        if (_appliedDiscounts.Contains(discountRule))
            throw new InvalidOperationException($"The {discountRule} discount rule has already been applied to this order.");
        _appliedDiscounts.Add(discountRule);
    }

    public bool HasDiscountBeenApplied(DiscountRule discountRule)
    {
        return _appliedDiscounts.Contains(discountRule);
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

        Total = _orderItems.Sum(i => i.DiscountedPrice);
    }
}
