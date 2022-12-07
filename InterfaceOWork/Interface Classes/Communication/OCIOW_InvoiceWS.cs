
using Newtonsoft.Json;
using PX.Data;
using PX.Objects.AR;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceOWork
{
    public class OCIOW_InvoiceWS : OCIOW_BaseWS
    {
        /// <summary>
        /// Nom du fichier
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Code siret du client
        /// </summary>
        public string customer_siret { get; set; }
        /// <summary>
        /// Email du client
        /// </summary>
        public string customer_email { get; set; }
        /// <summary>
        /// Code client
        /// </summary>
        public string customer_code { get; set; }
        /// <summary>
        /// Nom du client
        /// </summary>
        public string customer_name { get; set; }
        /// <summary>
        /// Nom de l'addresse
        /// </summary>
        public string address_name { get; set; }
        /// <summary>
        /// Adresse ligne 1
        /// </summary>
        public string address_line1 { get; set; }
        /// <summary>
        /// Adresse ligne 2
        /// </summary>
        public string address_line2 { get; set; }
        /// <summary>
        /// Adresse ligne 3
        /// </summary>
        public string address_line3 { get; set; }
        /// <summary>
        /// VIlle
        /// </summary>
        public string address_city { get; set; }
        /// <summary>
        /// Code postal
        /// </summary>
        public string address_zipcode { get; set; }
        /// <summary>
        /// Pays
        /// </summary>
        public string address_country { get; set; }
        /// <summary>
        /// Siret de l'emmeteur
        /// </summary>
        public string emitter_siret { get; set; }
        /// <summary>
        /// Numéro de facture
        /// </summary>
        public string invoice_number { get; set; }
        /// <summary>
        /// Code de service
        /// </summary>
        public string service_code { get; set; }
        /// <summary>
        /// Référence client
        /// </summary>
        public string customer_reference { get; set; }
        /// <summary>
        /// Est un avoir?
        /// </summary>
        public bool is_refund { get; set; }
        /// <summary>
        /// Date de la facture
        /// </summary>
        public string date_invoice { get; set; }
        /// <summary>
        /// Montant HT
        /// </summary>
        public string amount_untaxed { get; set; }
        /// <summary>
        /// Total de taxe
        /// </summary>
        public string amount_tax { get; set; }
        /// <summary>
        /// Total TTC
        /// </summary>
        public string amount_total { get; set; }
        /// <summary>
        /// Montant à payer
        /// </summary>
        public string amount_residual { get; set; }
        /// <summary>
        /// Contenu du fichier PDF encodé en base 64
        /// </summary>
        public string contents_pdf { get; set; }
        /// <summary>
        /// Forcer l'intégration au coffre fort 
        /// </summary>
        public bool force_store { get; set; }
        /// <summary>
        /// Précise le type à utiliser l'envoi du mail
        /// </summary>
        [JsonIgnore]
        public string type { get; set; }




        public static OCIOW_CreateInvoiceRequestResult SendInvoiceList(List<OCIOW_InvoiceWS> invoicesList, string invoiceListName)
        {
            OCIOW_InvoiceListStruct stru = new OCIOW_InvoiceListStruct()
            {
                name = invoiceListName,
                invoices = invoicesList,
                type = invoicesList[0].type
            };

            string jsonBody = JsonConvert.SerializeObject(stru);
            RestClient client = new RestClient(invoicesList[0].connectionInfos.BaseUrl + "/ext/dinvoice/create_invoices");
            client.Timeout = -1;

            //Création de la requète en POST
            RestRequest request = new RestRequest(Method.POST);

            //Paramétrage de la requète en type jSON et ajout de l'apikey
            request.AddHeader("content-type", "application/json");
            request.AddHeader("Authorization", "Bearer " + invoicesList[0].connectionInfos.Token);

            //Création du JSON et ajout à la requète
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);

            //Envoi de la requète et récupération de la réponse
            IRestResponse response = client.Execute(request);
            OCIOW_CreateInvoiceRequestResult result;
            try
            {
                result = JsonConvert.DeserializeObject<OCIOW_CreateInvoiceRequestResult>(response.Content);
                result.isExported = true;
                result.exportDate = DateTime.Now;
            }
            catch
            {
                result = new OCIOW_CreateInvoiceRequestResult
                {
                    errorMessage = response.Content,
                    isExported = false
                };
            }
            return result;


        }
    }
}
