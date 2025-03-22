namespace BarbellaInventory.Models
{
    public interface IBarbellaService
    {
        Task<OperationResult> AddBarbieSetAsync(BarbieSet barbieSet);
        Task<OperationResult> DeleteBarbieSetAsync(string id);
        Task<OperationResult> UpdateBarbieSetAsync(BarbieSet barbieSet);
        Task<BarbieSet> GetBarbieSetAsync(string id);
    
        Task<IEnumerable<BarbieSet>> GetBarbieListAsync();
    }
}