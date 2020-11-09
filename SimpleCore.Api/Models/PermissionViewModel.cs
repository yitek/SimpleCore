using SimpleCore.Api.Domians;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.Api.Models
{
    /// <summary>
    /// 授权
    /// </summary>
    public class PermissionViewModel
    {
        public IList<Func> Access { get; set; }
    }
}
