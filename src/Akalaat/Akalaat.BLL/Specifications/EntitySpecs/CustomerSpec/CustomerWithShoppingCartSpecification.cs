using Akalaat.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Specifications.EntitySpecs.CustomerSpec
{
    public class CustomerWithShoppingCartSpecification : BaseSpecification<Customer>
    {
        public CustomerWithShoppingCartSpecification()
        {
            AddInclude(C => C.ShoppingCart); 
        }
        public CustomerWithShoppingCartSpecification(string Id): base(C=>C.Id==Id)
        {
            AddInclude(C => C.ShoppingCart);

        }
    }
}
