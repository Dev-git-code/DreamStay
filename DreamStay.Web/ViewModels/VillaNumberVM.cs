using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using DreamStay.Domain.Entities;

namespace DreamStay.Web.ViewModels
{
    public class VillaNumberVM
    {
        public VillaNumber? VillaNumber { get; set; } = default;
        [ValidateNever]
        public IEnumerable<SelectListItem>? VillaList { get; set; }
    }
}
