using PX.Data;
using PX.Objects.CR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceOWork.DAC
{
    public class OCIOW_LocationExt : PXCacheExtension<Location>
    {
        #region CegSiret
        [PXDBString(14, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Siret")]
        public virtual string CegSiret { get; set; }
        public abstract class cegSiret : PX.Data.BQL.BqlString.Field<cegSiret> { }
        #endregion
    }
}
