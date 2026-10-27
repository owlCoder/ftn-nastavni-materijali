using Application.Common.Factory;
using Microsoft.AspNetCore.Mvc;
namespace WebAPI.Controllers
{
    public abstract class BaseController: ControllerBase
    {
        protected IRepoFactory repoFactory;

        public BaseController(): base()
        {
            this.repoFactory = RepoAbstractFactory.Instance.RepoFactory;
        }
    }
}
