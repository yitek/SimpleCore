using SimpleCore.OpenApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.Models
{
    public class QueryViewModel<TItem> : QueryViewModel
    {
        IList<TItem> _Items;
        /// <summary>
        /// 结果项
        /// </summary>
        [SwaggerIgnore]
        public IList<TItem> Items { 
            get {
                if(this._Items==null) this._Items = this.InternalItems as IList<TItem>;
                return this._Items;
            }
        }
    }
}
