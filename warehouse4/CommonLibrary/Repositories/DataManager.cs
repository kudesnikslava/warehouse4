using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.Models;
using CommonLibrary.Repositories.Implementations;
using Microsoft.Extensions.Caching.Distributed;

namespace CommonLibrary.Repositories
{
    public class DataManager
    {
	    private CustomersRepository _customersRepository;
	    private EntitiesRepository _entitiesRepository;

		private IDiskCache _cache;

		public DataManager(CustomersRepository customersRepository, EntitiesRepository entitiesRepository, IDiskCache cache)
		{
			_customersRepository = customersRepository;
			_entitiesRepository = entitiesRepository;
			_cache = cache;
		}

	    #region Customers


	    public List<Customer> GetAllCustomers()
	    {
		    return _customersRepository.GetAll().Result;
	    }

	    public async Task<Customer> GetCustomerById(string customerId)
	    {
		    Customer customer = _cache.GetCustomerById(customerId);
		    if (customer != null)
			    return customer;

		    customer = await _customersRepository.GetById(customerId);
		    if (customer == null)
			    return null;

		    _cache.SetCustomer(customer);

		    return customer;
	    }

	    public async Task CreateCustomer(Customer newCustomer)
	    {
		    await _customersRepository.Create(newCustomer);
	    }

	    public void UpdateOrCreateCustomer(Customer customer)
	    {
		    _customersRepository.Replace(customer);
	    }

	    public bool RemoveCustomer(string customerId)
	    {
		    return _customersRepository.Remove(customerId).Result;
	    }

	    public bool CustomerExists(string customerId)
	    {
		    return GetCustomerById(customerId) != null;
	    }

		#endregion Customers

		#region Entities


		public List<Entity> GetAllEntities()
	    {
		    return _entitiesRepository.GetAll().Result;
	    }

	    public Entity GetEntityById(string entityId)
	    {
		    return _entitiesRepository.GetById(entityId).Result;
	    }

	    public async Task CreateEntity(Entity newEntity)
	    {
		    await _entitiesRepository.Create(newEntity);
	    }

	    public void UpdateOrCreateEntity(Entity entity)
	    {
		    _entitiesRepository.Replace(entity);
	    }

	    public bool RemoveEntity(string entityId)
	    {
		    return _entitiesRepository.Remove(entityId).Result;
	    }

	    public bool EntityrExists(string entityId)
	    {
		    return GetEntityById(entityId) != null;
	    }

	    #endregion Entities
	}
}
