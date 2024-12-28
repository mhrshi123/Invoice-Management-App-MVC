using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceApp.DataAccess.Entities
{
    public class InvoiceLineItem
    {
        public int InvoiceLineItemId { get; set; }

        [Required(ErrorMessage = "Please enter amount")]
        public double? Amount { get; set; }

        [Required(ErrorMessage ="Please enter description")]
        public string? Description { get; set; }

        public int? InvoiceId { get; set; }

        public Invoice? Invoice { get; set; }    
    }
}
