using Microsoft.AspNetCore.Mvc;

namespace E_CommerceWebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepo _categoryRepo;
        public CategoryController(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            var data = await _categoryRepo.GetAllCategoriesAsync();
            return View(data);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category category)
        {
            try
            {
                if (await _categoryRepo.GetCategoryWithNameAsync(category.CategoryName) != null)
                    return View(category);

                await _categoryRepo.AddNewCategoryAsync(category);
                // return to index
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Update/5
        public async Task<ActionResult> Update(int id)
        {
            var product = await _categoryRepo.GetCategoryWithIDAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: CategoryController/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int id, Category category)
        {
            try
            {
                await _categoryRepo.UpdateCategoryAsync(category);
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // Get: CategoryController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            await _categoryRepo.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
