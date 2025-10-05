using System.ComponentModel.DataAnnotations;
namespace SSD_Lab1.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Years in Business")]
        public int YearsInBusiness { get; set; }

        [Required]
        [Url]
        [Display(Name = "Website")]
        public string Website { get; set; }

        [Display(Name = "Province")]
        public string? Province { get; set; } 
    }
}
