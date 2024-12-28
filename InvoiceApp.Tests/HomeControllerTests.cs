

namespace InvoiceApp.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_Get_ReturnsViewResult()
        {
            //arrange
            var controller = new HomeController();

            //act
            var result = controller.Index();

            //assert
            Assert.IsType<ViewResult>(result);

        }




    }
}