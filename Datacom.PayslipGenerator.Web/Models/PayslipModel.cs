namespace Datacom.PayslipGenerator.Web.Models;

public record PayslipModel
{
    public decimal GrossIncomeAmount { get; set; }

    public string GrossIncomeFormattedAmount { get; set; } = "-";

    public string Name { get; set; } = String.Empty;

    public decimal NetIncomeAmount { get; set; }

    public string NetIncomeFormattedAmount { get; set; } = "-";

    public string PeriodEnd { get; set; } = String.Empty;

    public string PeriodStart { get; set; } = String.Empty;

    public decimal SuperannuationAmount { get; set; }

    public string SuperannuationFormattedAmount { get; set; } = "-";

    public decimal TaxAmount { get; set; }

    public string TaxFormattedAmount { get; set; } = "-";
}