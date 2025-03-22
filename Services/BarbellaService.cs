
using BarbellaInventory.Models;
using BarbellaInventory.Repositories;

namespace BarbellaInventory.Services
{
    public class BarbellaService : IBarbellaService
    {
        private readonly IBarbellaRepository _barbellaRepository;

        public BarbellaService(IBarbellaRepository barbellaRepository)
        {
            _barbellaRepository = barbellaRepository;
        }
       

        public async Task<OperationResult> AddBarbieSetAsync(BarbieSet barbieSet)
        {

            
                // Check valid name
                if (barbieSet == null || string.IsNullOrWhiteSpace(barbieSet.Name))
                {
                    return OperationResult.Failure("Invalid barbie set name.");
                }

                // Check if colleciton name is exits
                if (await _barbellaRepository.ExistsAsync(barbieSet.Name))
                {
                    return OperationResult.Failure("You are already this name.");
                }

                // Add the subscriber to the repository
                var success = await _barbellaRepository.AddAsync(barbieSet);
                //if it not show error , the otherwise show success message
                if (!success)
                {
                    return OperationResult.Failure("Failed to add colelction,  Please try again later.");
                }
                return OperationResult.Success($"Now you added the barbie colelction :  {barbieSet.Name}! .");

            
        }
        public async Task<OperationResult> DeleteBarbieSetAsync(string id)
        {
            // Simulate a long running operation
            var success = await _barbellaRepository.DeleteAsync(id);
            return success
            ? OperationResult.Success("Barbie set deleted successfully.")
            : OperationResult.Failure("Barbie set not found.");
        }
        public async Task<OperationResult> UpdateBarbieSetAsync(BarbieSet barbieSet)
        {
            var success = await _barbellaRepository.UpdateAsync(barbieSet);
            return success
            ? OperationResult.Success("Barbie set updated successfully.")
            : OperationResult.Failure("Barbie set not found.");
        }
        public async Task<BarbieSet> GetBarbieSetAsync(string id) => await _barbellaRepository.GetByIdAsync(id);

        public async Task<IEnumerable<BarbieSet>> GetBarbieListAsync() => await _barbellaRepository.GetAllAsync();
    }
}
