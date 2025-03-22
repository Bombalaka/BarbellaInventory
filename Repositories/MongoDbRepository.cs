using System.Collections.Generic;
using System.Threading.Tasks;
using BarbellaInventory.Models;
using MongoDB.Driver;

namespace BarbellaInventory.Repositories;

public class MongoDbRepository : IBarbellaRepository
{
    private readonly IMongoCollection<BarbieSet> _barbieSets;

    public MongoDbRepository(IMongoCollection<BarbieSet> barbieSets)
    {
        _barbieSets = barbieSets;
    }
    
    public async Task<IEnumerable<BarbieSet>> GetAllAsync()
    {
        
        return await _barbieSets.Find(_ => true).ToListAsync();
    }

    public async Task<BarbieSet?> GetByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return null;
        }
        return await _barbieSets.Find(s => s.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> AddAsync(BarbieSet barbieSet)
    {
        if (barbieSet == null)
        {
            return false;
        }

        await _barbieSets.InsertOneAsync(barbieSet);
        return true;
    }

    public async Task<bool> UpdateAsync(BarbieSet barbieSet)
    {
        if (barbieSet == null)
        {
            return false;
        }

        var result = await _barbieSets.ReplaceOneAsync(s => s.Id == barbieSet.Id, barbieSet);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _barbieSets.DeleteOneAsync(s => s.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<bool> ExistsAsync(string Name)
    {
        return await _barbieSets.Find(s => s.Name == Name).AnyAsync();
    }
}


