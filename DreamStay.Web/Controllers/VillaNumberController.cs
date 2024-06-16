using DreamStay.Infrastructure.Data;
using DreamStay.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DreamStay.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using DreamStay.Application.Common.Interfaces;


namespace DreamStay.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public VillaNumberController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        public IActionResult Index()
        {
            var villasNumbers = _unitofWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villasNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM villaNumberVM)
        {
            if(villaNumberVM.VillaNumber != null)
            {
                bool roomNumberExists = _unitofWork.VillaNumber.Any(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

                if (ModelState.IsValid && !roomNumberExists)
                {
                    _unitofWork.VillaNumber.Add(villaNumberVM.VillaNumber);
                    _unitofWork.Save();
                    TempData["success"] = "The Villa Number has been created successfully";
                    return RedirectToAction(nameof(Index));
                }

                if (roomNumberExists)
                {
                    TempData["error"] = "The Villa Number already exists.";
                }

            }
            villaNumberVM.VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(villaNumberVM);
        }

        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitofWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };
            
            if(villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if (villaNumberVM.VillaNumber != null)
            {
                if (ModelState.IsValid)
                {
                    _unitofWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                    _unitofWork.Save();
                    TempData["success"] = "The Villa Number has been updated successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            villaNumberVM.VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            TempData["error"] = "The Villa Number cannot be Updated";
            return View(villaNumberVM);
        }

        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitofWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitofWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };

            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }
        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = _unitofWork.VillaNumber.Get(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            if(objFromDb is not null)
            {
                _unitofWork.VillaNumber.Remove(objFromDb);
                _unitofWork.Save();
                TempData["success"] = "The Villa Number has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa Number could not be deleted";
            return View();
        }
    }
}
