using CarRentalSystem.DTOs;
using CarRentalSystem.Helper;
using CarRentalSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalSystem.Services
{
    public class CustomerService
    {
       private readonly CustomerRepository _repo = new CustomerRepository();

        public List<CustomerDTO> GetAll() => _repo.GetAll();

        public List<CustomerDTO> SearchCustomers(string col, string key)
        {
            if (string.IsNullOrEmpty(key))
                return GetAll();

            return _repo.Search(col, key);
        }

        public void AddCustomer(CustomerDTO customer)
        {
            Validate(customer);

            int affected = _repo.Add(customer);
            if (affected <= 0)
                throw new Exception("Failed to add customer.");
        }

        public void UpdateCustomer(CustomerDTO customer)
        {
            if (customer.CusId <= 0)
                throw new Exception("Invalid Customer ID.");

            Validate(customer);

            int affected = _repo.Update(customer);
            if (affected <= 0)
                throw new Exception("Failed to update customer.");
        }

        public void DeleteCustomer(int customerId)
        {
            if (customerId <= 0)
                throw new Exception("Invalid Customer ID.");

            int affected = _repo.Delete(customerId);
            if (affected <= 0)
                throw new Exception("Failed to delete customer.");
        }

        private void Validate(CustomerDTO customer)
        {
            if (string.IsNullOrWhiteSpace(customer.CusName))
                throw new Exception("Customer name is required.");

            if (string.IsNullOrWhiteSpace(customer.CusAdd))
                throw new Exception("Customer address is required.");

            if (string.IsNullOrWhiteSpace(customer.Phone))
                throw new Exception("Phone is required.");

            if (!long.TryParse(customer.Phone, out _))
                throw new Exception("Phone must be numeric.");
        }
    }
}
