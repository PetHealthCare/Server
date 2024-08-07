﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DTOs.Request.Customer;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayers
{
    public class CustomerDAO : GenericDAO<Customer>
	{
		private static readonly Lazy<CustomerDAO> _instance =
		new Lazy<CustomerDAO>(() => new CustomerDAO(new PetHealthCareContext()));
		public static CustomerDAO Instance => _instance.Value;
		public CustomerDAO(PetHealthCareContext context) : base(context)
		{
            
        }
        public User Login(LoginRequest loginCustomerRequest)
        {

			// Retrieve the user by email
			var user = _context.Users.FirstOrDefault(c => c.Email == loginCustomerRequest.Email);

			// Check if user exists and the password is correct
			if (user != null && BCrypt.Net.BCrypt.Verify(loginCustomerRequest.Password, user.Password))
			{
				return user;
			}

			// Return null or throw an exception if login fails
			return null;
		}

        
        public bool Resgiter(Customer customer)
        {
               _context.Add(customer);
            return _context.SaveChanges() > 0;
           
        }

        public Customer GetCustomerByUserId(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.UserId == id);
        }

        public bool UpdateProfile(UpdateProfileCustomerResquest customerResquest)
        {
            Customer customerUpdate = GetById(customerResquest.CustomerId);
            if (customerUpdate == null)
            {
                return false;
            }
            customerUpdate.FullName = customerResquest.FullName;
            customerUpdate.PhoneNumber = customerResquest.PhoneNumber;
            
            customerUpdate.Address = customerResquest.Address;
            _context.Entry(customerUpdate).State = EntityState.Modified;
            return _context.SaveChanges() > 0;
        }
        public bool UpdateCustomer(Customer customer)
        {
            var customerUpdate = GetById(customer.CustomerId);
            if (customerUpdate == null)
            {
                return false;
            }
            customerUpdate.FullName = customer.FullName;
            customerUpdate.PhoneNumber = customer.PhoneNumber;
            
            customerUpdate.Address = customer.Address;
            customerUpdate.Status = customer.Status;
            _context.Customers.Update(customerUpdate);
            return _context.SaveChanges() > 0;
        }
    }
}
