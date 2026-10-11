using DBFake.FakeDBCreator;
using Application.Common.Factory;
using WebAPI.Constants;
using WebAPI.Controllers;
using WebAPI.WebAPIModel;
using Application.Supplier.Interfaces;
using Moq;
using NUnit.Framework;
using System.Linq;
using NUnit.Framework.Legacy;

namespace Test.WebAPITest
{
    public class AddProductToCartTest
    {
        private Mock<ISupplierStockService> supplierServiceStockMock;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DBInitializer.InitializeDB();
        }

        [SetUp]
        public void Setup()
        {
            (new FakeDBCreator(RepoAbstractFactory.Instance.RepoFactory) { ProductQuantityOnStock = 50 }).InsertDBData(); // Initial quantity of all products on the local stock stock is 50
            supplierServiceStockMock = new Mock<ISupplierStockService>();
            supplierServiceStockMock.Setup(t => t.AddProduct(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            supplierServiceStockMock.Setup(t => t.WithdrawProduct(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<float>())).Returns(true);
            supplierServiceStockMock.Setup(t => t.UpdateProduct(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<float>())).Returns(true);
        }

        [Test]
        public void TestShoppingCartAndStock()
        {
            supplierServiceStockMock.Setup(t => t.GetQuantity(It.IsAny<string>(), It.IsAny<string>())).Returns(25); // set suppliers stock to 25
            var controller = new AddProductToCartController() { SupplierStockService = supplierServiceStockMock.Object };
            controller.InitializeApp();
            ProductItemsResult response;
            response = controller.Get("1", "1", 20f); // Withdraw 20 (of 50) products from local stock and add to cart. There is 30 products left on the local stock
            ClassicAssert.AreEqual(response.ProductItems.Where(t => t.ProductId == "1").FirstOrDefault().Quantity, 20f);
            ClassicAssert.AreEqual(response.Message, MessageConstants.ADDING_PRODUCT_TO_SHOPPING_CART_SUCCESFUL);
            response = controller.Get("1", "1", 35f); // Withdraw all 30 stock from local stock and 5 from the supplier stock. There is 0 products left on the local stock   
            ClassicAssert.AreEqual(response.ProductItems.Where(t => t.ProductId == "1").FirstOrDefault().Quantity, 55f);
            ClassicAssert.AreEqual(response.Message, MessageConstants.ADDING_PRODUCT_TO_SHOPPING_CART_SUCCESFUL);
            supplierServiceStockMock.Setup(t => t.GetQuantity(It.IsAny<string>(), It.IsAny<string>())).Returns(20); // set suppliers stock to 20
            response = controller.Get("1", "1", 30f); // the local stock is empty. Trying to get all 30 products from supplier's stock but there is only 20.
            ClassicAssert.AreEqual(response.ProductItems.Where(t => t.ProductId == "1").FirstOrDefault().Quantity, 55f);
            ClassicAssert.AreNotEqual(response.Message, MessageConstants.ADDING_PRODUCT_TO_SHOPPING_CART_SUCCESFUL);
            response = controller.Get("1", "1", 20f); // the local stock is empty.Trying and success to get all 20 products.
            supplierServiceStockMock.Setup(t => t.GetQuantity(It.IsAny<string>(), It.IsAny<string>())).Returns(0); // set suppliers stock to 20
            ClassicAssert.AreEqual(response.ProductItems.Where(t => t.ProductId == "1").FirstOrDefault().Quantity, 75f);
            ClassicAssert.AreEqual(response.Message, MessageConstants.ADDING_PRODUCT_TO_SHOPPING_CART_SUCCESFUL);
            response = controller.Get("1", "1", 1f); // trying to get only 1 product either from local or supplier stock but both are empty.
            ClassicAssert.AreEqual(response.ProductItems.Where(t => t.ProductId == "1").FirstOrDefault().Quantity, 75f);
            ClassicAssert.AreNotEqual(response.Message, MessageConstants.ADDING_PRODUCT_TO_SHOPPING_CART_SUCCESFUL);
        }
    }
}