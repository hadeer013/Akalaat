using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Models
{

    public class Category: BaseEntity
    {

        [Required]
        [MinLength(5, ErrorMessage = "Name must be at least 3 characters long.")]
        public string Name { get; set; }

        public string Category_image { get; set; }

        public string Description { get; set; }

        [ForeignKey("Menu")]
        public int? Menu_ID { get; set; }
        public virtual Menu Menu { get; set; }
    }
}
