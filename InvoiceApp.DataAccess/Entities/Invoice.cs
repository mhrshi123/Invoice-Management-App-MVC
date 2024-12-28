

using System.ComponentModel.DataAnnotations;

namespace InvoiceApp.DataAccess.Entities
{
    public class Invoice
    {
        public int InvoiceId { get; set; }

        [Required(ErrorMessage = "Please enter invoice date!")]
        public DateTime? InvoiceDate { get; set; }

        public DateTime? InvoiceDueDate
        {
            get
            {
                return InvoiceDate?.AddDays(Convert.ToDouble(PaymentTerms?.DueDays));
            }
        }

        public double? PaymentTotal { get; set; } = 0.0;

        public DateTime? PaymentDate { get; set; }

        [Required(ErrorMessage ="Payment term is required")]
        public int PaymentTermsId { get; set; }

        public PaymentTerm? PaymentTerms { get; set; }

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public List<InvoiceLineItem>? LineItems { get; set; }


    }
}
