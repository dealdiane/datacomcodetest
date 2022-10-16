namespace Datacom.PayslipGenerator.Services;

public interface IRoundOffStrategy
{
    decimal Round(decimal value);

    decimal Round(decimal value, int precision);
}