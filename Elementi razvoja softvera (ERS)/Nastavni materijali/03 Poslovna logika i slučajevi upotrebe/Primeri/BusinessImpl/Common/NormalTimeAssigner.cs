using Application.Common.Interfaces;
using System;

namespace BusinessImpl.Common
{
    public class NormalTimeAssigner : ITimeAssigner
    {
        public DateTime DateTime { get => DateTime.Now; }
    }
}
