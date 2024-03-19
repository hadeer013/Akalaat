using Akalaat.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Specifications.EntitySpecs.ShoppingCartSpec
{
    public class ShoppingCartWithCustomerAndItemSpecification : BaseSpecification<ShoppingCartItem>
    {
        public ShoppingCartWithCustomerAndItemSpecification()
        {
            AddInclude(C => C.Item);
        }
        //public ShoppingCartWithCustomerAndItemSpecification(int Id) : base(s => s.Id == Id)
        //{
        //    AddInclude(C => C.Item);

        //}


    }
}
