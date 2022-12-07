using PX.Data;
using PX.Objects.AR;
using System;

namespace InterfaceOWork
{
    public class OCIOW_ARRegisterExt : PXCacheExtension<ARRegister>
    {

        #region Usrcodeservice
        [PXMergeAttributes]
        public virtual string Usrcodeservice { get; set; }
        public abstract class usrcodeservice : PX.Data.BQL.BqlString.Field<usrcodeservice> { }
        #endregion

        #region Usrnumengagement
        [PXMergeAttributes]
        public virtual string Usrnumengagement { get; set; }
        public abstract class usrnumengagement : PX.Data.BQL.BqlString.Field<usrnumengagement> { }
        #endregion

        #region UsrPresentOWork
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Présent dans OWork")]
        public virtual bool? UsrPresentOWork { get; set; }
        public abstract class usrPresentOWork : PX.Data.BQL.BqlBool.Field<usrPresentOWork> { }
        #endregion

        #region UsrOWorkErrorMessage
        [PXDBString(4000, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Message d'erreur")]
        public virtual string UsrOWorkErrorMessage { get; set; }
        public abstract class usrOWorkErrorMessage : PX.Data.BQL.BqlString.Field<usrOWorkErrorMessage> { }
        #endregion

        #region UsrOWorkExportDate
        [PXDBDate()]
        [PXUIField(DisplayName = "Date d'export")]
        public virtual DateTime? UsrOWorkExportDate { get; set; }
        public abstract class usrOWorkExportDate : PX.Data.BQL.BqlDateTime.Field<usrOWorkExportDate> { }
        #endregion

        #region UsrCodeFactureOWork
        [PXDBInt()]
        [PXUIField(DisplayName = "Code facture")]
        public virtual int? UsrCodeFactureOWork { get; set; }
        public abstract class usrCodeFactureOWork : PX.Data.BQL.BqlInt.Field<usrCodeFactureOWork> { }
        #endregion

        #region UsrCodeLiasseOWork
        [PXDBInt()]
        [PXUIField(DisplayName = "Code liasse")]
        public virtual int? UsrCodeLiasseOWork { get; set; }
        public abstract class usrCodeLiasseOWork : PX.Data.BQL.BqlInt.Field<usrCodeLiasseOWork> { }
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
