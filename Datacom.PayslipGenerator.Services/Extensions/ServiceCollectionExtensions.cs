using Datacom.PayslipGenerator.Income;
using Datacom.PayslipGenerator.Services;
using Datacom.PayslipGenerator.Services.Income;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddDefaultPayslipGeneratorServices(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddScoped<IPayslipGenerator, DefaultPayslipGenerator>();
        services.AddScoped<IRoundOffStrategy, RoundOffStrategy>();
        services.AddScoped<ITaxCalculatorFactory, DefaultTaxCalculatorFactory>();
        services.AddScoped<IIncomeCalculator, DefaultIncomeCalculator>();

        // Different lookup service implementations allow tax retrieval from different sources (e.g. IRD API)
        services.AddScoped<ITaxBracketLookupStrategy>(provider => new InMemoryTaxBracketLookupStrategy(
            new Dictionary<int, IEnumerable<TaxBracket>>
            {
                [2022] = new TaxBracket[]
                {
                    (000_000, 014_000, 10.5m),
                    (014_000, 048_000, 17.5m),
                    (048_000, 070_000, 30.0m),
                    (070_000, 180_000, 33.0m),
                    (180_000, 999_000, 39.0m),
                }
            }));
    }
}