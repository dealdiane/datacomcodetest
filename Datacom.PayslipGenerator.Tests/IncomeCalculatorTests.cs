using Datacom.PayslipGenerator.Payslips;
using Datacom.PayslipGenerator.Services;
using Datacom.PayslipGenerator.Services.Income;
using Datacom.PayslipGenerator.Services.Tax;
using Moq;

namespace Datacom.PayslipGenerator.Tests;

public class IncomeCalculatorTests
{
    [Test]
    public void InvalidPaymentFrequencyThrows_Throws()
    {
        var a = new Mock<IRoundOffStrategy>();
        var b = new Mock<ITaxCalculatorFactory>();
        var c = new Mock<IPaymentFrequency>();

        c.Setup(x => x.AnnualPaymentFrequencyCount).Returns(0);

        var x = new DefaultIncomeCalculator(
            a.Object,
            b.Object);

        Assert.Throws<InvalidOperationException>(() => x.CalculateIncome(0, c.Object, 0, 0));
    }
}