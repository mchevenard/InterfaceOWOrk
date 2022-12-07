using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceOWork
{
    public class OCIOW_BaseWS
    {
        [JsonIgnore]
        public OCIOW_OWorkWS connectionInfos { get; set; }
    }
}
