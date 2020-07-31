using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SWD391.Models
{
    [Table("tbl_Rate")]
    public class Rate
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [Column("currency_type")]
        public int CurrencyType { get; set; }
        public double Value { get; set; }
        [Column("bank_id")]
        public int BankID { get; set; }
        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual Bank Bank { get; set; }
    }
}
