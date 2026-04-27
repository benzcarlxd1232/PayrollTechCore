using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IT15_LabExam.Models
{
    public class Payroll
    {
        [Key]
        public int PayrollId { get; set; }

        [Required, Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required, Display(Name = "Days Worked")]
        [Range(0.5, 31, ErrorMessage = "Days Worked must be between 0.5 and 31.")]
        [Column(TypeName = "decimal(5,1)")]
        public decimal DaysWorked { get; set; }

        [Display(Name = "Gross Pay")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal GrossPay { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Deduction cannot be negative.")]
        public decimal Deduction { get; set; }

        [Display(Name = "Net Pay")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal NetPay { get; set; }
    }
}
