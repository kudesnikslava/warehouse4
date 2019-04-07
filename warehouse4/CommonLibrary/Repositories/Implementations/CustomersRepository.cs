using System;
using System.Collections.Generic;
using System.Text;
using CommonLibrary.Models;
using CommonLibrary.Models.Search;
using CommonLibrary.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Repositories.Implementations
{
    public class CustomersRepository : BaseRepository<Customer, BaseSearchOptions>
    {
	    private ILogger<CustomersRepository> _logger;
	    public CustomersRepository(ILogger<CustomersRepository> logger) //
	    {
		    _logger = logger;
		    Init();
	    }

	    private void Init()
	    {
		    _list.Add(new Customer()
		    {
			    Id = "C1",
			    FirstName = "Petrov",
			    LastName = "Petr",
			    Age = 20,
			    CreationDate = new DateTime(2010, 12, 03, 14, 16, 54)
		    });

		    _list.Add(new Customer()
		    {
			    Id = "C2",
			    FirstName = "Sidorov",
			    LastName = "Sidr",
			    Age = 25,
			    CreationDate = new DateTime(2011, 11, 04, 11, 26, 34)
		    });
		}
    }
}
