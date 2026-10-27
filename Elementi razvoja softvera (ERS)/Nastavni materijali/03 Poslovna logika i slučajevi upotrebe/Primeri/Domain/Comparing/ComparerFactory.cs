using ObjectsComparer;
using System;
using System.Collections.Generic;

namespace Domain.Comparing
{
    public class ComparerFactory
    {
        private static readonly Lazy<ComparerFactory> instance = new Lazy<ComparerFactory>(() => new ComparerFactory());
        public static ComparerFactory Instance { get { return instance.Value; } }
        private ComparerFactory() {
            comparerDict = new Dictionary<Type, IBaseComparer>();
        }

        private readonly Dictionary<System.Type, IBaseComparer> comparerDict;

        public ObjectsComparer.Comparer<T> GetComparer<T>() where T: Entity
        {
            var atype = typeof(T);
            if (!comparerDict.TryGetValue(atype, out IBaseComparer aComparer))
            {
                aComparer = new ObjectsComparer.Comparer<T>();
                (aComparer as ObjectsComparer.Comparer<T>).IgnoreMember("ProductStocks");
                comparerDict.Add(atype, aComparer);
            }
            return aComparer as ObjectsComparer.Comparer<T>;

        }
    }
}
