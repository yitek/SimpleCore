using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.Models
{
    public class QueryViewModel
    {
        /// <summary>
        /// 分页-页码
        /// </summary>
        public int? PageIndex { get; set; }
        /// <summary>
        /// 分页-每页记录数
        /// </summary>
        public int? PageSize { get; set; }
        /// <summary>
        /// 排序方向
        /// </summary>
        public string SortDir { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public long Total { get; set; }

        internal protected object InternalItems { get; set; }
    }
}
