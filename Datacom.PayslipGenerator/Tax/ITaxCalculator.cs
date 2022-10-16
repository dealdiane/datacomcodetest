using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datacom.PayslipGenerator.Tax;

public interface ITaxCalculator
{
    decimal CalculateTax(decimal annualSalary, IPaymentFrequency paymentFrequency);
}
