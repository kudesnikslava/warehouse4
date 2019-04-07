using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Warehouse.Models
{
	/// <summary>
	/// Entiry request
	/// </summary>
	public class EntityCreateRequest
    {
	    [JsonProperty(Required = Required.Always)]
		public string Name { get; set; }

	    [JsonProperty(Required = Required.Always)]
		public DateTime CreatedDate { get; set; }

	    [JsonProperty(Required = Required.Always)]
		public int AvailableQuantity { get; set; }
	}
}
