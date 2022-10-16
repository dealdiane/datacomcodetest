using Datacom.PayslipGenerator.Services.PaymentFrequency;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace Datacom.PayslipGenerator.Console;

public sealed class ConsoleHostedService : IHostedService
{
    private readonly IPayslipGenerator _payslipGenerator;

    public ConsoleHostedService(IPayslipGenerator payslipGenerator)
    {
        _payslipGenerator = payslipGenerator;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var parameterA = new PayslipParameter(60_050, 3, new MonthlyPaymentFrequency(), 2022, 9);
        var payslipA = await _payslipGenerator.GeneratePayslipAsync(parameterA);

        Debug.Assert(payslipA.GrossIncomeAmount == 5_004.17m);
        Debug.Assert(payslipA.NetIncomeAmount == 4_084.59m);
        Debug.Assert(payslipA.TaxAmount == 919.58m);
        Debug.Assert(payslipA.SuperannuationAmount == 450.38m);
        Debug.Assert(payslipA.PeriodStart == new DateOnly(2022, 03, 01));
        Debug.Assert(payslipA.PeriodEnd == new DateOnly(2022, 03, 31));

        var parameterB = new PayslipParameter(120_000, 1, new MonthlyPaymentFrequency(), 2022, 10);
        var payslipB = await _payslipGenerator.GeneratePayslipAsync(parameterB);

        Debug.Assert(payslipB.GrossIncomeAmount == 10_000m);
        Debug.Assert(payslipB.NetIncomeAmount == 7_456.67m);
        Debug.Assert(payslipB.TaxAmount == 2_543.33m);
        Debug.Assert(payslipB.SuperannuationAmount == 1_000m);
        Debug.Assert(payslipB.PeriodStart == new DateOnly(2022, 01, 01));
        Debug.Assert(payslipB.PeriodEnd == new DateOnly(2022, 01, 31));

        var parameterC = new PayslipParameter(120_000, 1, new FortNightlyPaymentFrequency(), 2022, 10);
        var payslipC = await _payslipGenerator.GeneratePayslipAsync(parameterC);

        Debug.Assert(payslipC.GrossIncomeAmount == 4_615.38m);
        Debug.Assert(payslipC.NetIncomeAmount == 3_441.53m);
        Debug.Assert(payslipC.TaxAmount == 1_173.85m);
        Debug.Assert(payslipC.SuperannuationAmount == 461.54m);
        Debug.Assert(payslipC.PeriodStart == new DateOnly(2022, 01, 01));
        Debug.Assert(payslipC.PeriodEnd == new DateOnly(2022, 01, 14));

        System.Console.WriteLine("Assertions passed. Press CTRL+C to exit.");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}