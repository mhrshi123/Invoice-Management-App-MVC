
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;


namespace InvoiceApp.DataAccess.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Customer's name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Customer's address is required")]
        public string? Address1 { get; set; }


        public string? Address2 { get; set; }

        [Required(ErrorMessage = "Customer's city is required")]
        public string? City { get; set; } = null!;

        [Required(ErrorMessage = "Customer's provice or state is required")]
        [RegularExpression("^[A-Za-z]{2}$", ErrorMessage = "Invalid state code")]
        public string? ProvinceOrState { get; set; } = null!;


        [Required(ErrorMessage = "Customer's zipcode is required")]
        [RegularExpression("^(?:\\d{5}(?:-\\d{4})?|\\d{5}|([A-Za-z]\\d[A-Za-z])[- ]?\\d[A-Za-z]\\d$)", ErrorMessage = "Zip/Postal Code: Must be in a valid US zip or Canadian postal code format.")]

        public string? ZipOrPostalCode { get; set; } = null!;

        [Required(ErrorMessage = "Customer's phonenumber is required")]
        [RegularExpression("^\\d{3}[-]?\\d{3}[- ]?\\d{4}$", ErrorMessage = "Phone number must be in the format xxx-xxx-xxxx")]
        public string? Phone { get; set; }

        public string? ContactLastName { get; set; }

        public string? ContactFirstName { get; set; }


        [EmailAddress(ErrorMessage = "Please enter a valid contact email address")]
        public string? ContactEmail { get; set; }

        public bool IsDeleted { get; set; } = false;

        public List<Invoice>? Invoices { get; set; }
    }
}
