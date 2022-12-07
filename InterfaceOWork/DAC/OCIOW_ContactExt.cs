using PX.Data;
using PX.Objects.CR;
using PX.Objects.FA;

namespace InterfaceOWork
{
    public class OCIOW_ContactExt : PXCacheExtension<Contact>
    {
        public abstract class usrContactDemat : PX.Data.BQL.BqlBool.Field<usrContactDemat> { }
    }
}
