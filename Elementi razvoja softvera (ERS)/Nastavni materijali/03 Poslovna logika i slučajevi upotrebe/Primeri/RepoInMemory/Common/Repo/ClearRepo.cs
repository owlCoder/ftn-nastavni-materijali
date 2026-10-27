using Application.Common.Repo;
using RepoInMemory.Common.DB;

namespace RepoInMemory.Common.Repo
{
    public class ClearRepo : IClearRepo
    {
        public void ClearAll()
        {
            InMemoryDatabase.Instance.Clear();
        }
    }
}
