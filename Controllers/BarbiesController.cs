


using BarbellaInventory.Models;
using Microsoft.AspNetCore.Mvc;

namespace Barbie_Inventory.Controllers
{

    public class BarbiesController : Controller
    {
        private readonly IBarbellaService _barbellaService;
        public BarbiesController(IBarbellaService barbellaService)
        {
            _barbellaService = barbellaService;
        }
        

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        /// GET: List of Barbies collection
        [HttpGet]
        public async Task<IActionResult> BarbiesList()
        {
            ViewBag.Message = TempData["SuccessMessage"];
            var barbieSets = (await _barbellaService.GetBarbieListAsync()).ToList();
            return View(barbieSets);

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveNewBarbieSet(BarbieSet barbieSet)
        {
            // Add this line to assign a unique Id
            barbieSet.Id = Guid.NewGuid().ToString();

            // Validate the model
            if (!ModelState.IsValid)
            {
                // If form is invalid, show Create page with the filled data and errors
                return View("Create", barbieSet);
            }

            var result = await _barbellaService.AddBarbieSetAsync(barbieSet);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                return View("Create", barbieSet);
            }
            System.Console.WriteLine($"Barbie Set Added Successfully : {barbieSet.Name}.");

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(BarbiesList));

        }

        /// GET: Edit the Barbies collection
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var barbieSet = await _barbellaService.GetBarbieSetAsync(id);
            if (barbieSet == null)
            {
                return NotFound();
            }
            return View(barbieSet);
        }
        /// POST: Save changed the edited Barbies collection
        [HttpPost]
        public async Task<IActionResult> Edit(BarbieSet barbieSet)
        {
            var result = await _barbellaService.UpdateBarbieSetAsync(barbieSet);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Message);
                return View(barbieSet); // ✅ Return the Edit view
            }

            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(BarbiesList));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _barbellaService.DeleteBarbieSetAsync(id);
            TempData["SuccessMessage"] = result.Message;
            return RedirectToAction(nameof(BarbiesList));
        }

    }

}