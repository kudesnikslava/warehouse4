using System;
using System.Collections.Generic;
using System.Text;
using CommonLibrary.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CommonLibrary.Repositories.Implementations
{


	public class RedisDiskCache : IDiskCache
	{
	   private const string customerKey = "Customer";

	   private IDatabase _redisDb;
	    public RedisDiskCache()
	    {
			var redis = ConnectionMultiplexer.Connect("localhost:6379");
		    _redisDb = redis.GetDatabase();
		}

		public Customer GetCustomerById(String customerId)
		{
			Customer customer = null;
			var jsonCustomer = _redisDb.StringGet(GetKeyForCustomer(customerId));
			if (jsonCustomer.HasValue)
			{
				customer = JsonConvert.DeserializeObject<Customer>(jsonCustomer);
			}

			return customer;
		}

		public bool SetCustomer(Customer customer)
		{
			String json = JsonConvert.SerializeObject(customer);
			return _redisDb.StringSet(GetKeyForCustomer(customer.Id), json);
		}

		private String GetKeyForCustomer(String customerId)
	   {
		   return $"{customerKey}:{customerId}";
	   }
    }
}
