using DreamStay.Infrastructure.Data;
using DreamStay.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using DreamStay.Application.Common.Interfaces;

namespace DreamStay.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaController(IUnitofWork unitofWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofWork = unitofWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var villas = _unitofWork.Villa.GetAll();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("Name", "The Description cannot match the Name of Villa");
            }
            if (ModelState.IsValid)
            {
                if (obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);

                    obj.ImageUrl = @"\images\VillaImage\" + fileName;
                }
                else
                {
                    obj.ImageUrl = "https://placehold.co/600X400";
                }
                _unitofWork.Villa.Add(obj);
                _unitofWork.Save();
                TempData["success"] = "The Villa has been created successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa could not be created";
            return View();
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _unitofWork.Villa.Get(u=>u.Id == villaId);
            if(obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid && obj.Id > 0)
            {
                if (obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                                                        obj.ImageUrl.TrimStart('\\'));

                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStream);

                    obj.ImageUrl = @"\images\VillaImage\" + fileName;
                }

                _unitofWork.Villa.Update(obj);
                _unitofWork.Save();
                TempData["success"] = "The Villa has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa could not be updated";
            return View(obj);
        }

        public IActionResult Delete(int villaId)
        {
            Villa? objFromDb = _unitofWork.Villa.Get(u => u.Id == villaId);
            if (objFromDb is null)
            {      
                return RedirectToAction("Error", "Home");
            }
            return View(objFromDb);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _unitofWork.Villa.Get(u => u.Id == obj.Id);
            if(objFromDb is not null)
            {
                if (!string.IsNullOrEmpty(objFromDb?.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                                                    objFromDb.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _unitofWork.Villa.Remove(objFromDb);
                _unitofWork.Save();
                TempData["success"] = "The Villa has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa could not be deleted";
            return View();
        }
    }
}
