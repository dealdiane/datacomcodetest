namespace Datacom.PayslipGenerator.Services.PaymentFrequency;

public abstract class BasePaymentFrequency : IPaymentFrequency
{
    protected BasePaymentFrequency(int annualPaymentFrequencyCount)
    {
        if (annualPaymentFrequencyCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(annualPaymentFrequencyCount), "Annual payment frequency must be more than zero.");
        }

        AnnualPaymentFrequencyCount = annualPaymentFrequencyCount;
    }

    public int AnnualPaymentFrequencyCount { get; init; }

    public abstract DateOnly GetEndDateOfPeriod(int period, int year);

    public abstract DateOnly GetStartDateOfPeriod(int period, int year);

    protected DateTime GetFinancialYearStartDate(int year) => new DateTime(year, 1, 1);
}