using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.Domains
{
    public class RecordEntity:Entity
    {
        [Required]
        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        [Required]
        [Display(Name = "创建者Id")]
        public Guid CreateByUserId { get; set; }

        [Required]
        [Display(Name = "创建者用户名")]
        public string CreateByUserName { get; set; }

        [Required]
        [Display(Name = "更新时间")]
        public DateTime UpdateTime { get; set; }

        [Required]
        [Display(Name = "更新者用户Id")]
        public Guid UpdateByUserId { get; set; }

        [Required]
        [Display(Name = "更新者用户名")]
        public string UpdateByUserName { get; set; }
    }
}
