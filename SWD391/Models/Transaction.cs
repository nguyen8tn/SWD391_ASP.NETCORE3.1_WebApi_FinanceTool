using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SWD391.Models
{
    public class Transaction
    {
        [Table("tbl_user_saving")]
        public class Account
        {
            [Key]
            [Column("id")]
            [JsonProperty(PropertyName = "ID")]
            public int ID { get; set; }

            [JsonProperty(PropertyName = "BankID")]
            [Column("bank_id")]
            public int BankID { get; set; }

            [JsonProperty(PropertyName = "UID")]
            [ForeignKey("User")]
            [Column("user_id")]
            public string UID { get; set; }

            [JsonProperty(PropertyName = "Name")]
            [Column("name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "Amount")]
            [Column("amount")]
            public double Amount { get; set; }

            [JsonProperty(PropertyName = "StartDate")]
            [Column("start_date")]
            public DateTime? StartDate { get; set; }

            [JsonProperty(PropertyName = "Term")]
            [Column("term")]
            public int Term { get; set; }

            [JsonProperty(PropertyName = "InterestRate")]
            [Column("interest_rate")]
            public double InterestRate { get; set; }

            [JsonProperty(PropertyName = "FreeInterestRate")]
            [Column("free_interest_rate")]
            public double FreeInterestRate { get; set; }

            [Column("calculation_day")]
            [JsonProperty(PropertyName = "CalculationDay")]
            public int CalculationDay { get; set; }

            [JsonProperty(PropertyName = "CreatedDate")]
            [Column("created_date")]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
            public DateTime? CreatedDate { get; set; }
            [JsonProperty(PropertyName = "Type")]
            public int Type { get; set; }

            [System.Text.Json.Serialization.JsonIgnore]
            public virtual User User { get; set; }
            [System.Text.Json.Serialization.JsonIgnore]
            public virtual Bank Bank { get; set; }
        }

        [Table("tbl_user_loan")]
        public class LoanAccount
        {
            [Key]
            [Column("id")]
            public int ID { get; set; }
        }

        public class AccountTransaction
        {

        }
    }
}
