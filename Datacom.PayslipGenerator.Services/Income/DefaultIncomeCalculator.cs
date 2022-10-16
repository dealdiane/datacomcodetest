using Datacom.PayslipGenerator.Income;

namespace Datacom.PayslipGenerator.Services.Income;

public sealed class DefaultIncomeCalculator : IIncomeCalculator
{
    private readonly IRoundOffStrategy _roundOffStrategy;
    private readonly ITaxCalculatorFactory _taxCalculatorFactory;

    public DefaultIncomeCalculator(
        IRoundOffStrategy roundOffStrategy,
        ITaxCalculatorFactory taxCalculatorFactory)
    {
        _roundOffStrategy = roundOffStrategy;
        _taxCalculatorFactory = taxCalculatorFactory;
    }

    public IncomeDetails CalculateIncome(decimal annualSalary, IPaymentFrequency paymentFrequency, int taxYear, decimal superannuationRate)
    {
        if (paymentFrequency.AnnualPaymentFrequencyCount <= 0)
        {
            throw new InvalidOperationException("Invalid payment frequency.");
        }

        var annualPaymentFrequencyCount = (decimal)paymentFrequency.AnnualPaymentFrequencyCount; // Cast ensures that divisions are performed using floating point
        var grossIncome = _roundOffStrategy.Round(annualSalary / annualPaymentFrequencyCount);
        var taxCalculator = _taxCalculatorFactory.GetTaxCalculator(taxYear, paymentFrequency);
        var tax = taxCalculator.CalculateTax(annualSalary, paymentFrequency);
        var netIncome = _roundOffStrategy.Round(grossIncome - tax);
        var superannuationAmount = _roundOffStrategy.Round(grossIncome * (superannuationRate / 100m));

        return new IncomeDetails(grossIncome, tax, netIncome, superannuationAmount);
    }
}