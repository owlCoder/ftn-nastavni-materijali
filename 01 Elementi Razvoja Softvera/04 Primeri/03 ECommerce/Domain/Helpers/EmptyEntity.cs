using System;

namespace Domain.Helpers
{
    public class EmptyEntity : Entity
    {
        public override object GetId()
        {
            return new Guid();
        }
    }
}
