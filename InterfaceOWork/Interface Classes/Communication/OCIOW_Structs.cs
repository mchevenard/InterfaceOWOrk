using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceOWork
{
    /// <summary>
    /// Réprésente une liasse de facture dans O'Work
    /// </summary>
    public struct OCIOW_InvoiceListStruct
    {
        /// <summary>
        /// Nom de la liasse de facture dans O'Work
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Liste des factures à envoyer dans à O'Work
        /// </summary>
        public List<OCIOW_InvoiceWS> invoices { get; set; } 
        /// <summary>
        /// Précise le type à utiliser l'envoi du mail
        /// </summary>
        public string type { get; set; }
    }

    /// <summary>
    /// Réprésente une liste d'attachments à attacher à des factures
    /// </summary>
    public struct OCIOW_AttachmentListStruct
    {
        /// <summary>
        /// Liste des attachments à envoyer
        /// </summary>
        public List<OCIOW_AttachmentWS> attachments { get; set; }
    }

    /// <summary>
    /// Stucture reflétant le Json retourné par la route create_invoice en cas d'intégration réussie
    /// </summary>
    public struct OCIOW_CreateInvoiceRequestResult
    {
        /// <summary>
        /// Liste des indentifiant des factures créées dans O'Work
        /// </summary>
        public List<int> InvoicesIds { get; set; }
        /// <summary>
        /// Identifiant de la liasse de facture créée dans O'Work
        /// </summary>
        public int editionId { get; set; }
        /// <summary>
        /// Inidque si les enregistrement ont bien étés exportés
        /// </summary>
        public bool isExported { get; set; }
        /// <summary>
        /// Potentiel message d'erreur
        /// </summary>
        public string errorMessage { get; set; }
        /// <summary>
        /// Date de l'export
        /// </summary>
        public DateTime exportDate { get; set; }
    }

    /// <summary>
    /// Structure représentant l'interprétation de la reponse à la requète d'envoie d'attachments
    /// </summary>
    public struct OCIOW_AddAttachmentRequestResult
    {
        /// <summary>
        /// Définit si l'attachment a bien été exporté
        /// </summary>
        public bool isExported { get; set; }
        /// <summary>
        /// Message d'erreur au cas où l'export s'est mal déroulé
        /// </summary>
        public string errorMessage { get; set; }
    }
}
