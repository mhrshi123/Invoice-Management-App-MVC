using InvoiceApp.DataAccess.Entities;

namespace InvoiceApp.Web.Models
{
    public class CustomersViewModel
    {
        public List<Customer>? Customers { get; set; }
        
        public List<string> Categories { get; set; }

        public string ActiveCategory { get; set; }


    }
}
