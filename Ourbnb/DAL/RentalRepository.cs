using Microsoft.EntityFrameworkCore;
using Ourbnb.Models;
using System;

namespace Ourbnb.DAL
{
	public class RentalRepository : IRepository<Rental>
	{
		private readonly RentalDbContext _db;
		public RentalRepository(RentalDbContext db)
		{
			_db = db;
		}

        public async Task Create(Rental rental)
        {
            _db.Rentals.Add(rental);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var rental = await _db.Rentals.FindAsync(id);
            if (rental == null)
            {
                return false;
            }

            _db.Rentals.Remove(rental);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Rental?> getObjectById(int id)
        {
            return await _db.Rentals.FindAsync(id);
        }

        public async Task<IEnumerable<Rental>> GetAll()
        {
            return await _db.Rentals.ToListAsync();
        }

        public async Task Update(Rental rental)
        {
            _db.Rentals.Add(rental);
            await _db.SaveChangesAsync();
        }
    }
}

