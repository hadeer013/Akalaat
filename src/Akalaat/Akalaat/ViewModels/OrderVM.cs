﻿using Akalaat.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akalaat.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }

        // Properties for order details
        [Display(Name = "Order Date")]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }

        [Display(Name = "Arrival Time")]
        [DataType(DataType.Time)]
        public DateTime ArrivalTime { get; set; }

        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be a positive number.")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Total Discount")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Total discount must be a positive number.")]
        public decimal TotalDiscount { get; set; }

        public string Customer_ID { get; set; }
        public virtual Customer? Customer { get; set; }
        //  public ICollection<Item> Items { get; set; } = new HashSet<Item>();
        [Display(Name ="Items")]
        public List<int> SelectedItemS { get; set; } = new List<int>();

        public IEnumerable<SelectListItem> Items { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
