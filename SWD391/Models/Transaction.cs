using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SWD391.Models
{
    public class Transaction
    {
        [Table("tbl_user_saving")]
        public class UserSaving
        {
            [Key]
            [Column("id")]
            [JsonProperty(PropertyName = "id")]
            public int ID { get; set; }

            [JsonProperty(PropertyName = "bank_id")]
            [Column("bank_id")]
            public int BankID { get; set; }

            [JsonProperty(PropertyName = "user_id")]
            [Column("user_id")]
            public string UID { get; set; }

            [JsonProperty(PropertyName = "name")]
            [Column("name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "amount")]
            [Column("amount")]
            public double Amount { get; set; }

            [JsonProperty(PropertyName = "start_date")]
            [Column("start_date")]
            public DateTime? StartDate { get; set; }

            [JsonProperty(PropertyName = "term")]
            [Column("term")]
            public int Term { get; set; }

            [JsonProperty(PropertyName = "interest_rate")]
            [Column("interest_rate")]
            public double InterestRate { get; set; }

            [JsonProperty(PropertyName = "free_interest_rate")]
            [Column("free_interest_rate")]
            public double FreeInterestRate { get; set; }

            [Column("calculation_day")]
            [JsonProperty(PropertyName = "calculation_day")]
            public int CalculationDay { get; set; }

            [JsonProperty(PropertyName = "created_date")]
            [Column("created_date")]
            public DateTime? CreatedDate { get; set; }
        }
    }
}
