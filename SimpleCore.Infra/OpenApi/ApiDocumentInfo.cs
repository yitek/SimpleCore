using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.OpenApi
{
    public class ApiDocumentInfo
    {
        public string ConstractName { get; set; }
        public string ContractEmail { get; set; }

        public string ContractUrl { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Dictionary<string, ApiDocumentInfo> Versions { get; set; }
    }
}
