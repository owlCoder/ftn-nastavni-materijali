using DBFake.FakeDBCreator;
using Application.Common.Factory;
using WebAPI.Controllers;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Test.WebAPITest
{
    public class ListCartContentTest
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DBInitializer.InitializeDB();
        }

        [Test]
        public void TestTotalPrice()
        {
            var dataMocker = new FakeDBCreator(RepoAbstractFactory.Instance.RepoFactory) { Price = 10f, ShoppingCartQuantity = 2f };
            dataMocker.InsertDBData();
            dataMocker.InsertShoppingCarts();
            var controller = new ListCartContentController();
            var result = controller.Get("1");
            ClassicAssert.AreEqual(result.ProductItems.Count, 3);
            float totalPrice = 0f;
            result.ProductItems.ForEach(t => totalPrice += t.Quantity * t.UnitPrice);
            ClassicAssert.AreEqual(totalPrice, 60);
        }
    }
}
