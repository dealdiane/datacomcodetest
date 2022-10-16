namespace Datacom.PayslipGenerator.Payslips;

public record PayslipParameter(decimal AnnualSalary, int PaymentPeriod, IPaymentFrequency PaymentFrequency, int TaxYear, decimal SuperannuationRate);
