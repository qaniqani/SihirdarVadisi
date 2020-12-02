using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sihirdar.Service.Draw.Exception
{
    public class EntityException : BaseException
    {
        public EntityException(string message) : base(message)
        {
        }
    }
}