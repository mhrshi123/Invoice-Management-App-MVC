
namespace InvoiceApp.Tests
{
    public class CustomerControllerTests
    {
        [Fact]
        public void AllCustomers_Get_CustomersViewModel()
        {
            //arrange
            var _mockInvioceManagerService = new Mock<IInvoiceManagerService>();

            var customers = new List<Customer>()
            {
                new (){CustomerId = 1, Name = "Alice", IsDeleted = false},
                new (){CustomerId = 2, Name = "Bob", IsDeleted = false}
            };

            _mockInvioceManagerService
                .Setup(m => m.GetCustomerByCategory('A', 'E'))
                .Returns(customers);

            var controller = new CustomerController(_mockInvioceManagerService.Object);

            //act
            var result = controller.AllCustomers('A', 'E') as ViewResult;

            var customersViewModel = result?.Model as CustomersViewModel;

            //assert
            Assert.Equal(customers, customersViewModel?.Customers);
        }




        // Verify that the view name is "Add"
        [Fact]
        public void AddCustomer_Get_ReturnsAddViewResult()
        {
            //arrange
            var _mockInvoiceManagerService = new Mock<IInvoiceManagerService>();
            var controller = new CustomerController(_mockInvoiceManagerService.Object);

            //act
            var result = controller.AddCustomer() as ViewResult;

            //assert
            Assert.Equal("Add", result.ViewName);

        }


    }
}
