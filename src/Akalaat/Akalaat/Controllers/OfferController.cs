using Akalaat.BLL.Interfaces;
using Akalaat.DAL.Models;
using Akalaat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Akalaat.Controllers
{
    public class OfferController : Controller
    {
        private readonly IGenericRepository<Offer> OfferRepo;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        // GET: DishController

        public OfferController(IGenericRepository<Offer> OfferRepo, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            this.OfferRepo = OfferRepo;
            this.userManager = userManager;
            hostingEnvironment = environment;
        }
        // GET: OfferController
        public async Task<IActionResult> Index()
        {
            var offers = await OfferRepo.GetAllAsync();
            return View(offers);
        }

        // GET: OfferController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var offer = await OfferRepo.GetByIdAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            return View(offer);
        }

        // GET: OfferController/Create
        public async Task<IActionResult> AddOffer()
        {
            return View();
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOffer(OfferVM addOfferVM)
        {
            if (addOfferVM.OfferImage != null && addOfferVM.OfferImage.Length > 0)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(addOfferVM.OfferImage.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await addOfferVM.OfferImage.CopyToAsync(fileStream);
                }

                Offer OfferEntity = new Offer
                {
                    Name = addOfferVM.Name,
                    Description = addOfferVM.Description,
                    Offer_image = uniqueFileName,
                    Menu_ID = addOfferVM.Menu_ID,
                };

                await OfferRepo.Add(OfferEntity);

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("OfferImage", "Please select a file.");
            }

            return View(addOfferVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var offer = await OfferRepo.GetByIdAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            return View(offer);
        }

        // POST: OfferController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var offer = await OfferRepo.GetByIdAsync(id);
            if (offer == null)
            {
                return NotFound();
            }

            await OfferRepo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var offer = await OfferRepo.GetByIdAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            var offerVM = new OfferVM
            {
                Name = offer.Name,
                Description = offer.Description,
                Menu_ID = offer.Menu_ID
            };
            return View(offerVM);
        }

        // POST: OfferController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OfferVM offerVM)
        {
            if (id != offerVM.Menu_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var offer = await OfferRepo.GetByIdAsync(id);
                if (offer == null)
                {
                    return NotFound();
                }

                if (offerVM.OfferImage != null && offerVM.OfferImage.Length > 0)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(offerVM.OfferImage.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await offerVM.OfferImage.CopyToAsync(fileStream);
                    }

                    offer.Offer_image = uniqueFileName;
                }

                offer.Name = offerVM.Name;
                offer.Description = offerVM.Description;
                offer.Menu_ID = offerVM.Menu_ID;

                await OfferRepo.Update(offer);
                return RedirectToAction("Index");
            }
            return View(offerVM);
        }

    
}
}
