using SimpleCore.Api.Domians;
using SimpleCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.Api.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ArticleQueryViewModel : QueryViewModel<Article>
    {
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
    }
}
