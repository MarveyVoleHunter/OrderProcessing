using AutoMapper;
using OrderProcessing.Domain.Entities;
using OrderProcessing.ImportService.CsvReaders;

namespace OrderProcessing.ImportService
{
    public class DataImporter
    {
        private readonly CustomerCsvReader _customerCsvReader;
        private readonly OrderCsvReader _orderCsvReader;
        private readonly OrderItemCsvReader _orderItemCsvReader;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;

        public DataImporter(
            CustomerCsvReader customerCsvReader,
            OrderCsvReader orderCsvReader,
            OrderItemCsvReader orderItemCsvReader,
            IMapper mapper,
            IRepository repository)
        {
            _customerCsvReader = customerCsvReader ?? throw new ArgumentNullException(nameof(customerCsvReader));
            _orderCsvReader = orderCsvReader ?? throw new ArgumentNullException(nameof(orderCsvReader));
            _orderItemCsvReader = orderItemCsvReader ?? throw new ArgumentNullException(nameof(orderItemCsvReader));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public void DoImports()
        {
            var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataFiles");
            var customerDtos = _customerCsvReader.Read(Path.Combine(folderPath, "Customers.csv"));
            var orderDtos = _orderCsvReader.Read(Path.Combine(folderPath, "Orders.csv"));
            var orderItemDtos = _orderItemCsvReader.Read(Path.Combine(folderPath, "OrderItems.csv"));
            var customers = _mapper.Map<IEnumerable<Customer>>(customerDtos);
            var orders = _mapper.Map<IEnumerable<Order>>(orderDtos);
            var orderItems = _mapper.Map<IEnumerable<OrderItem>>(orderItemDtos);

            AssociateEntities(customers, orders, orderItems);
            ApplyExistingDiscounts(orderItems);
            
            _repository.AddCustomers(customers);
            _repository.AddOrders(orders);
            _repository.AddOrderItems(orderItems);
       }

        private static void AssociateEntities(IEnumerable<Customer> customers, IEnumerable<Order> orders,
            IEnumerable<OrderItem> orderItems)
        {
            foreach (var order in orders)
            {
                order.AddItems(orderItems.Where(i => i.OrderId.Equals(order.OrderId)));
            }

            foreach (var customer in customers)
            {
                customer.Orders.AddRange(orders.Where(o => o.CustomerId.Equals(customer.CustomerId)));
            }
        }

        private static void ApplyExistingDiscounts(IEnumerable<OrderItem> orderItems)
        {
            foreach(var item in orderItems)
            {
                item.DiscountValue = Math.Round(item.ListPrice * item.DiscountPercentage, 2);
            }           
        }
    }
}