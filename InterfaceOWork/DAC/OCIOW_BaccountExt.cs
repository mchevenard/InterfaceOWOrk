using PX.Data;
using PX.Objects.CR;

namespace InterfaceOWork
{
    public class OCIOW_BaccountExt : PXCacheExtension<BAccount>
    {
        #region CegSiret
        [PXDBString(14, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Siret")]
        public virtual string CegSiret { get; set; }
        public abstract class cegSiret : PX.Data.BQL.BqlString.Field<cegSiret> { }
        #endregion

        #region UsrOWForceStore
        [PXDBBool()]
        [PXUIField(DisplayName = "Mettre au coffre")]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual bool? UsrOWForceStore { get; set; }
        public abstract class usrOWForceStore : PX.Data.BQL.BqlBool.Field<usrOWForceStore> { }
        #endregion

    }
}
