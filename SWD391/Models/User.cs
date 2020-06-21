using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SWD391.Models
{
    [Table("tbl_user")]
    public class User
    {
        [Key]
        [Column("id")]
        public string Uid { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("createdDate")]
        public DateTime? CreatedDate { get; set; }
    }
}
