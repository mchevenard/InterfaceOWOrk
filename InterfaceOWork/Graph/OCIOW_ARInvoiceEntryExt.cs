using BibliothequeFonctions;
using InterfaceOWork.DAC;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Data.Update.ExchangeService;
using PX.Objects.AR;
using PX.Objects.CR;
using PX.Objects.GL;
using PX.Objects.GL.DAC;
using System;
using System.Collections;
using System.Collections.Generic;

namespace InterfaceOWork
{
    public class OCIOW_ARInvoiceEntryExt : PXGraphExtension<ARInvoiceEntry>
    {
        public PXSetup<OCIOW_Setup> OWork_Setup;

        public PXAction<ARInvoice> exportToOWork;
        [PXButton(CommitChanges = true)]
        [PXUIField(DisplayName = "Exporter à O'Work")]
        protected virtual IEnumerable ExportToOWork(PXAdapter adapter)
        {
            ExportFactureOWork();
            return adapter.Get();
        }

        #region Events
        protected virtual void _RowSelectedForm(Events.RowSelected<ARInvoice> e)
        {
            ARInvoice row = e.Row;
            if (row == null) return;
            exportToOWork.SetVisible((row.Status == ARDocStatus.Closed || row.Status == ARDocStatus.Open) && (row.DocType == ARInvoiceType.Invoice || row.DocType == ARDocType.CreditMemo));
        }

        protected virtual void _CustomerIDFieldUpdated(Events.FieldUpdated<ARInvoice.customerID> e)
        {
            ARInvoice row = (ARInvoice)e.Row;
            if (row == null) return;
            Customer cus = Base.OCI_SelectById<Customer, Customer.bAccountID>(row.CustomerID);
            if (cus == null) return;

            OCIOW_BaccountExt cusExt = cus.GetExtension<OCIOW_BaccountExt>();
            OCIOW_ARRegisterExt rowExt = row.GetExtension<OCIOW_ARRegisterExt>();
            rowExt.UsrOWForceStore = cusExt.UsrOWForceStore;
        }
        #endregion

        public void ExportFactureOWork()
        {
            OCIOW_InvoiceWS invWS = Mapping();
            OCIOW_CreateInvoiceRequestResult result = OCIOW_InvoiceWS.SendInvoiceList(new List<OCIOW_InvoiceWS> { invWS }, invWS.invoice_number + " par " + Base.Accessinfo.UserName);
            GestionErreurOWork(Base.Document.Current, result);
            Base.Save.Press();
        }

        /// <summary>
        /// Fonction de mapping des données à partir d'un ARInvoice (chargé dans le current de la vue Document du graph) en OCIOW_InvoiceWS
        /// </summary>
        /// <returns>OCIOW_InvoiceWS mappé et peuplé correctement</returns>
        /// <exception cref="PXException"></exception>
        public virtual OCIOW_InvoiceWS Mapping()
        {
            //Récupération des informations nécessaire à l'envoi de la facture à O'Work
            ARInvoice inv = Base.Document.Current;
            if(inv == null)  new PXException(OCIOW_Exceptions.CantGetInvoiceInfos);

            if (inv.DocType != ARDocType.CreditMemo && inv.DocType != ARDocType.Invoice) throw new PXException(OCIOW_Exceptions.WrongDocType);
            bool isRefund = inv.DocType == ARDocType.CreditMemo;

            ARAddress add = Base.Billing_Address.Select();
            if (add == null) throw new PXException(OCIOW_Exceptions.NoAddressFound, inv.BillAddressID, inv.RefNbr);

            Customer cus = Base.customer.Select();
            if (cus == null) throw new PXException(OCIOW_Exceptions.NoCustomerFound, inv.CustomerID, inv.RefNbr);
            OCIOW_BaccountExt cusExt = cus.GetExtension<OCIOW_BaccountExt>();

            Location loc = Base.OCI_SelectById<Location, Location.locationID>(cus.DefLocationID);
            if (loc == null) throw new PXException(OCIOW_Exceptions.NoDefLocationFound, cus.DefLocationID, cus.AcctCD);
            OCIOW_LocationExt locExt = loc.GetExtension<OCIOW_LocationExt>();

            string email = "";
            Contact billContact = Base.OCI_SelectById<Contact, Contact.contactID>(cus.DefBillContactID);
            if (billContact != null)
                email = billContact.EMail;
            //ON selectionne tout les conatct démat lié au client de la facture et on envoie leur email, conccaténé par des ;
            PXResultset<Contact> conPXRS = new PXResultset<Contact>();
            conPXRS.AddRange(SelectFrom<Contact>.Where<Contact.bAccountID.IsEqual<@P.AsInt>.And<OCIOW_ContactExt.usrContactDemat.IsEqual<True>>>.View.Select(Base, cus.BAccountID));
            List<Contact> conList = conPXRS.PXRStoList();
            foreach(Contact con in conList)
            {
                if (!String.IsNullOrEmpty(email))
                    email += ";";
                email += con.EMail;
            }

            OCIOW_ARRegisterExt invExt = inv.GetExtension<OCIOW_ARRegisterExt>();

            //Récupération du siret du client

            Branch bra = Base.OCI_SelectById<Branch, Branch.branchID>(inv.BranchID);
            if (bra == null) throw new PXException(OCIOW_Exceptions.NoBranchFound, inv.BranchID);


            Organization org = Base.OCI_SelectById<Organization, Organization.organizationID>(bra.OrganizationID);
            if (org == null) throw new PXException(OCIOW_Exceptions.NoOrganizationFound, bra.OrganizationID);

            BAccount orgBAccount = Base.OCI_SelectById<BAccount, BAccount.bAccountID>(org.BAccountID);
            if (orgBAccount == null) throw new PXException(OCIOW_Exceptions.noOrganizationBAccountFound, org.BAccountID , org.OrganizationID);
            OCIOW_BaccountExt orgBAccountExt = orgBAccount.GetExtension<OCIOW_BaccountExt>();
           
            
            OCIOW_Setup setup = OWork_Setup.Current;
            if (setup == null) throw new PXException(OCIOW_Exceptions.noSetupFound);
            
            OCIOW_InvoiceWS invWS =  new OCIOW_InvoiceWS()
            {
                address_city = add.City,
                address_country = add.CountryID,
                address_zipcode = add.PostalCode,
                amount_residual = inv.CuryDocBal.ToString().Replace(",", "."),
                amount_tax = inv.CuryTaxTotal.ToString().Replace(",", "."),
                amount_total = (inv.CuryVatTaxableTotal + inv.CuryTaxTotal + inv.CuryVatExemptTotal).ToString().Replace(",", "."),
                amount_untaxed = (inv.CuryVatTaxableTotal + inv.CuryVatExemptTotal).ToString().Replace(",", "."),
                customer_code = cus.AcctCD,
                customer_email = email,
                customer_name = cus.AcctName,
                customer_reference = invExt.Usrnumengagement,
                customer_siret = cusExt.CegSiret,
                date_invoice = inv.DocDate.Value.ToString("yyyy-MM-dd"),
                emitter_siret = orgBAccountExt.CegSiret,
                invoice_number = inv.RefNbr,
                is_refund = isRefund,
                name = "",
                service_code = invExt.Usrcodeservice,
                contents_pdf = getPdfBase64(inv, setup),
                connectionInfos = new OCIOW_OWorkWS { Token = setup.BearerToken, BaseUrl = setup.BaseUrl },
                force_store = invExt.UsrOWForceStore.HasValue && invExt.UsrOWForceStore.Value,
                type = bra.BranchCD.Trim()
            };
            if (add.AddressLine1 != null)
            {
                invWS.address_line1 = add.AddressLine1.Substring(0, Math.Min(30, add.AddressLine1.Length));

                invWS.address_name = add.AddressLine1.Substring(0, Math.Min(30, add.AddressLine1.Length));
            }
            if (add.AddressLine2 != null) invWS.address_line2 = add.AddressLine2.Substring(0, Math.Min(30, add.AddressLine2.Length));
            if (add.AddressLine3 != null) invWS.address_line3 = add.AddressLine3.Substring(0, Math.Min(30, add.AddressLine3.Length));
            return invWS;



        }
        
        public void GestionErreurOWork(ARInvoice inv, OCIOW_CreateInvoiceRequestResult result, int index = 0)
        {
            OCIOW_ARRegisterExt invExt = inv.GetExtension<OCIOW_ARRegisterExt>();
            invExt.UsrOWorkErrorMessage = result.errorMessage;
            invExt.UsrPresentOWork = result.isExported;
            invExt.UsrOWorkExportDate = result.exportDate;
            if(result.InvoicesIds != null && result.InvoicesIds.Count > 0)
                invExt.UsrCodeFactureOWork = result.InvoicesIds[index];
            invExt.UsrCodeLiasseOWork = result.editionId;
            Base.Document.Update(inv);

        }
        public virtual string getPdfBase64(ARInvoice inv, OCIOW_Setup setup)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add(nameof(inv.DocType), inv.DocType);
            parameters.Add(nameof(inv.RefNbr), inv.RefNbr);
        
            return Convert.ToBase64String(ReportManagement.getPDFBytes(parameters, setup.ReportCode));
        }
    }
}
