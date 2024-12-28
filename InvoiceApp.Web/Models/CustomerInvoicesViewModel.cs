using InvoiceApp.DataAccess.Entities;

namespace InvoiceApp.Web.Models
{
    public class CustomerInvoicesViewModel
    {
        public List<Invoice>? Invoices { get; set; }

        public Customer? Customer { get; set; }
        public InvoiceLineItem? NewItem { get; set; }
        
        public Invoice? NewInvoice { get; set; }

        public Invoice? SelectedInvoice { get; set; }

        public List<PaymentTerm>? PaymentTerms { get; set; }

        public double? LineItemsAmountTotal { get; set; }
    }
}
