using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using commercetools.Sdk.Api.Models.CustomerGroups;
using commercetools.Sdk.Api.Models.Customers;
using commercetools.Base.Client;
using commercetools.Sdk.Api.Extensions;
using commercetools.Sdk.Api.Models.Common;

namespace Training.Services
{
    public class CustomerService
    {
        private readonly IClient _client;
        private readonly string _projectKey;
        
        public CustomerService(IClient client, string projectKey)
        {
            _client = client;
            _projectKey = projectKey;
        }
        /// <summary>
        /// GET Customer by key
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<ICustomer> GetCustomerByKey(string customerKey)
        {
            return await _client.WithProject(_projectKey)
                .Customers()
                .WithKey(customerKey)
                .Get()
                .ExecuteAsync();
        }

        /// <summary>
        /// POST a Customer sign-up
        /// </summary>
        /// <param name="customerDraft"></param>
        /// <returns></returns>
        public async Task<ICustomer> CreateCustomer(ICustomerDraft customerDraft)
        {
            ICustomerSignInResult result = await _client.WithProject(_projectKey)
                .Customers()
                .Post(customerDraft)
                .ExecuteAsync();

            return result.Customer;
        }

        /// <summary>
        /// Create a Customer Token
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<ICustomerToken> CreateCustomerToken(ICustomer customer)
        {
            ICustomerCreateEmailToken customerCreateEmailToken = new CustomerCreateEmailToken()
            {
                Id = customer.Id,
                Version = customer.Version,
                TtlMinutes = 7200
            };
            ICustomerToken customerToken = await _client.WithProject(_projectKey)
                .Customers()
                .EmailToken()
                .Post(customerCreateEmailToken)
                .ExecuteAsync();

            return customerToken;
        }

        /// <summary>
        /// Confirm a Customer Email
        /// </summary>
        /// <param name="customerToken"></param>
        /// <returns></returns>
        public async Task<ICustomer> ConfirmCustomerEmail(ICustomerToken customerToken)
        {
            ICustomer customer = _client.WithProject(_projectKey)
                .Customers()
                .WithEmailToken(customerToken.Value)
                .Get()
                .ExecuteAsync()
                .Result;
            ICustomerEmailVerify customerEmailVerify = new CustomerEmailVerify()
            {
                Version = customer.Version,
                TokenValue = customerToken.Value
            };
            customer = await _client.WithProject(_projectKey)
                .Customers()
                .EmailConfirm()
                .Post(customerEmailVerify)
                .ExecuteAsync();

            return customer;
        }

        /// <summary>
        /// GET Customer Group by key
        /// </summary>
        /// <param name="customerGroupKey"></param>
        /// <returns></returns>
        public async Task<ICustomerGroup> GetCustomerGroupByKey(string customerGroupKey)
        {
            return await _client.WithApi().WithProjectKey(Settings.ProjectKey)
                .CustomerGroups()
                .WithKey(customerGroupKey)
                .Get()
                .ExecuteAsync();
        }

        /// <summary>
        /// POST Set Customer Group update for the customer
        /// </summary>
        /// <param name="customerKey"></param>
        /// <param name="customerGroupKey"></param>
        /// <returns></returns>
        public async Task<ICustomer> AssignCustomerToCustomerGroup(string customerKey, string customerGroupKey)
        {
            ICustomer customer = await this.GetCustomerByKey(customerKey);
            ICustomerGroup customerGroup = await this.GetCustomerGroupByKey(customerGroupKey);
            ICustomerGroupResourceIdentifier customerGroupResourceIdentifier = new CustomerGroupResourceIdentifier()
            {
                TypeId = IReferenceTypeId.FindEnum("customer-group"),
                Key = customerGroupKey
            };
            ICustomerUpdateAction customerUpdateAction = new CustomerSetCustomerGroupAction()
            {
                Action = "setCustomerGroup",
                CustomerGroup = customerGroupResourceIdentifier
            };
            IList<ICustomerUpdateAction> customerUpdateActions = new List<ICustomerUpdateAction>();
            customerUpdateActions.Add(customerUpdateAction);
            ICustomerUpdate customerUpdate = new CustomerUpdate()
            {
                Version = customer.Version,
                Actions = customerUpdateActions
            };
            ICustomer updatedCustomer = await _client.WithProject(_projectKey)
                .Customers()
                .WithKey(customerKey)
                .Post(customerUpdate)
                .ExecuteAsync();

            return updatedCustomer;
        }
    }
}