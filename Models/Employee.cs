using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IT15_LabExam.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required, Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        [Required, Display(Name = "Daily Rate")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Daily Rate must be greater than 0.")]
        public decimal DailyRate { get; set; }

        public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
