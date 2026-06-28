using AutoMapper;
using OrderProcessing.DiscountService;
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
        private IEnumerable<Customer> _customers;
        private IEnumerable<Order> _orders;
        private IEnumerable<OrderItem> _orderItems;
        private DiscountCalculator _discountCalculator;

        public DataImporter(
            CustomerCsvReader customerCsvReader,
            OrderCsvReader orderCsvReader,
            OrderItemCsvReader orderItemCsvReader,
            IMapper mapper)
        {
            _customerCsvReader = customerCsvReader ?? throw new ArgumentNullException(nameof(customerCsvReader));
            _orderCsvReader = orderCsvReader ?? throw new ArgumentNullException(nameof(orderCsvReader));
            _orderItemCsvReader = orderItemCsvReader ?? throw new ArgumentNullException(nameof(orderItemCsvReader));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void DoImports()
        {
            var folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataFiles");

            var customerDtos = _customerCsvReader.Read(Path.Combine(folderPath, "Customers.csv"));
            var orderDtos = _orderCsvReader.Read(Path.Combine(folderPath, "Orders.csv"));
            var orderItemDtos = _orderItemCsvReader.Read(Path.Combine(folderPath, "OrderItems.csv"));

            _customers = _mapper.Map<IEnumerable<Customer>>(customerDtos);
            _orders = _mapper.Map<IEnumerable<Order>>(orderDtos);
            _orderItems = _mapper.Map<IEnumerable<OrderItem>>(orderItemDtos);

            AssociateEntities();
            ApplyExistingDiscounts();
       }

        private void AssociateEntities()
        {
            foreach (var order in _orders)
            {
                order.AddItems(_orderItems.Where(i => i.OrderId.Equals(order.OrderId)));
            }

            foreach (var customer in _customers)
            {
                customer.Orders.AddRange(_orders.Where(o => o.CustomerId.Equals(customer.CustomerId)));
            }
        }

        private void ApplyExistingDiscounts()
        {
            foreach(var item in _orderItems)
            {
                item.DiscountValue = Math.Round(item.ListPrice * item.DiscountPercentage, 2);
            }           
        }
    }
}