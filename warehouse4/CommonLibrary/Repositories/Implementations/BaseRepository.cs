using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.Models;
using CommonLibrary.Models.Search;
using CommonLibrary.Repositories.Interfaces;

namespace CommonLibrary.Repositories.Implementations
{
	public abstract class BaseRepository<TModel, TSearch> : IBaseRepository<TModel, TSearch> 
		where TModel : BaseModelWithId, new()
		where TSearch : BaseSearchOptions
	{
		protected List<TModel> _list;


		public BaseRepository()
		{
			_list = new List<TModel>();
		}
		public async Task<List<TModel>> GetAll()
		{
			return _list;
		}

		public async Task<TModel> GetById(string id)
		{
			return _list.FirstOrDefault(t => t.Id == id);
		}


		public async Task<TModel> Create(TModel item)
		{
			if (String.IsNullOrEmpty(item.Id))
			{
				item.Id = Guid.NewGuid().ToString("N");
			}
			
			_list.Add(item);
			return item;
		}

		public async Task<bool> Remove(string id)
		{
			return  _list.RemoveAll(t => t.Id == id) > 0;

			//bool result = true;
			//try
			//{
			//	_list.RemoveAll(t => t.Id == id);
			//}
			//catch (Exception e)
			//{
			//	result = false;
			//}

			//return result;

			//TODO С точки зрения репозитория true - было что-то удалено, false - небыло что удалять. 
			//с точки зрения usage: true - теперь запись отсутствует (была она там или нет), false - ошибка удаления
		}

		public async Task<TModel> GetFirstOrDefault(TSearch searchOptions)
		{
			if (searchOptions == null)
				return null;

			if (!String.IsNullOrEmpty(searchOptions.Id))
			{
				return _list.FirstOrDefault(t => t.Id == searchOptions.Id);
			}

			return null;

		}

		public async Task<List<TModel>> GetMultiple(TSearch searchOptions = default(TSearch), PaginationOptions paginationOptions = null)
		{
			return _list.Where(t=>GetMultiplePredicate(searchOptions)(t)).ToList();
		}

		public async Task<long> GetMultipleCount(TSearch searchOptions)
		{
			return _list.Count(GetMultiplePredicate(searchOptions));
		}

		public Func<TModel, bool> GetMultiplePredicate(TSearch searchOptions = default(TSearch), PaginationOptions paginationOptions = null)
		{
			Func<TModel, bool> predicate = model => { return true; };
			if (searchOptions != null)
			{
				if (!String.IsNullOrEmpty(searchOptions.Id))
				{
					Func<TModel, bool> prevpredicate = predicate;
					predicate = model => { return model.Id == searchOptions.Id; };
					predicate += prevpredicate;
				}
			}

			return predicate;
		}

		public async Task<TModel> Replace(TModel item)
		{
			int index = _list.FindIndex(t => t.Id == item.Id);
			if (index < 0)
				return await Create(item);

			_list[index] = item;
			return item;
		}


	}
}
