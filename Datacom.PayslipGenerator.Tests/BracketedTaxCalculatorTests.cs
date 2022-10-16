using Datacom.PayslipGenerator.Payslips;
using Datacom.PayslipGenerator.Services;
using Datacom.PayslipGenerator.Services.Tax;
using Moq;

namespace Datacom.PayslipGenerator.Tests;

public class BracketedTaxCalculatorTests
{
    [Test]
    public void EmptyTaxBracket_ReturnsZero()
    {
        var a = new Mock<IRoundOffStrategy>();
        var b = new Mock<IPaymentFrequency>();
        var x = new ByBracketTaxCalculator(Enumerable.Empty<TaxBracket>(), a.Object);

        var tax = x.CalculateTax(100_000, b.Object);

        Assert.That(tax, Is.EqualTo(0));
    }

    [Test]
    public void NegativeAnnualSalary_Throws()
    {
        var a = new Mock<IRoundOffStrategy>();
        var b = new Mock<IPaymentFrequency>();
        var x = new ByBracketTaxCalculator(Enumerable.Empty<TaxBracket>(), a.Object);

        Assert.Throws<ArgumentOutOfRangeException>(() => x.CalculateTax(-1, b.Object));
    }

    [Test]
    public void Tax_IsCorrect()
    {
        var a = new Mock<IRoundOffStrategy>();
        var b = new Mock<IPaymentFrequency>();

        a.Setup(x => x.Round(30_000))
            .Returns(30_000);

        b.Setup(x => x.AnnualPaymentFrequencyCount)
            .Returns(1);

        var x = new ByBracketTaxCalculator(
            new[]
            {
                new TaxBracket(00_000m, 050_000m, 50m),
                new TaxBracket(50_000m, 100_000m, 10m)
            }, a.Object);

        var tax = x.CalculateTax(100_000, b.Object);

        Assert.That(tax, Is.EqualTo(30_000));
    }
}