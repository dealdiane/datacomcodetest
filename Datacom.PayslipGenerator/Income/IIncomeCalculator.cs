namespace Datacom.PayslipGenerator.Income
{
    public interface IIncomeCalculator
    {
        IncomeDetails CalculateIncome(decimal annualSalary, IPaymentFrequency paymentFrequency, int taxYear, decimal superannuationRate);
    }
}