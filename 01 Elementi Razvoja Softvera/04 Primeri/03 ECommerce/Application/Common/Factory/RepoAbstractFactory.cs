using System;

namespace Application.Common.Factory
{
    public class RepoAbstractFactory
    {
        private static readonly Lazy<RepoAbstractFactory> instance = new Lazy<RepoAbstractFactory>(() => new RepoAbstractFactory());
        public static RepoAbstractFactory Instance { get { return instance.Value; } }
        public IRepoFactory RepoFactory { get; set; }
        private RepoAbstractFactory() { }
    }
}
