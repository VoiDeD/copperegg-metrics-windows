using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace CopperEggLib
{
    /// <summary>
    /// The root class used to making requests to the CopperEgg backend.
    /// </summary>
    public partial class CopperEgg
    {
        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        public string APIKey { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="CopperEgg"/> class.
        /// </summary>
        /// <param name="apiKey">The API key to use.</param>
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
