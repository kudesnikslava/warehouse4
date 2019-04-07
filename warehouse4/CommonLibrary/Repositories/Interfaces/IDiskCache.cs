using System;
using CommonLibrary.Models;

namespace CommonLibrary.Repositories.Implementations
{
	public interface IDiskCache
	{
		Customer GetCustomerById(String customerId);
		bool SetCustomer(Customer customer);
	}
}