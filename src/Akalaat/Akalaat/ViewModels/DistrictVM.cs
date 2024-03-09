using Akalaat.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akalaat.ViewModels
{
	public class DistrictVM
	{
		public string Name { get; set; }
		public int City_ID { get; set; }
		public string City_Name { get; set; }
	}
}
