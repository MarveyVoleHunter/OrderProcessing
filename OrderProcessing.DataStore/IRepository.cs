using OrderProcessing.Domain.Entities;

public interface IRepository
{
    void AddCustomers(IEnumerable<Customer> customers);
    void AddOrders(IEnumerable<Order> orders);
    void AddOrderItems(IEnumerable<OrderItem> orderItems);
    IEnumerable<Customer> GetAllCustomers();
    IEnumerable<Order> GetAllOrders();
    IEnumerable<OrderItem> GetAllOrderItems();
    Customer? GetCustomerById(int id);
}