using Akalaat.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akalaat.ViewModels
{
    public class AddAddressBookVM
    {
        public string AddressDetails { get; set; }
        public int Region_ID { get; set; }
    }
}
