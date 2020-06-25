using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SWD391.Models
{
    [Table("tbl_tax")]
    public class Tax
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
    }
}
