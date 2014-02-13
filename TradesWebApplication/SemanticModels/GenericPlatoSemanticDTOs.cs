using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradesWebApplication.SemanticModels
{
    public class GenericPlatoSemanticDTO
    {
            public string @type { get; set; }
            public string @id { get; set; }
            public string canonicalLabel { get; set; }   
    }

    public class GenericPlatoSemanticWithTypeDTO
    {
        public string Type { get; set; }
        public string @type { get; set; }
        public string @id { get; set; }
        public string canonicalLabel { get; set; }
    }

    public class GenericIdPlatoSemanticDTO
    {
        public string @id { get; set; }
    }
}