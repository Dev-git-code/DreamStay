using DreamStay.Infrastructure.Data;
using DreamStay.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DreamStay.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using DreamStay.Application.Common.Interfaces;


namespace DreamStay.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public AmenityController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        public IActionResult Index()
        {
            var villasNumbers = _unitofWork.Amenity.GetAll(includeProperties: "Villa");
            return View(villasNumbers);
        }

        public IActionResult Create()
        {
            AmenityVM AmenityVM = new()
            {
                VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(AmenityVM);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM AmenityVM)
        {
            if(AmenityVM.Amenity != null)
            {
                bool roomNumberExists = _unitofWork.Amenity.Any(u => u.Villa_Number == AmenityVM.Amenity.Villa_Number);

                if (ModelState.IsValid && !roomNumberExists)
                {
                    _unitofWork.Amenity.Add(AmenityVM.Amenity);
                    _unitofWork.Save();
                    TempData["success"] = "The Villa Number has been created successfully";
                    return RedirectToAction(nameof(Index));
                }

                if (roomNumberExists)
                {
                    TempData["error"] = "The Villa Number already exists.";
                }

            }
            AmenityVM.VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(AmenityVM);
        }

        public IActionResult Update(int AmenityId)
        {
            AmenityVM AmenityVM = new()
            {
                VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitofWork.Amenity.Get(u => u.Villa_Number == AmenityId)
            };
            
            if(AmenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(AmenityVM);
        }

        [HttpPost]
        public IActionResult Update(AmenityVM AmenityVM)
        {
            if (AmenityVM.Amenity != null)
            {
                if (ModelState.IsValid)
                {
                    _unitofWork.Amenity.Update(AmenityVM.Amenity);
                    _unitofWork.Save();
                    TempData["success"] = "The Villa Number has been updated successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            AmenityVM.VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            TempData["error"] = "The Villa Number cannot be Updated";
            return View(AmenityVM);
        }

        public IActionResult Delete(int AmenityId)
        {
            AmenityVM AmenityVM = new()
            {
                VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitofWork.Amenity.Get(u => u.Villa_Number == AmenityId)
            };

            if (AmenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(AmenityVM);
        }
        [HttpPost]
        public IActionResult Delete(AmenityVM AmenityVM)
        {
            Amenity? objFromDb = _unitofWork.Amenity.Get(u => u.Villa_Number == AmenityVM.Amenity.Villa_Number);

            if(objFromDb is not null)
            {
                _unitofWork.Amenity.Remove(objFromDb);
                _unitofWork.Save();
                TempData["success"] = "The Villa Number has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa Number could not be deleted";
            return View();
        }
    }
}
