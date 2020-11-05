using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.Api.Domians
{
    /// <summary>
    /// 功能类型
    /// </summary>
    public enum FuncTypes
    {
        /// <summary>
        /// API 只能通过ajax调用
        /// </summary>
        Api,
        /// <summary>
        /// 页面，其实是控制前端
        /// </summary>
        Page,
        /// <summary>
        /// 页面，且是主菜单项
        /// </summary>
        MainMenu,
        /// <summary>
        /// 页面，且是顶菜单
        /// </summary>
        TopMenu
    }
}
