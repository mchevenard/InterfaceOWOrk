using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceOWork
{
    public class OCIOW_Exceptions
    {
        public const string CantGetInvoiceInfos = "Impossible de récupérer les informations de la facture";
        public const string NoAddressFound = "Impossible de retrouver l'adresse de factuartion {0} pour la facture {1}";
        public const string NoCustomerFound = "Impossible de retrouver les informations du client {0} client pour la facture {1}";
        public const string NoContactFound = "Impossible de retrouver les informations du contact principal ({0}) pour le client {1}";
        public const string NoBranchFound = "Impossible de retrouver les informations de l'établissement {0}";
        public const string NoOrganizationFound = "Impossible de retrouver les informations de la société {0}";
        public const string noOrganizationBAccountFound = "Impossible de retrouver les informations du compte de tiers {0] lié à la société {1}";
        public const string NoDefLocationFound = "Impossible de retrouver les informations de l'addresse par défaut du ({0}) du client {1}";
        public const string noSetupFound = "Veuilez saisir les informations necessaires dans le paramétrage O'Work";
        public const string WrongDocType = "La pièce doit être une facture ou un avoir pour être exporté à O'Work";
    }
}
