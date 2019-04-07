using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Warehouse.Models.Requests
{
    public class CustomerCreateRequest
    {

		/// <summary>
	    /// First name
	    /// </summary>
	    [JsonProperty(Required = Required.Always)]
	    public string FirstName { get; set; }

	    /// <summary>
	    /// Last name
	    /// </summary>
	    [JsonProperty(Required = Required.Always)]
	    public string LastName { get; set; }

	    /// <summary>
	    /// Age
	    /// </summary>
	    [JsonProperty(Required = Required.Always)]
	    public byte Age { get; set; }

	    /// <summary>
	    /// Creation date
	    /// </summary>
	    [JsonProperty(Required = Required.Always)]
	    public DateTime CreationDate { get; set; }
	}
}
