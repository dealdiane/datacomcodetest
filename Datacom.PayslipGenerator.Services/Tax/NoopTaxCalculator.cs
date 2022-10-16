namespace Datacom.PayslipGenerator.Services.Tax;

internal sealed class NoopTaxCalculator : ITaxCalculator
{
    internal static NoopTaxCalculator Instance = new NoopTaxCalculator();

    private NoopTaxCalculator()
    {
    }

    public decimal CalculateTax(decimal annualSalary, IPaymentFrequency paymentFrequency) => Decimal.Zero;
}