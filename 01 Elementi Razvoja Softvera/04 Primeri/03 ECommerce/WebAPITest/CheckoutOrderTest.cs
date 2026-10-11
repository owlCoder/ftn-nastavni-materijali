using DBFake.FakeDBCreator;
using Application.Common.Factory;
using WebAPI.Controllers;
using Application.Common.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using NUnit.Framework.Legacy;

namespace Test.WebAPITest
{
    public class CheckoutOrderTest
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DBInitializer.InitializeDB();
        }

        [Test]
        public void TestScheckoutOrderWithLowerDiscount()
        {
            var dataMocker = new FakeDBCreator(RepoAbstractFactory.Instance.RepoFactory) { Price = 10f, ShoppingCartQuantity = 2f, PhoneNumber = "+38164123657" };
            dataMocker.InsertDBData();
            dataMocker.InsertShoppingCarts();
            Mock<ITimeAssigner> timeAssignerMock = new Mock<ITimeAssigner>();
            timeAssignerMock.Setup(t => t.DateTime).Returns(new DateTime(2022, 10, 1, 10, 0, 0));
            var controller = new CheckoutOrderController
            {
                TimeAssigner = timeAssignerMock.Object
            };
            var result = controller.Get("1");
            ClassicAssert.AreEqual(result.TotalAmount, 60f);
            timeAssignerMock.Setup(t => t.DateTime).Returns(new DateTime(2022, 10, 1, 16, 30, 0));
            result = controller.Get("2");
            ClassicAssert.AreEqual(result.TotalAmount, 54f);
        }

        [Test]
        public void TestScheckoutOrderWithHigherDiscount()
        {
            var dataMocker = new FakeDBCreator(RepoAbstractFactory.Instance.RepoFactory) { Price = 10f, ShoppingCartQuantity = 2f, PhoneNumber = "+38164123658" };
            dataMocker.InsertDBData();
            dataMocker.InsertShoppingCarts();
            Mock<ITimeAssigner> timeAssignerMock = new Mock<ITimeAssigner>();
            timeAssignerMock.Setup(t => t.DateTime).Returns(new DateTime(2022, 10, 1, 10, 0, 0));
            var controller = new CheckoutOrderController
            {
                TimeAssigner = timeAssignerMock.Object
            };
            var result = controller.Get("1");
            ClassicAssert.AreEqual(result.TotalAmount, 60f);
            timeAssignerMock.Setup(t => t.DateTime).Returns(new DateTime(2022, 10, 1, 16, 30, 0));
            result = controller.Get("2");
            ClassicAssert.AreEqual(result.TotalAmount, 48f);
        }

        [Test]
        public void TestScheckoutOrderWithAllDiscounts()
        {
            var dataMocker = new FakeDBCreator(RepoAbstractFactory.Instance.RepoFactory) { Price = 10f, ShoppingCartQuantity = 2f };
            dataMocker.InsertDBData();
            dataMocker.InsertShoppingCarts();
            Mock<ITimeAssigner> timeAssignerMock = new Mock<ITimeAssigner>();
            var controller = new CheckoutOrderController
            {
                TimeAssigner = timeAssignerMock.Object
            };
            timeAssignerMock.Setup(t => t.DateTime).Returns(new DateTime(2022, 10, 1, 16, 30, 0));
            var result = controller.Get("1", "Novi Sad", "Jiricekova", "2", "+3816436526");
            ClassicAssert.AreEqual(result.TotalAmount, 48f);
            result = controller.Get("2", "Novi Sad", "Jiricekova", "2", "+3816436527");
            ClassicAssert.AreEqual(result.TotalAmount, 54f);
            result = controller.Get("3", "Novi Sad", "Jiricekova", "2", "+3816436520");
            ClassicAssert.AreEqual(result.TotalAmount, 42f);
        }


    }
}
