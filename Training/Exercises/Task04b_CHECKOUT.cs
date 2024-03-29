using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Api.Models.Channels;
using commercetools.Sdk.Api.Models.Orders;
using commercetools.Sdk.Api.Models.States;
using commercetools.Sdk.Api.Models.Subscriptions;
using commercetools.Base.Client;
using commercetools.Sdk.Api.Extensions;

using Training.Services;
using commercetools.Sdk.Api.Models.Customers;
using commercetools.Sdk.Api.Models.Carts;

namespace Training
{
    /// <summary>
    /// Create a cart for a customer, add a product to it, create an order from the cart and change the order state.
    /// </summary>
    public class Task04B : IExercise
    {
        private readonly IClient _client;
        private const string _channelId = "044e2a01-0528-4574-afb4-5bab68e4bb44";
        private const string _customerKey = "SOME_GENERATED_KEY_FROM_CUSTOMER_DATA";
        private const string _discountCode = "TEST";
        private const string _stateOrderedPackedKey = "OrderPacked";
        private const string _productSku = "A0E200000001YWY";

        private readonly CustomerService _customerService;
        private readonly CartService _cartService;
        private readonly PaymentService _paymentService;
        private readonly OrderService _orderService;
        private readonly ChannelService _channelService;


        public Task04B(IEnumerable<IClient> clients)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            _customerService = new CustomerService(_client, Settings.ProjectKey);
            _cartService = new CartService(_client, Settings.ProjectKey);
            _paymentService = new PaymentService(_client, Settings.ProjectKey);
            _orderService = new OrderService(_client, Settings.ProjectKey);
            _channelService = new ChannelService(_client, Settings.ProjectKey);

        }

        public async Task ExecuteAsync()
        {
            // TODO: GET customer
            ICustomer customer = await _customerService.GetCustomerByKey(_customerKey);
            // TODO: CREATE a cart for the customer
            ICartDraft cartDraft = new CartDraft()
            {
                Currency = "EUR",
                CustomerId = customer.Id
            };
            ICart cart = await _cartService.CreateCart(cartDraft);
            Console.WriteLine($"Cart {cart.Id} for customer: {cart.CustomerId}");
            
            // TODO: GET a channel if your inventory mode will not be NONE
            IChannel channel = await _channelService.GetChannelById(_channelId);

            // TODO: ADD items to the cart
            
            
            // TODO: ADD discount coupon code to the cart
            
            // TODO: RECALCULATE the cart

            // TODO: ADD default shipping to the cart
            
            // TODO: CREATE a payment 
            
            // Console.WriteLine($"Payment Created with Id: {payment.Id}");

            // TODO: ADD transaction to the payment
            
            // TODO: ADD payment to the cart
            
            // TODO: CREATE order
            // Console.WriteLine($"Order Created with order number: {order.OrderNumber}");
            
            // TODO: UPDATE order state to Confirmed
            // Console.WriteLine($"Order state changed to: {order.OrderState.Value}");
            
            // TODO: GET custom workflow state for Order
            // TODO: UPDATE order custom workflow state
            
            // Console.WriteLine($"Order Workflow State changed to: {order.State?.Obj?.Name["en"]}");
        }

    }
}