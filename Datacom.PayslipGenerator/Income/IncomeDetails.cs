namespace Datacom.PayslipGenerator.Income;

public record IncomeDetails(decimal GrossIncomeAmount, decimal TaxAmount, decimal NetIncomeAmount, decimal SuperannuationAmount);