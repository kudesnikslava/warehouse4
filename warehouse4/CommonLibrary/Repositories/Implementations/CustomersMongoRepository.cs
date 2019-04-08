using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.Models;
using CommonLibrary.Models.Search;
using CommonLibrary.Repositories.Interfaces;
using MongoDB.Driver;

namespace CommonLibrary.Repositories.Implementations
{
	public class CustomersMongoRepository: IBaseRepository<Customer, BaseSearchOptions>, ICustomersRepository
	{
		private readonly IMongoDatabase _db;
		protected  IMongoCollection<Customer> Collection { get; }

		public CustomersMongoRepository( IMongoClient client)
		{
			_db = client.GetDatabase("customer");
			//Collection.Indexes.CreateOne(new CreateIndexModel<Customer>(Builders<Customer>.IndexKeys.Ascending(f => f.FirstName)));
		}

		public async Task<List<Customer>> GetAll()
		{
			var request = Collection.Find(null);
			return await request.ToListAsync();
		}

		public Task<Customer> GetFirstOrDefault(BaseSearchOptions searchOptions)
		{
			throw new NotImplementedException();
		}

		public Task<List<Customer>> GetMultiple(BaseSearchOptions searchOptions = null, PaginationOptions paginationOptions = null)
		{
			throw new NotImplementedException();
		}

		public Task<long> GetMultipleCount(BaseSearchOptions searchOptions)
		{
			throw new NotImplementedException();
		}

		public Task<Customer> GetById(string id)
		{
			throw new NotImplementedException();
		}

		public Task<Customer> Create(Customer item)
		{
			throw new NotImplementedException();
		}

		public Task<Customer> Replace(Customer item)
		{
			throw new NotImplementedException();
		}

		public Task<bool> Remove(string id)
		{
			throw new NotImplementedException();
		}

		public Func<Customer, bool> GetMultiplePredicate(BaseSearchOptions searchOptions = default(BaseSearchOptions),
			PaginationOptions paginationOptions = null)
		{
			throw new NotImplementedException();
		}
	}
}
