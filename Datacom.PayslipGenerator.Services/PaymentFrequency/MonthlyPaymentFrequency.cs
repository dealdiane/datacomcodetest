namespace Datacom.PayslipGenerator.Services.PaymentFrequency;

public sealed class MonthlyPaymentFrequency : BasePaymentFrequency
{
    public MonthlyPaymentFrequency()
        : base(12)
    {
    }

    public override DateOnly GetEndDateOfPeriod(int period, int year)
    {
        var periodStartDate = GetStartDateOfPeriod(period, year);

        return periodStartDate.AddMonths(1).AddDays(-1);
    }

    public override DateOnly GetStartDateOfPeriod(int period, int year)
    {
        if (period < 0 || period > AnnualPaymentFrequencyCount)
        {
            throw new ArgumentOutOfRangeException(nameof(period), $"Parameter must be between 0 and {AnnualPaymentFrequencyCount}.");
        }

        var financialYearStartDate = GetFinancialYearStartDate(year);

        return DateOnly.FromDateTime(financialYearStartDate.AddMonths(period - 1));
    }
}