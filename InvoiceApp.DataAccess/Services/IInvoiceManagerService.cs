using InvoiceApp.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceApp.DataAccess.Services
{
    public interface IInvoiceManagerService
    {
        public List<string> GetAllCategories();



        public List<Customer> GetCustomerByCategory(char startingLetter, char endingLetter);

        public Customer? GetCustomerById(int id);

        public string GetCustomerCategory(string customerName);

        //public int AddCustomer(Customer customer);
        public void AddCustomer(Customer customer);

        public void UpdateCustomer(Customer customer);

        public void ToggleCustomerDeletion(Customer customer);

        public List<PaymentTerm> GetAllPaymentTerms();

        public Invoice? GetInvoiceById(int invoiceId);

        public void AddInvoiceToCustomer(int customerId, Invoice newInvoice);

        public void AddInvoiceLineItemToInvoice(InvoiceLineItem invoiceLineItem, Invoice invoice);

        public Invoice? GetFirstOrDefaultInvoice(Customer customer);

        public List<Invoice> GetAllInvoicesByCustomerId(int customerId);

        public Invoice GetFirstInvoiceByCustomer(int customerId);

        public double GetTotalLineItemsAmount(Invoice invoice);



    }
}
