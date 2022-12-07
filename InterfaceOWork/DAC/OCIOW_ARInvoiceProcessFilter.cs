using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceOWork
{
    public class OCIOW_ARInvoiceProcessFilter : IBqlTable
    {
        [PXBool]
        [PXUIField(DisplayName = "N'afficher que les nouvelles factures")]
        [PXDefault(true)]
        public virtual bool? DisplayNewInvoicesOnly { get; set; }
        public abstract class displayNewInvoicesOnly : PX.Data.BQL.BqlBool.Field<displayNewInvoicesOnly> { }
    }
}
