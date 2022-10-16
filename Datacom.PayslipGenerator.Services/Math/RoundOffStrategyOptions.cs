namespace Datacom.PayslipGenerator.Services;

public record RoundOffStrategyOptions
{
    public MidpointRounding Mode { get; set; } = MidpointRounding.ToEven;

    public int DefaultPrecision { get; set; } = 2;
}
