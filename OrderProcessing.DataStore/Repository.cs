using OrderProcessing.Domain.Entities;

namespace OrderProcessing.DataStore
{
    public class Repository : IRepository
    {
        private readonly List<Customer> _customers = [];
        private readonly List<Order> _orders = [];
        private readonly List<OrderItem> _orderItems = [];

        public void AddCustomers(IEnumerable<Customer> customers)
        {
            _customers.AddRange(customers);
        }

        public void AddOrders(IEnumerable<Order> orders)
        {
            _orders.AddRange(orders);
        }

        public void AddOrderItems(IEnumerable<OrderItem> orderItems)
        {
            _orderItems.AddRange(orderItems);
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _customers.AsReadOnly();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orders.AsReadOnly();
        }

        public IEnumerable<OrderItem> GetAllOrderItems()
        {
            return _orderItems.AsReadOnly();
        }

        public Customer? GetCustomerById(int id)
        {
            return _customers.FirstOrDefault(c => c.CustomerId == id);
        }
    }
}
