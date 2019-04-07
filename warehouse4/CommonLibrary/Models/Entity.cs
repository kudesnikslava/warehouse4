using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonLibrary.Models
{
	/// <summary>
	/// 
	/// </summary>
    public class Entity : BaseModelWithId
    {

	    public string Name { get; set; }

	    public DateTime CreatedDate { get; set; }

	    public int AvailableQuantity { get; set; }
	}
}
