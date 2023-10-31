using Ourbnb.Models;
using System;
namespace Ourbnb.DAL
{
	// repository layout for the repositories
	public interface IRepository<T>
	{
		// get all, id, create, update and delete (CRUD) 
		Task<IEnumerable<T>?> GetAll();
		Task<T?> getObjectById(int id);
		Task<bool> Create(T t);
		Task<bool> Update(T t);
		Task<bool> Delete(int id);
	}
}
