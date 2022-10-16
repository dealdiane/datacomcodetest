using Datacom.PayslipGenerator.Payslips;
using Datacom.PayslipGenerator.Services.PaymentFrequency;
using Datacom.PayslipGenerator.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Datacom.PayslipGenerator.Web.Controllers
{
    [Route("api/[controller]/{action}")]
    public class PayslipController : ControllerBase
    {
        private const string AnonynousName = "Anonymous";
        private static readonly IPaymentFrequency _defaultPaymentFrequency = new MonthlyPaymentFrequency();
        private readonly IPayslipGenerator _payslipGenerator;

        public PayslipController(IPayslipGenerator payslipGenerator)
        {
            _payslipGenerator = payslipGenerator;
        }

        [HttpPost]
        [ActionName("GenerateList")]
        public async Task<IActionResult> GenerateListAsync(
            [FromBody]
            IEnumerable<GeneratePayslipModel> models)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payslips = new List<PayslipModel>();

            foreach (var item in models)
            {
                var payslip = await GeneratePayslipAsync(item);

                payslips.Add(payslip);
            }

            return Ok(payslips);
        }

        [HttpPost]
        [ActionName("GenerateSingle")]
        public async Task<IActionResult> GenerateSingleAsync(
            [FromBody]
            GeneratePayslipModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payslip = await GeneratePayslipAsync(model);

            return Ok(payslip);
        }

        private async Task<PayslipModel> GeneratePayslipAsync(GeneratePayslipModel model)
        {
            // These properties should've been validated prior to calling this method
            Debug.Assert(model.AnnualSalary is not null);
            Debug.Assert(model.PaymentPeriod is not null);
            Debug.Assert(model.SuperannuationRate is not null);

            var annualSalary = model.AnnualSalary ?? throw new ArgumentNullException();
            var paymentPeriod = model.PaymentPeriod ?? throw new ArgumentNullException();
            var superannuationRate = model.SuperannuationRate ?? throw new ArgumentNullException();

            var parameter = new PayslipParameter(
                annualSalary,
                paymentPeriod,
                _defaultPaymentFrequency,
                DateTimeOffset.Now.Year,
                superannuationRate);

            var payslip = await _payslipGenerator.GeneratePayslipAsync(parameter, HttpContext.RequestAborted) ?? throw new InvalidOperationException();
            var name = String.Join(" ", model.FirstName, model.LastName).Trim();

            if (String.IsNullOrWhiteSpace(name))
            {
                name = AnonynousName;
            }

            return new PayslipModel
            {
                GrossIncomeAmount = payslip.GrossIncomeAmount,
                GrossIncomeFormattedAmount = payslip.GrossIncomeAmount.ToString("C2"),
                Name = name,
                NetIncomeAmount = payslip.NetIncomeAmount,
                NetIncomeFormattedAmount = payslip.NetIncomeAmount.ToString("C2"),
                PeriodEnd = payslip.PeriodEnd.ToString("dd MMMM yy"),
                PeriodStart = payslip.PeriodStart.ToString("dd MMMM yy"),
                TaxAmount = payslip.TaxAmount,
                TaxFormattedAmount = payslip.TaxAmount.ToString("C2"),
                SuperannuationAmount = payslip.SuperannuationAmount,
                SuperannuationFormattedAmount = payslip.SuperannuationAmount.ToString("C2"),
            };
        }
    }
}