using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Sdk.Api.Models.Common;
using commercetools.Sdk.Api.Models.Customers;
using commercetools.Base.Client;
using Training.Services;

namespace Training
{
    /// <summary>
    /// CREATE a customer
    /// CREATE a email verfification token
    /// Verfify customer
    /// </summary>
    public class Task02A : IExercise
    {
        private readonly IClient _client;
        private readonly CustomerService _customerService;
        private const string _customerKey = "SOME_GENERATED_KEY_FROM_CUSTOMER_DATA";

        public Task02A(IEnumerable<IClient> clients)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            _customerService = new CustomerService(_client, Settings.ProjectKey);
        }

        public async Task ExecuteAsync()
        {
            // CREATE customer draft
            var customerDraft = new CustomerDraft
            {
                Email = "vladislavs+test@scandiweb.com",
                Password = "Option123#",
                Key = _customerKey,
                FirstName = "Vlad",
                LastName = "314",
                Addresses = new List<IBaseAddress>{
                        new AddressDraft {
                            Country = "LV",
                            Key = _customerKey + "-home"
                    }
                },
                DefaultShippingAddress = 0,
                DefaultBillingAddress = 0
            };

            // TODO: SIGNUP a customer
            ICustomer customer = await _customerService.CreateCustomer(customerDraft);
            Console.WriteLine($"Customer Created with Id : {customer.Id} and Key : {customer.Key} and Email Verified: {customer.IsEmailVerified}");

            // TODO: CREATE a email verfification token
            ICustomerToken customerToken = await _customerService.CreateCustomerToken(customer);

            // TODO: CONFIRM CustomerEmail
            ICustomer retrievedCustomer = await _customerService.ConfirmCustomerEmail(customerToken);
            Console.WriteLine($"Is Email Verified:{retrievedCustomer.IsEmailVerified}");
        }
    }
}