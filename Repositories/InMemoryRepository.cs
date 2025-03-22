using System.Collections.Concurrent;
using BarbellaInventory.Models;

namespace BarbellaInventory.Repositories
{
    public class InMemoryRepository : IBarbellaRepository
    {
        // This is our toy box that keeps Barbie sets safe even if many kids play at once!
        private static readonly ConcurrentDictionary<string, BarbieSet> _barbieSets = new(StringComparer.OrdinalIgnoreCase);

        public Task<IEnumerable<BarbieSet>> GetAllAsync()
        {
            return Task.FromResult(_barbieSets.Values.AsEnumerable());
        }

        public Task<bool> AddAsync(BarbieSet barbieSet)
        {
            if (barbieSet == null || string.IsNullOrEmpty(barbieSet.Id))
            {
                return Task.FromResult(false);
            }

            // Add it to our toy box if it's not already there
            return Task.FromResult(_barbieSets.TryAdd(barbieSet.Id, barbieSet));
        }

        public Task<bool> DeleteAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(_barbieSets.TryRemove(id, out _));
        }

        public Task<bool> ExistsAsync(string Name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(_barbieSets.ContainsKey(Name));
        }

        public Task<BarbieSet?> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Task.FromResult<BarbieSet?>(null);
            }

            _barbieSets.TryGetValue(id, out var barbieSet);
            return Task.FromResult(barbieSet);
        }

        public Task<bool> UpdateAsync(BarbieSet barbieSet)
        {
            if (barbieSet == null || string.IsNullOrEmpty(barbieSet.Id))
            {
                return Task.FromResult(false);
            }

            // Only update if it exists!
            if (!_barbieSets.ContainsKey(barbieSet.Id))
            {
                return Task.FromResult(false);
            }

            _barbieSets[barbieSet.Id] = barbieSet; // Replace the old with the new
            return Task.FromResult(true);
        }
    }
}