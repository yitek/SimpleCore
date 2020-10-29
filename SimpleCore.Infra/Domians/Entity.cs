using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleCore.Domains
{
    public class Entity
    {
        [Required]
        [Display(Name = "编号")]
        public Guid Id { get; set; }
    }
}
