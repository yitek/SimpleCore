using SimpleCore.Domains;
using SimpleCore.Models;
using SimpleCore.Api.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.Api.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountQueryViewModel: QueryViewModel<Account>
    {
        /// <summary>
        /// 用户名/电子邮件/手机号
        /// </summary>
        
        public string Keyword { get; set; }
        /// <summary>
        /// 账号创建时间(起始时间)
        /// </summary>
        public DateTime? CreateTime_Min { get; set; }

        /// <summary>
        /// 账号创建时间(结束时间)
        /// </summary>
        public DateTime? CreateTime_Max { get; set; }
    }
}
