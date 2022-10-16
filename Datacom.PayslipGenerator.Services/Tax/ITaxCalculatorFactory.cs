namespace Datacom.PayslipGenerator.Services.Tax;

public interface ITaxCalculatorFactory
{
    ITaxCalculator GetTaxCalculator(int year, IPaymentFrequency paymentFrequency);
}