using InvoiceApp.Web.Models;
using InvoiceApp.DataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using InvoiceApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InvoiceApp.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IInvoiceManagerService _invoiceManagerService;

        public CustomerController(IInvoiceManagerService InvoiceManagerService)
        {
            _invoiceManagerService = InvoiceManagerService;
        }

        [HttpGet("customers/from_{startingLetter}-{endingLetter}")]
        public IActionResult AllCustomers(char startingLetter, char endingLetter)
        {


            if (startingLetter == null || endingLetter == null)
            {
                return RedirectToAction("AllCustomers", new { startingLetter = 'A', endingLetter = 'E' });
            }

            var viewModel = new CustomersViewModel()
            {

                Customers = _invoiceManagerService.GetCustomerByCategory(startingLetter, endingLetter),
                Categories = _invoiceManagerService.GetAllCategories(),
                ActiveCategory = $"{startingLetter}-{endingLetter}"
            };
            return View("All", viewModel);
        }

        [HttpGet("customers")]
        public IActionResult AllCustomers(int customerId)
        {

            var viewModel = new CustomersViewModel()
            {

                Customers = _invoiceManagerService.GetCustomerByCategory('A', 'E'),
                Categories = _invoiceManagerService.GetAllCategories(),
                ActiveCategory = $"A-E"
            };
            return View("All", viewModel);
        }

        [HttpGet("customers/add")]
        public IActionResult AddCustomer()
        {
            var viewModel = new Customer();

            return View("Add", viewModel);
        }

        [HttpPost("customers/add")]
        public IActionResult AddCustomer(Customer viewModel)
        {
            if (!ModelState.IsValid)
            {

                return View("Add", viewModel);
            }

            var customerCategory = _invoiceManagerService.GetCustomerCategory(viewModel.Name);

            _invoiceManagerService.AddCustomer(viewModel);
            TempData["Message"] = $"A customer name {viewModel.Name} added successfully";
            TempData["className"] = "success";
            return RedirectToAction("AllCustomers", new
            {
                startingLetter = customerCategory[0],
                endingLetter = customerCategory[2]
            });

        }



        [HttpGet("customers/edit/{customerId}")]
        public IActionResult EditCustomer(int customerId)
        {
            var customer = _invoiceManagerService.GetCustomerById(customerId);

            if (customer == null)
            {
                return NotFound("Customer not found");
            }


            return View("Edit", customer);

        }


        [HttpPost("customers/edit/{customerId}")]
        public IActionResult EditCustomer(Customer viewModel)
        {
            if (!ModelState.IsValid)
            {

                return View("Edit", viewModel);
            }

            var customerCategory = _invoiceManagerService.GetCustomerCategory(viewModel.Name);


            _invoiceManagerService.UpdateCustomer(viewModel);
            TempData["Message"] = $"A customer name {viewModel.Name} updated successfully";
            TempData["className"] = "info";
            return RedirectToAction("AllCustomers", new
            {
                startingLetter = customerCategory[0],
                endingLetter = customerCategory[2]
            });

        }

        [HttpGet("customers/delete/{customerId}/{startingLetter}-{endingLetter}")]
        public IActionResult ToggleDeleteCustomer(int customerId, char startingLetter, char endingLetter)
        {
            var customerToToggleDeletion = _invoiceManagerService.GetCustomerById(customerId);

            if (_invoiceManagerService.GetCustomerById(customerId) == null)
            {
                return NotFound("Customer not found");
            }

            _invoiceManagerService.ToggleCustomerDeletion(customerToToggleDeletion);
            if (customerToToggleDeletion.IsDeleted == true)
            {
                TempData["DeleteMessage"] = $"A customer name {customerToToggleDeletion.Name} was deleted";
                TempData["DeletedCustomerId"] = $"{customerToToggleDeletion.CustomerId}";
                TempData["deleteClassName"] = "danger";
                TempData["DeletedCustomerCategory"] = $"{startingLetter}-{endingLetter}";

            }
            else
            {
                var newCategory = _invoiceManagerService.GetCustomerCategory(customerToToggleDeletion.Name);
                TempData["UndoCustomerCategory"] = newCategory;
            }


            return RedirectToAction("AllCustomers", new { startingLetter, endingLetter });
        }

        [HttpGet("customers/invoices/{customerId}")]
        public IActionResult ManageInvoices(int customerId)
        {
            var customer = _invoiceManagerService.GetCustomerById(customerId);

            if (customer == null)
            {
                return NotFound("Customer not found");
            }
            Invoice invoice = _invoiceManagerService.GetFirstInvoiceByCustomer(customerId);

            var viewModel = new CustomerInvoicesViewModel()
            {
                Customer = customer,
                LineItemsAmountTotal = _invoiceManagerService.GetTotalLineItemsAmount(invoice),
                NewInvoice = new Invoice(),
                NewItem = new InvoiceLineItem(),
                Invoices = _invoiceManagerService.GetAllInvoicesByCustomerId(customerId),
                SelectedInvoice = _invoiceManagerService.GetFirstInvoiceByCustomer(customerId),
                PaymentTerms = _invoiceManagerService.GetAllPaymentTerms()

            };



            return View("Invoices", viewModel);
        }

        [HttpGet("customers/invoices/{customerId}/{invoiceId}")]
        public IActionResult ManageInvoices(int customerId, int invoiceId)
        {
            var customer = _invoiceManagerService.GetCustomerById(customerId);
            var invoice = _invoiceManagerService.GetInvoiceById(invoiceId);

            if (customer == null)
            {
                return NotFound("Customer not found");
            }
            var viewModel = new CustomerInvoicesViewModel()
            {
                LineItemsAmountTotal = _invoiceManagerService.GetTotalLineItemsAmount(invoice),
                Customer = customer,
                NewInvoice = new Invoice(),
                NewItem = new InvoiceLineItem(),
                Invoices = _invoiceManagerService.GetAllInvoicesByCustomerId(customerId),
                SelectedInvoice = invoice,
                PaymentTerms = _invoiceManagerService.GetAllPaymentTerms()


            };
            return View("Invoices", viewModel);
        }




        [HttpPost("customers{customerId}/invoices/add")]
        public IActionResult AddInvoice(CustomerInvoicesViewModel viewModel, int customerId)
        {


            viewModel.Invoices = _invoiceManagerService.GetAllInvoicesByCustomerId(customerId);
            viewModel.SelectedInvoice = _invoiceManagerService.GetFirstOrDefaultInvoice(_invoiceManagerService.GetCustomerById(customerId));
            viewModel.PaymentTerms = _invoiceManagerService.GetAllPaymentTerms();
            viewModel.LineItemsAmountTotal = _invoiceManagerService.GetTotalLineItemsAmount(viewModel.SelectedInvoice);
            viewModel.Customer = _invoiceManagerService.GetCustomerById(customerId);

            if (!ModelState.IsValid)
            {
                return View("Invoices", viewModel);
            }

            var newInvoice = new Invoice
            {
                InvoiceDate = viewModel.NewInvoice.InvoiceDate,
                Customer = viewModel.Customer,

                PaymentTermsId = viewModel.NewInvoice.PaymentTermsId,
            };

            _invoiceManagerService.AddInvoiceToCustomer(newInvoice.Customer.CustomerId, newInvoice);


            return RedirectToAction("ManageInvoices", new { customerId = newInvoice.Customer.CustomerId });
        }

        [HttpPost("customers/{customerId}/invoices/{invoiceId}/addlineitem")]
        public IActionResult AddLineItem(CustomerInvoicesViewModel viewModel, int invoiceId, int customerId)
        {
            viewModel.Invoices = _invoiceManagerService.GetAllInvoicesByCustomerId(viewModel.Customer.CustomerId);
            viewModel.SelectedInvoice = _invoiceManagerService.GetInvoiceById(invoiceId);
            viewModel.PaymentTerms = _invoiceManagerService.GetAllPaymentTerms();
            viewModel.LineItemsAmountTotal = _invoiceManagerService.GetTotalLineItemsAmount(viewModel.SelectedInvoice);
            viewModel.Customer = _invoiceManagerService.GetCustomerById(customerId);

            if (!ModelState.IsValid)
            {
                return View("Invoices", viewModel);

            }

            var NewItem = new InvoiceLineItem
            {
                Description = viewModel.NewItem.Description,
                Amount = viewModel.NewItem.Amount,
                InvoiceId = invoiceId,
                Invoice = viewModel.SelectedInvoice
            };

            _invoiceManagerService.AddInvoiceLineItemToInvoice(NewItem, viewModel.SelectedInvoice);

            return RedirectToAction("ManageInvoices", new { customerId = customerId, invoiceId = invoiceId });
        }

    }
}
