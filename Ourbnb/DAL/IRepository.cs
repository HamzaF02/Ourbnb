using Ourbnb.Models;
using System;
namespace Ourbnb.DAL
{
	public interface IRepository<T>
	{
		Task<IEnumerable<T>> GetAll();
		Task<T?> getObjectById(int id);
		Task Create(T t);
		Task Update(T t);
		Task<bool> Delete(int id);
	}
}
