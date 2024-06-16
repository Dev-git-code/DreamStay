using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamStay.Domain.Entities
{
    public class Villa
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Display(Name = "Price per Night")]
        [Range(0,10000)]
        public int Price {  get; set; }
        [Display(Name = "Area (sqft)")]
        public int Sqft {  get; set; }
        [Range(1,10)]
        public int Occupancy {  get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        [Display(Name = "Image Url")]
        public string? ImageUrl {  get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
