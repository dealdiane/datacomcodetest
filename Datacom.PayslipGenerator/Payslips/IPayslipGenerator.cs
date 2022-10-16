namespace Datacom.PayslipGenerator.Payslips;

public interface IPayslipGenerator
{
    Task<Payslip> GeneratePayslipAsync(PayslipParameter parameter, CancellationToken cancellationToken = default);
}