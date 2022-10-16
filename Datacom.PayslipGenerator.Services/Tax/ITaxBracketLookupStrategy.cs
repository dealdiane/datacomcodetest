namespace Datacom.PayslipGenerator.Services.Tax;

public interface ITaxBracketLookupStrategy
{
    IEnumerable<TaxBracket> GetTaxBrackets(int year);
}