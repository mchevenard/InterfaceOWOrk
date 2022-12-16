using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;
using PX.Objects.Common.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceOWork
{
    public class OWIOW_ProcessExportInvoiceMaint : PXGraph<OWIOW_ProcessExportInvoiceMaint>
    {
        public PXCancel<ARInvoice> Cancel;
        public PXFilter<OCIOW_ARInvoiceProcessFilter> Filters;
        [PXFilterable]
        public PXFilteredProcessing<ARInvoice, OCIOW_ARInvoiceProcessFilter> InvoiceProcessView;


        public IEnumerable invoiceProcessView()
        {
            PXResultset<ARInvoice> invPXRS = new PXResultset<ARInvoice>();
            if(Filters.Current.DisplayNewInvoicesOnly.Value)
            {
                    invPXRS.AddRange(PXSelect<ARInvoice, Where<
                ARInvoice.branchID.IsEqual<AccessInfo.branchID.FromCurrent>.
                And<ARInvoice.status.IsEqual<ARDocStatus.open>.Or<ARInvoice.status.IsEqual<ARDocStatus.closed>>>
                .And<ARInvoice.docType.IsEqual<ARDocType.invoice>.Or<ARInvoice.docType.IsEqual<ARInvoiceType.creditMemo>>>
                .And<OCIOW_ARRegisterExt.usrPresentOWork.IsEqual<False>>>>.Select(this));
            }
            else
                invPXRS.AddRange(PXSelect<ARInvoice, Where<
                ARInvoice.branchID.IsEqual<AccessInfo.branchID.FromCurrent>.
                And<ARInvoice.status.IsEqual<ARDocStatus.open>.Or<ARInvoice.status.IsEqual<ARDocStatus.closed>>>
                .And<ARInvoice.docType.IsEqual<ARDocType.invoice>.Or<ARInvoice.docType.IsEqual<ARInvoiceType.creditMemo>>>
                .And<OCIOW_ARRegisterExt.usrPresentOWork.IsEqual<False>.Or<OCIOW_ARRegisterExt.usrPresentOWork.IsNull>>>>.Select(this));

            foreach(var inv in invPXRS)
            {
                yield return inv;
            }
        }     
        public OWIOW_ProcessExportInvoiceMaint()
        {

            InvoiceProcessView.SetProcessCaption("Exporter");

            InvoiceProcessView.SetProcessAllCaption("Tout exporter");

            InvoiceProcessView.SetProcessDelegate(ExportList);

        }

        private static void ExportList(IList<ARInvoice> records)
        {//Récupération des equipements séléctionnés
            List<ARInvoice> invList = (List<ARInvoice>)records;

            //Création d'ungraph setup pour executer a requète du setup
            OCIOW_SetupMaint setupMaint = PXGraph.CreateInstance<OCIOW_SetupMaint>();

            //Récupération du setup
            OCIOW_Setup setup = SelectFrom<OCIOW_Setup>.View.SelectWindowed(setupMaint, 0, 1);

            //Création d'un graph Item et son extension
            ARInvoiceEntry invGraph = PXGraph.CreateInstance<ARInvoiceEntry>();
            OCIOW_ARInvoiceEntryExt invGraphExt = invGraph.GetExtension<OCIOW_ARInvoiceEntryExt>();

            List<OCIOW_InvoiceWS> invWSList = new List<OCIOW_InvoiceWS>();
            //Pour chaque article
            foreach (ARInvoice inv in invList)
            {
                invGraph.Clear();
                invGraph.Cancel.Press();
                invGraph.Document.Current = inv;
                invGraph.Document.UpdateCurrent();
                invWSList.Add(invGraphExt.Mapping());
            }

            OCIOW_CreateInvoiceRequestResult result = OCIOW_InvoiceWS.SendInvoiceList(invWSList, string.Format(OCIWS_Constants.LiasseName, DateTime.Now, invGraph.Accessinfo.UserName));
            string errorMessage = "";
            foreach(int id in result.InvoicesIds)
            {
                try
                {
                    invGraph.Clear();
                    invGraph.Cancel.Press();
                    invGraph.Document.Current = invList[result.InvoicesIds.IndexOf(id)];
                    invGraph.Document.UpdateCurrent();
                    invGraphExt.GestionErreurOWork(invGraph.Document.Current, result, result.InvoicesIds.IndexOf(id));
                    invGraph.Save.Press();
                }
                catch
                {
                    errorMessage += "Impossible de mettre à jour la gestion d'erreur OWork de la facture " + invList[result.InvoicesIds.IndexOf(id)].RefNbr + ". ";
                }
            }

            if(!string.IsNullOrEmpty(errorMessage))
            {
                throw new PXException(errorMessage);
            }
        }
    }
}
