using Datacom.PayslipGenerator.Services;

namespace Datacom.PayslipGenerator.Services.Tax;

public sealed class DefaultTaxCalculatorFactory : ITaxCalculatorFactory
{
    private readonly IRoundOffStrategy _roundOffStrategy;
    private readonly ITaxBracketLookupStrategy _taxBracketLookupStrategy;

    public DefaultTaxCalculatorFactory(
        IRoundOffStrategy roundOffStrategy,
        ITaxBracketLookupStrategy taxBracketLookupStrategy)
    {
        _roundOffStrategy = roundOffStrategy;
        _taxBracketLookupStrategy = taxBracketLookupStrategy;
    }

    public ITaxCalculator GetTaxCalculator(int year, IPaymentFrequency paymentFrequency)
    {
        if (year == 2022)
        {
            var taxBrackets = _taxBracketLookupStrategy.GetTaxBrackets(year);

            return new ByBracketTaxCalculator(taxBrackets, _roundOffStrategy);
        }

        return NoopTaxCalculator.Instance;
    }
}