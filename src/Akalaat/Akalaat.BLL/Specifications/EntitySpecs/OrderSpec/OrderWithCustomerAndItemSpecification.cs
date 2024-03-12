using Akalaat.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Specifications.EntitySpecs.OrderSpec
{
    public class OrderWithCustomerAndItemSpecification:BaseSpecification<Order>
    {
        public OrderWithCustomerAndItemSpecification() 
        {
           AddInclude( O=> O.Item);
           AddInclude(O => O.Customer);
            OrderBy = O => O.DateTime;
        }
    }
}
