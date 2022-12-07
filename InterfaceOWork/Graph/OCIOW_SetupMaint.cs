using System;
using PX.Data;
using PX.Data.BQL.Fluent;

namespace InterfaceOWork
{
    public class OCIOW_SetupMaint : PXGraph<OCIOW_SetupMaint, OCIOW_Setup>
    {
        public SelectFrom<OCIOW_Setup>.View Setup;
        


    }
}