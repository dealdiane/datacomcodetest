using Microsoft.Extensions.Options;

namespace Datacom.PayslipGenerator.Services;

public sealed class RoundOffStrategy : IRoundOffStrategy
{
    private readonly RoundOffStrategyOptions _options;

    public RoundOffStrategy(IOptions<RoundOffStrategyOptions> options)
    {
        _options = options.Value;
    }

    public decimal Round(decimal value) => Round(value, _options.DefaultPrecision);

    public decimal Round(decimal value, int precision)
    {
        if (precision < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(precision), "Parameter must be a positive number");
        }

        return System.Math.Round(value, precision, _options.Mode);
    }
}