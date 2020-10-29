using SimpleCore.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.Web.Domains
{
    /// <summary>
    /// 
    /// </summary>
    public class Account : RecordEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [StringLength(200,ErrorMessage ="最多50个字符")]
        [Display(Name = "用户名")]
        public string Username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [StringLength(200, ErrorMessage = "最多200个字符")]
        [Display(Name = "密码")]
        public string Password { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        public string Mobile { get; set; }
    }
}
