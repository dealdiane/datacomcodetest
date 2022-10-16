using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Datacom.PayslipGenerator.Web.Models
{
    public record GeneratePayslipModel
    {
        [Required(ErrorMessage = "The annual salary field is required.")]
        [Range(0, 100_000_000, ErrorMessage = "{0} must be between {1:C0} and {2:C0}.")]
        [Display(Name = "Annual salary")]
        public decimal? AnnualSalary { get; set; }

        [StringLength(50, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [AllowNull]
        [Display(Name = "First name")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [Display(Name = "Last name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "The payment period field is required.")]
        [Range(1, 12, ErrorMessage = "{0} must be a number between {1} and {2}.")]
        [Display(Name = "Payment period")]
        public int? PaymentPeriod { get; set; }

        [Required(ErrorMessage = "The superannuation rate field is required.")]
        [Range(0, 100, ErrorMessage = "{0} must be a number between {1} and {2}.")]
        [Display(Name = "Superannuation rate")]
        public decimal? SuperannuationRate { get; set; }
    }
}