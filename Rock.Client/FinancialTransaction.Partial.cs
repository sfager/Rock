using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock.Client
{
    public partial class FinancialTransaction
    {
        public virtual DefinedValue CurrencyTypeValue { get; set; }

        public virtual List<FinancialTransactionImage> Images { get; set; }
    }
}
