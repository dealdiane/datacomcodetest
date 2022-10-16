using Datacom.PayslipGenerator.Services;
using Microsoft.Extensions.Options;

namespace Datacom.PayslipGenerator.Tests;

public class RoundOffTests
{
    [Test]
    public void DefaultOption_IsExpected()
    {
        var options = Options.Create(new RoundOffStrategyOptions());

        Assert.That(options.Value.Mode, Is.EqualTo(MidpointRounding.ToEven));
        Assert.That(options.Value.DefaultPrecision, Is.EqualTo(2));
    }

    [Test]
    public void DefaultOption_RoundsMid()
    {
        var options = Options.Create(new RoundOffStrategyOptions());
        var instance = new RoundOffStrategy(options);

        var x = instance.Round(0.656m);
        var y = instance.Round(0.651m);

        Assert.That(x, Is.EqualTo(0.66m));
        Assert.That(y, Is.EqualTo(0.65m));
    }
}