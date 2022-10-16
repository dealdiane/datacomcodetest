namespace Datacom.PayslipGenerator.Services.PaymentFrequency;

public sealed class FortNightlyPaymentFrequency : BasePaymentFrequency
{
    public FortNightlyPaymentFrequency()
        : base(26)
    {
    }

    public override DateOnly GetEndDateOfPeriod(int period, int year)
    {
        var periodStartDate = GetStartDateOfPeriod(period, year);

        return periodStartDate.AddDays(14).AddDays(-1);
    }

    public override DateOnly GetStartDateOfPeriod(int period, int year)
    {
        if (period < 0 || period > AnnualPaymentFrequencyCount)
        {
            throw new ArgumentOutOfRangeException(nameof(period), $"Parameter must be between 0 and {AnnualPaymentFrequencyCount}.");
        }

        var financialYearStartDate = GetFinancialYearStartDate(year);

        return DateOnly.FromDateTime(financialYearStartDate.AddDays((period - 1) * 14));
    }
}