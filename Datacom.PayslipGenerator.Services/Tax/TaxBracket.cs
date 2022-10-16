namespace Datacom.PayslipGenerator.Services.Tax;

public record TaxBracket(decimal Minimum, decimal Maximum, decimal Rate)
{
    public static implicit operator TaxBracket((decimal, decimal, decimal) value) => new TaxBracket(value.Item1, value.Item2, value.Item3);
}