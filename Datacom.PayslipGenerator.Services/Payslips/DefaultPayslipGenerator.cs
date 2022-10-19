using Datacom.PayslipGenerator.Income;

namespace Datacom.PayslipGenerator.Services.Payslips;

public sealed class DefaultPayslipGenerator : IPayslipGenerator
{
    private readonly IIncomeCalculator _incomeCalculator;

    public DefaultPayslipGenerator(IIncomeCalculator incomeCalculator)
    {
        _incomeCalculator = incomeCalculator;
    }

    public Task<Payslip> GeneratePayslipAsync(PayslipParameter parameter, CancellationToken cancellationToken = default)
    {
        if (parameter.AnnualSalary < 0)
        {
            throw new ArgumentException("Annual salary must be a positive number.");
        }

        if (parameter.SuperannuationRate < 0 || parameter.SuperannuationRate > 50)
        {
            throw new ArgumentOutOfRangeException("Superannuation rate must be between 0 and 50.");
        }

        if (parameter.TaxYear < 1975)
        {
            throw new ArgumentOutOfRangeException("Tax year must be equal to or more than 1975.");
        }

        var paymentFrequency = parameter.PaymentFrequency;

        var incomeDetails = _incomeCalculator.CalculateIncome(
            parameter.AnnualSalary,
            paymentFrequency,
            parameter.TaxYear,
            parameter.SuperannuationRate);

        var periodStartDate = paymentFrequency.GetStartDateOfPeriod(parameter.PaymentPeriod, parameter.TaxYear);
        var periodEndDate = paymentFrequency.GetEndDateOfPeriod(parameter.PaymentPeriod, parameter.TaxYear);

        return Task.FromResult(
            new Payslip(
                incomeDetails.GrossIncomeAmount,
                incomeDetails.NetIncomeAmount,
                incomeDetails.TaxAmount,
                incomeDetails.SuperannuationAmount,
                periodStartDate,
                periodEndDate));
    }
}