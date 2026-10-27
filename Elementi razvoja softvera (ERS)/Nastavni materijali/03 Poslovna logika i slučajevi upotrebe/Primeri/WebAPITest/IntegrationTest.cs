using DBFake.FakeDBCreator;
using Application.Common.Factory;
using WebAPI.Constants;
using WebAPI.Controllers;
using Application.Supplier.Interfaces;
using Application.Common.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using NUnit.Framework.Legacy;

namespace Test.WebAPITest
{
    public class IntegrationTest
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
            var dataMocker = new FakeDBCreator(RepoAbstractFactory.Instance.RepoFactory) { ProductQuantityOnStock = 50, Price = 10f };
            (new FakeDBCreator(RepoAbstractFactory.Instance.RepoFactory) { ProductQuantityOnStock = 50 }).InsertDBData();
            supplierServiceStockMock = new Mock<ISupplierStockService>();
            supplierServiceStockMock.Setup(t => t.AddProduct(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            supplierServiceStockMock.Setup(t => t.WithdrawProduct(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<float>())).Returns(true);
            supplierServiceStockMock.Setup(t => t.UpdateProduct(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<float>())).Returns(true);
        }

        [Test]
        public void IntegrationTest1()
        {
            supplierServiceStockMock.Setup(t => t.GetQuantity(It.IsAny<string>(), It.IsAny<string>())).Returns(10);
            var controller1 = new AddProductToCartController() { SupplierStockService = supplierServiceStockMock.Object };
            var response1 = controller1.Get("1", "1", 20f);
            ClassicAssert.AreEqual(response1.ProductItems.Where(t => t.ProductId == "1").FirstOrDefault().Quantity, 20f);
            ClassicAssert.AreEqual(response1.Message, MessageConstants.ADDING_PRODUCT_TO_SHOPPING_CART_SUCCESFUL);
            response1 = controller1.Get("1", "1", 30f);
            ClassicAssert.AreEqual(response1.ProductItems.Where(t => t.ProductId == "1").FirstOrDefault().Quantity, 50f);
            ClassicAssert.AreEqual(response1.Message, MessageConstants.ADDING_PRODUCT_TO_SHOPPING_CART_SUCCESFUL);
            response1 = controller1.Get("1", "2", 10f);
            ClassicAssert.AreEqual(response1.ProductItems.Where(t => t.ProductId == "2").FirstOrDefault().Quantity, 10f);
            ClassicAssert.AreEqual(response1.Message, MessageConstants.ADDING_PRODUCT_TO_SHOPPING_CART_SUCCESFUL);
            var controller2 = new ListCartContentController();
            var response2 = controller2.Get("1");
            ClassicAssert.AreEqual(response2.ProductItems.Count, 2);
            float totalPrice = 0f;
            response2.ProductItems.ForEach(t => totalPrice += t.Quantity * t.UnitPrice);
            ClassicAssert.AreEqual(totalPrice, 600);
            Mock<ITimeAssigner> timeAssignerMock = new Mock<ITimeAssigner>();
            timeAssignerMock.Setup(t => t.DateTime).Returns(new DateTime(2022, 10, 1, 16, 30, 0));
            var controller3 = new CheckoutOrderController
            {
                TimeAssigner = timeAssignerMock.Object
            };
            var result = controller3.Get("1", "Novi Sad", "Jiricekova", "2", "+3816436520");
            ClassicAssert.AreEqual(result.TotalAmount, 420f);            
            result = controller3.Get("1", "Novi Sad", "Jiricekova", "2", "+3816436526");
            ClassicAssert.AreEqual(result.TotalAmount, 0f);
        }
    }
}
