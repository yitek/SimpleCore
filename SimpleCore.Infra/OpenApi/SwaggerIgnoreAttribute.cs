using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.OpenApi
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerIgnoreAttribute:Attribute
    {
    }
}
