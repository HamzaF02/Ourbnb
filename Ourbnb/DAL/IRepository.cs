using Ourbnb.Models;
using System;
namespace Ourbnb.DAL
{
	public interface IRepository<T>
	{
		Task<IEnumerable<T>> GetAll();
		Task<T?> getObjectById(int id);
		Task<bool> Create(T t);
		Task<bool> Update(T t);
		Task<bool> Delete(int id);
	}
}
