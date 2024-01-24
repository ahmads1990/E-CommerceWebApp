using E_CommerceWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

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
        public ActionResult Index()
        {
            var data = _categoryRepo.GetAllCategories();
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
        public ActionResult Create(Category category)
        {
            try
            {
                if (_categoryRepo.GetCategoryWithNameAsync(category.CategoryName) != null)
                    return View(category);

                _categoryRepo.AddNewCategoryAsync(category);
                // return to index
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Update/5
        public ActionResult Update(int id)
        {
            var product = _categoryRepo.GetCategoryWithIDAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: CategoryController/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, Category category)
        {
            try
            {
                _categoryRepo.UpdateCategoryAsync(category);
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // Get: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            _categoryRepo.DeleteCategoryAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
