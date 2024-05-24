﻿using DataAccessLayers;
using Repositories;
using Services;

namespace Presentation
{
	/// <summary>
	/// Functions for create dependency injections
	/// </summary>
	public static class DependencyInjection
	{
		/// <summary>
		/// This function to add dependency injection for NuGet Package
		/// </summary>
		/// <param name="services"></param>
		public static void AddPackage(this IServiceCollection services)
		{
			//Add other service in nuget package
			services.AddSwaggerGen();
		}

		/// <summary>
		/// Create dependencies for service (interface) & service (class) or repository (interface) & repository (class)
		/// </summary>
		/// <param name="services"></param>
		public static void AddMasterServices(this IServiceCollection services)
		{
			// Add dependency injection for class and interface
			//EX:
			//services.AddScoped<IAccountRepository, AccountRepository>();
			//services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<ICustomerRepository, CustomerRepository>();
			services.AddScoped<ICustomerService, CustomerService>();
			services.AddScoped<IServiceRepository, ServiceRepository>();
			services.AddScoped<IServiceService, ServiceService>();
            //services.AddSingleton<CustomerDAO>();
           
        }
	}

}
