
using Akalaat.BLL.Interfaces;
using Akalaat.DAL.Models;
using Akalaat.Utilities;
using Akalaat.ViewModels;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



public class CategoryController : Controller
{
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly FileManagement _fileManagement;

        public CategoryController(IGenericRepository<Category> categoryRepository,FileManagement fileManagement)
        {
            _categoryRepository = categoryRepository;
            _fileManagement = fileManagement;
        }

        // GET: Category
        [Route("Category/index/{menuId}")]
        public async Task<IActionResult> Index(int menuId)
        {
            // Retrieve categories for the specified menuId
            var categoryVM = new CategoryVM()
            {
                Categories = await _categoryRepository.GetAllAsync([(category => category.Menu_ID == menuId)]),
                Menu_ID = menuId
            };
            // Pass categories to the view
            return View(categoryVM);
        }
        
        // GET: Category/Create
        public IActionResult Create()
        {
            return RedirectToAction(nameof(Index));
        }

        // POST: Category/Create
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryVM categoryVm)
        {
            if (ModelState.IsValid)
            {
                //copy image file to wwwroot
                var ImgPath=await _fileManagement.AddFileAsync($"Images/Menus/{categoryVm.Menu_ID}/Categories/", 
                    $"{categoryVm.Name}.jpg", categoryVm.CategoryImageFile);

                if (ImgPath != null)
                {
                    await _categoryRepository.Add(new Category()
                    {
                        Name = categoryVm.Name,
                        Category_image = ImgPath,
                        Description = categoryVm.Description,
                        Menu_ID=categoryVm.Menu_ID
                    });


                    return RedirectToAction("Index", "Category", 
                        new { id = categoryVm.Menu_ID });                
                }
            }
            return RedirectToAction("Index", "Category",
                new { id = categoryVm.Menu_ID });        }

        public async Task<IActionResult> Edit(int id)
        {
            var category=await _categoryRepository.GetByIdAsync(id);
            
            return View(new CategoryVM()
            {
                Menu_ID = category?.Menu_ID??0,
                Name = category?.Name,
                Description = category.Description,
                Category_image = category.Category_image
             
            });

        }


        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryVM categoryVm)
        {
            if (id != categoryVm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var category = (await _categoryRepository.GetByIdAsync(id));

                    if (categoryVm.CategoryImageFile != null)
                    {
                        _fileManagement.DeleteImageFile(category?.Category_image??"");

                        var ImgPath=await _fileManagement.AddFileAsync($"Images/Menus/{categoryVm.Menu_ID}/Categories/", 
                            $"{categoryVm.Name}.jpg", categoryVm.CategoryImageFile);

                        if (ImgPath != null)
                        {
                            await _categoryRepository.Update(new Category()
                            {
                                Id = categoryVm.Id,
                                Name = categoryVm.Name,
                                Category_image = ImgPath,
                                Description = categoryVm.Description,
                                Menu_ID = categoryVm.Menu_ID
                            });


                                        
                        }
                    }
                    else
                    {
                        await _categoryRepository.Update(new Category()
                        {
                            Id = id,
                            Name = categoryVm.Name,
                            Description = categoryVm.Description,
                            Menu_ID= categoryVm.Menu_ID
                        });
                    }
                    return RedirectToAction("Index", "Category", 
                        new { id = categoryVm.Menu_ID });   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CategoryExists(categoryVm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
         
               
                
            }
            return RedirectToAction("Index", "Category",
                new { id = categoryVm.Menu_ID });
            
        }

    
        // POST: Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = (await _categoryRepository.GetByIdAsync(id));
            _fileManagement.DeleteImageFile(category?.Category_image??"");
            await _categoryRepository.Delete(id);
            return RedirectToAction("Index", "Category",
                new { id = category.Menu_ID ?? 0 });
            
        }

        private async Task<bool> CategoryExists(int id)
        {
            return (await _categoryRepository.GetByIdAsync(id))!=null;
        }
}

