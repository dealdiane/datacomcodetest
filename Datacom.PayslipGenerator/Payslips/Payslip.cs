namespace Datacom.PayslipGenerator.Payslips;

public record Payslip(decimal GrossIncomeAmount, decimal NetIncomeAmount, decimal TaxAmount, decimal SuperannuationAmount, DateOnly PeriodStart, DateOnly PeriodEnd);