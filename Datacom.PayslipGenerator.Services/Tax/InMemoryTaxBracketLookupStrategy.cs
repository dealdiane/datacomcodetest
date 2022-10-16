namespace Datacom.PayslipGenerator.Services.Tax;

public sealed class InMemoryTaxBracketLookupStrategy : ITaxBracketLookupStrategy
{
    private readonly IDictionary<int, IEnumerable<TaxBracket>> _taxBrackets;

    public InMemoryTaxBracketLookupStrategy(IDictionary<int, IEnumerable<TaxBracket>> taxBrackets)
    {
        _taxBrackets = taxBrackets;
    }

    public IEnumerable<TaxBracket> GetTaxBrackets(int year) => _taxBrackets.TryGetValue(year, out var brackets)
        ? brackets
        : throw new InvalidOperationException($"No tax brackets defined for the year {year}.");
}