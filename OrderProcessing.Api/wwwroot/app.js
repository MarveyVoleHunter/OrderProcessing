const { createApp, ref, computed, onMounted } = Vue;

createApp({
  setup() {
    const customers = ref([]);
    const orders = ref([]);
    const orderItems = ref([]);
    const selectedCustomerId = ref(null);
    const selectedOrderId = ref(null);

    const customerOrders = computed(() => {
      if (!selectedCustomerId.value) return [];
      return orders.value.filter(o => o.customerId === selectedCustomerId.value);
    });

    const fetchCustomers = async () => {
      const response = await fetch('/api/customers');
      if (!response.ok) return;
      customers.value = await response.json();
    };

    const fetchOrders = async () => {
      const response = await fetch('/api/orders');
      if (!response.ok) return;
      orders.value = await response.json();
    };

    const fetchOrderItems = async (orderId) => {
      orderItems.value = [];
      if (!orderId) return;
      const response = await fetch(`/api/orderitems/order/${orderId}`);
      if (!response.ok) return;
      orderItems.value = await response.json();
    };

    const selectCustomer = (customerId) => {
      selectedCustomerId.value = customerId;
      selectedOrderId.value = null;
      orderItems.value = [];
    };

    const selectOrder = (orderId) => {
      selectedOrderId.value = orderId;
      fetchOrderItems(orderId);
    };

    const formatCurrency = (value) => {
      if (value == null || Number.isNaN(Number(value))) return '$0.00';
      return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
      }).format(value);
    };

    onMounted(async () => {
      await Promise.all([fetchCustomers(), fetchOrders()]);
    });

    return {
      customers,
      orders,
      orderItems,
      selectedCustomerId,
      selectedOrderId,
      customerOrders,
      selectCustomer,
      selectOrder,
      formatCurrency
    };
  }
}).mount('#app');
