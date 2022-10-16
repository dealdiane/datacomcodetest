using Datacom.PayslipGenerator.Services;

namespace Datacom.PayslipGenerator.Services.Tax;

public sealed class ByBracketTaxCalculator : ITaxCalculator
{
    private readonly IRoundOffStrategy _roundOffStrategy;
    private readonly IEnumerable<TaxBracket> _taxBrackets;

    public ByBracketTaxCalculator(
        IEnumerable<TaxBracket> taxBrackets,
        IRoundOffStrategy roundOffStrategy)
    {
        _taxBrackets = taxBrackets;
        _roundOffStrategy = roundOffStrategy;
    }

    public decimal CalculateTax(decimal annualSalary, IPaymentFrequency paymentFrequency)
    {
        if (annualSalary < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(annualSalary), "Parameter must be a positive number.");
        }

        if (!_taxBrackets.Any())
        {
            return Decimal.Zero;
        }

        var tax = Decimal.Zero;
        var annualPaymentFrequencyCount = (decimal)paymentFrequency.AnnualPaymentFrequencyCount;

        foreach (var taxBracket in _taxBrackets)
        {
            var range = taxBracket.Maximum - taxBracket.Minimum;

            var taxableValue = System.Math.Min(annualSalary, range);
            var taxBracketValue = (taxableValue * (taxBracket.Rate / 100m));

            tax += (taxBracketValue / annualPaymentFrequencyCount);
            annualSalary -= taxableValue;

            if (annualSalary <= 0)
            {
                break;
            }
        }

        return _roundOffStrategy.Round(tax);
    }
}