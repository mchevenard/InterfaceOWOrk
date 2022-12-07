using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System;

namespace InterfaceOWork
{
    public class OCIOW_AttachmentWS : OCIOW_BaseWS
    {
        /// <summary>
        /// Numéro de la facture à laquelle atacher la PJ
        /// </summary>
        public int invoice_id { get; set; }
        /// <summary>
        /// Nom de la PJ
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Contenu de la PJ (doit être un PDf encodé en base 64)
        /// </summary>
        public string contents { get; set; }

        public static OCIOW_AddAttachmentRequestResult SendInvoiceList(List<OCIOW_AttachmentWS> attachmentList)
        {
            OCIOW_AttachmentListStruct stru = new OCIOW_AttachmentListStruct()
            {
                attachments = attachmentList
            };

            string jsonBody = JsonConvert.SerializeObject(stru);
            RestClient client = new RestClient(attachmentList[0].connectionInfos.BaseUrl + "/ext/dinvoice/add_attachments");
            client.Timeout = -1;

            //Création de la requète en POST
            RestRequest request = new RestRequest(Method.POST);

            //Paramétrage de la requète en type jSON et ajout de l'apikey
            request.AddHeader("content-type", "application/json");
            request.AddHeader("Authorization", "Bearer " + attachmentList[0].connectionInfos.Token);

            //Création du JSON et ajout à la requète
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);

            //Envoi de la requète et récupération de la réponse
            IRestResponse response = client.Execute(request);
            OCIOW_AddAttachmentRequestResult result = new OCIOW_AddAttachmentRequestResult() { isExported = false };
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                result.isExported = true;
            else
                result.errorMessage = response.Content;
            return result;


        }
    }
}
