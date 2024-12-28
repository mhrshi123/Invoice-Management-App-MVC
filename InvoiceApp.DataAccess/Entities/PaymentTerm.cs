using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceApp.DataAccess.Entities
{
    public class PaymentTerm
    {
        public int PaymentTermId { get; set; }

        public string? Description { get; set; } = null!;

        public int? DueDays { get; set; }

        public List<Invoice>? Invoices { get; set; }
    }
}
