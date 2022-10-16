using Datacom.PayslipGenerator.Services.PaymentFrequency;

namespace Datacom.PayslipGenerator.Tests;

public class PaymentFrequencyTests
{
    [Test]
    public void Frequency_IsCorrect()
    {
        var monthly = new MonthlyPaymentFrequency();
        var fortNightly = new FortnightlyPaymentFrequency();

        Assert.That(monthly.AnnualPaymentFrequencyCount, Is.EqualTo(12));
        Assert.That(fortNightly.AnnualPaymentFrequencyCount, Is.EqualTo(26));
    }

    [Test]
    public void PeriodEndDate_IsCorrect()
    {
        var monthly = new MonthlyPaymentFrequency();
        var fortNightly = new FortnightlyPaymentFrequency();

        var x1 = monthly.GetEndDateOfPeriod(1, 2022);
        var x2 = monthly.GetEndDateOfPeriod(3, 2022);
        var y1 = fortNightly.GetEndDateOfPeriod(1, 2022);
        var y2 = fortNightly.GetEndDateOfPeriod(4, 2022);

        Assert.That(x1, Is.EqualTo(new DateOnly(2022, 1, 31)));
        Assert.That(x2, Is.EqualTo(new DateOnly(2022, 3, 31)));
        Assert.That(y1, Is.EqualTo(new DateOnly(2022, 1, 14)));
        Assert.That(y2, Is.EqualTo(new DateOnly(2022, 2, 25)));
    }

    [Test]
    public void PeriodStartDate_IsCorrect()
    {
        var monthly = new MonthlyPaymentFrequency();
        var fortNightly = new FortnightlyPaymentFrequency();

        var x1 = monthly.GetStartDateOfPeriod(1, 2022);
        var x2 = monthly.GetStartDateOfPeriod(3, 2022);
        var y1 = fortNightly.GetStartDateOfPeriod(1, 2022);
        var y2 = fortNightly.GetStartDateOfPeriod(4, 2022);

        Assert.That(x1, Is.EqualTo(new DateOnly(2022, 1, 1)));
        Assert.That(x2, Is.EqualTo(new DateOnly(2022, 3, 1)));
        Assert.That(y1, Is.EqualTo(new DateOnly(2022, 1, 1)));
        Assert.That(y2, Is.EqualTo(new DateOnly(2022, 2, 12)));
    }
}