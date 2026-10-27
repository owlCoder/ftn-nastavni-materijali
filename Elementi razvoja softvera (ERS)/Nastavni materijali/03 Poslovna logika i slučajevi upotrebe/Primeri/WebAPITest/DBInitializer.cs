using RepoInMemory.Common.Factory;
using DBFake.FakeDBCreator;
using Application.Common.Factory;

namespace Test.WebAPITest
{
    public class DBInitializer
    {
        public static void InitializeDB()
        {
            var repoFactory = new RepoFactory();
            RepoAbstractFactory.Instance.RepoFactory = repoFactory;
            new FakeDBCreator(repoFactory).InsertDBData();
        }
    }
}
