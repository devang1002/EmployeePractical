using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePractical.Models
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        [Required]
        public DateTime StartingDate { get; set; }
        [Required]
        [MaxLength(20)]
        [Phone(ErrorMessage ="Please enter valid phone number.")]
        public string Mobile { get; set; }
        [Required]
        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [MaxLength(50)]
        public string? MaritalStatus { get; set; }
        [Required]
        [MaxLength(50)]
        public string Gender { get; set; }
        [MaxLength(200)]
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageUrl { get; set; }
    }
}
