using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using commercetools.Base.Client;
using commercetools.Sdk.Api.Models.Customers;
using Training.Services;

namespace Training
{
     /// <summary>
    /// ASSIGN the customer to the customer group
    /// </summary>
    public class Task02B : IExercise
    {
        private readonly IClient _client;
        private const string _customerKey = "SOME_GENERATED_KEY_FROM_CUSTOMER_DATA";
        private const string _customerGroupKey = "diamond";

        private readonly CustomerService _customerService;

        public Task02B(IEnumerable<IClient> clients)
        {
            _client = clients.FirstOrDefault(c => c.Name.Equals("Client"));
            _customerService = new CustomerService(_client, Settings.ProjectKey);
        }

        public async Task ExecuteAsync()
        {

            // TODO: SET customerGroup for the customer
            ICustomer updatedCustomer = await _customerService.AssignCustomerToCustomerGroup(_customerKey, _customerGroupKey);
            Console.WriteLine($"customer {updatedCustomer.Id} in customer group {updatedCustomer.CustomerGroup.Id}");
        }
    }
}