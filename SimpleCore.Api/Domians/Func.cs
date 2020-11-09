using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.Api.Domians
{
    /// <summary>
    /// 系统功能
    /// </summary>
    public class Func
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 功能名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图标式样 
        /// </summary>
        public string Icon { get; set; }

        public string Image { get; set; }

        
        /// <summary>
        /// 功能描述
        /// </summary>

        public string Description { get; set; }

        /// <summary>
        /// 树形结构编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 功能类型 ，参见FuncTypes
        /// </summary>
        public FuncTypes Type { get; set; }

        /// <summary>
        /// 功能会被菜单/页面组织成一棵树结构
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// 该项功能是否会随着父级菜单被选中而选中
        /// </summary>
        public int isRelativeWithParent { get; set; }
    }
}
