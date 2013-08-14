using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace CopperEggLib
{
    public partial class CopperEgg
    {
        public string APIKey { get; set; }


        public CopperEgg( string apiKey = null )
        {
            APIKey = apiKey;
        }


        APIClient GetAPIClient()
        {
            return new APIClient( APIKey );
        }
    }
}
