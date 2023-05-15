using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3G.DGSN.DOMAIN
{
    public class Session
    {
        [Key]
        public string state { get; set; }
    }
}
