using Akalaat.BLL.Interfaces;
using Akalaat.DAL.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Akalaat.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IGenericRepository<Review> reviewRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        public ReviewController(IGenericRepository<Review> reviewRepo, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostingEnvironment)
        {
            this.reviewRepo = reviewRepo;
            this.userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var reviews = await reviewRepo.GetAllAsync();
            return View(reviews);
        }
        public async Task<IActionResult> Details(int id)
        {
            var review = await reviewRepo.GetByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // GET: Review/AddReview
        public IActionResult AddReview()
        {
            return View();
        }
        // POST: Review/AddReview
        [HttpPost]
        public async Task<IActionResult> AddReview(AddReviewVM addReviewVM)
        {
            if (addReviewVM.ReviewImage != null && addReviewVM.ReviewImage.Length > 0)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(addReviewVM.ReviewImage.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await addReviewVM.ReviewImage.CopyToAsync(fileStream);
                }


                Review reviewEntity = new Review
                {
                    Rating = --addReviewVM.Rating,
                    Comment = addReviewVM.Comment,
                    No_of_Likes = addReviewVM.No_of_Likes,
                    ReviewImage = uniqueFileName,
                    Customer_ID = addReviewVM.Customer_ID,
                    Resturant_ID = addReviewVM.Resturant_ID
                };

                await reviewRepo.Add(reviewEntity);

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("ReviewImage", "Please select a file.");
            }

            return View(addReviewVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var review = await reviewRepo.GetByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Review/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await reviewRepo.GetByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            await reviewRepo.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var review = await reviewRepo.GetByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            var reviewVM = new AddReviewVM
            {
                Rating = review.Rating,
                Comment = review.Comment,
                No_of_Likes = review.No_of_Likes,
                Customer_ID = review.Customer_ID,
                Resturant_ID = review.Resturant_ID
            };

            return View(reviewVM);
        }

        // POST: Review/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddReviewVM addReviewVM)
        {


            //if (ModelState.IsValid)
            //{
            var review = await reviewRepo.GetByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            if (addReviewVM.ReviewImage != null && addReviewVM.ReviewImage.Length > 0)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(addReviewVM.ReviewImage.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await addReviewVM.ReviewImage.CopyToAsync(fileStream);
                }

                // Update the review entity with the new image path
                review.ReviewImage = uniqueFileName;
            }

            // Update other properties of the review entity
            review.Rating = addReviewVM.Rating;
            review.Comment = addReviewVM.Comment;
            review.No_of_Likes = addReviewVM.No_of_Likes;

            await reviewRepo.Update(review);
            return RedirectToAction("Index");
        }

        //return View(addReviewVM);
        //}
    }


}
