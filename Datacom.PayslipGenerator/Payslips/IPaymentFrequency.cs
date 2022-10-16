namespace Datacom.PayslipGenerator.Payslips;

public interface IPaymentFrequency
{
    int AnnualPaymentFrequencyCount { get; }

    DateOnly GetEndDateOfPeriod(int period, int year);

    DateOnly GetStartDateOfPeriod(int period, int year);
}
