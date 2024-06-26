﻿using Akalaat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.BLL.Specifications.EntitySpecs.AddressBookSpec
{
    public class AddresswithRegionSpec : BaseSpecification<Address_Book>
    {
        public AddresswithRegionSpec(string Id) : base(b => b.Customer_ID == Id)
        {
            AddThenInclude(ad => ad.Include(b => b.Region).ThenInclude(r => r.District).ThenInclude(d => d.City));
        }
    }
}
