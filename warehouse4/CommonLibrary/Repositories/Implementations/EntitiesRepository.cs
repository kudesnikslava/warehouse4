using System;
using System.Collections.Generic;
using System.Text;
using CommonLibrary.Models;
using CommonLibrary.Models.Search;
using CommonLibrary.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.Repositories.Implementations
{
    public class EntitiesRepository : BaseRepository<Entity, BaseSearchOptions>
    {
	    private ILogger<EntitiesRepository> _logger;
	    public EntitiesRepository(ILogger<EntitiesRepository> logger) //
	    {
		    _logger = logger;
		    InitEntities();
	    }

		private void InitEntities()
		{
			_list.Add(new Entity()
			{
				Id = "E1",
				Name = "Book",
				CreatedDate = new DateTime(2010, 12, 03, 14, 16, 54),
				AvailableQuantity = 10
			});

			_list.Add(new Entity()
			{
				Id = "E2",
				Name = "Table",
				CreatedDate = new DateTime(2010, 12, 03, 14, 16, 54),
				AvailableQuantity = 5
			});

		}
	}
}
