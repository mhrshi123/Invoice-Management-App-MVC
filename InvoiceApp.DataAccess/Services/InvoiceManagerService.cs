using InvoiceApp.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceApp.DataAccess.Services
{
    public class InvoiceManagerService : IInvoiceManagerService
    {
        public List<string> _categories = new List<string> { "A-E", "F-K", "L-R", "S-Z" };

        private readonly InvoiceAppDbContext _context;

        public InvoiceManagerService(InvoiceAppDbContext context)
        {
            _context = context;
        }


        public List<string> GetAllCategories()
        {
            return _categories;
        }
        public string GetCustomerCategory(string customerName)
        {
            char firstLetter = customerName.ToUpper()[0];

            if (firstLetter >= 'A' && firstLetter <= 'E') return "A-E";
            if (firstLetter >= 'F' && firstLetter <= 'K') return "F-K";
            if (firstLetter >= 'L' && firstLetter <= 'R') return "L-R";
            if (firstLetter >= 'S' && firstLetter <= 'Z') return "S-Z";

            return "Other";
        }

        public List<Customer> GetCustomerByCategory(char startingLetter, char endingLetter)
        {

            var categoryLetters = new List<string>();

            for (char c = startingLetter; c <= endingLetter; c++)
            {
                categoryLetters.Add(c.ToString());
            }

            return _context.Customers
                .Include(c => c.Invoices) 
                .Where(c => categoryLetters.Any(letter => c.Name.StartsWith(letter) && !c.IsDeleted))
                .ToList();
        }

        public Customer? GetCustomerById(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.CustomerId == id);
        }



        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();

        }

        public void ToggleCustomerDeletion(Customer customer)
        {
            customer.IsDeleted = !customer.IsDeleted;
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }
        public List<Invoice> GetAllInvoicesByCustomerId(int customerId)
        {
            return _context.Invoices.Include(i => i.LineItems).Where(i => i.CustomerId == customerId).ToList();
        }
        public List<PaymentTerm> GetAllPaymentTerms()
        {
            return _context.PaymentTerms.ToList();
        }

        public Invoice? GetInvoiceById(int invoiceId)
        {
            return _context.Invoices.Include(i => i.LineItems).FirstOrDefault(i => i.InvoiceId == invoiceId);
        }

        public void AddInvoiceToCustomer(int customerId, Invoice newInvoice)
        {


            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (customer != null)
            {
                newInvoice.Customer = customer;
                _context.Invoices?.Add(newInvoice);
                _context.SaveChanges();
            }

        }

        public Invoice GetFirstInvoiceByCustomer(int customerId)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);


            return customer.Invoices?.FirstOrDefault();


        }

        public void AddInvoiceLineItemToInvoice(InvoiceLineItem invoiceLineItem, Invoice invoice)
        {
            invoice.LineItems.Add(invoiceLineItem);
            _context.SaveChanges();
        }

        public Invoice? GetFirstOrDefaultInvoice(Customer customer)
        {
            return customer.Invoices?.FirstOrDefault();
        }

        public double GetTotalLineItemsAmount(Invoice invoice)
        {
            return invoice?.LineItems?.Sum(li => li.Amount ?? 0) ?? 0;
        }

    }
}
