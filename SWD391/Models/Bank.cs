using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SWD391.Models
{
    [Table("tbl_bank")]
    public class Bank
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("bank_name")]
        public string Name { get; set; }
        [Column("loan_rate_six")]
        public double LoanRateSix { get; set; }
        [Column("loan_rate_twelve")]
        public double LoanRateTwelve { get; set; }
        [Column("loan_rate_twenty_four")]
        public double LoanRateTwentyFour { get; set; }
        [Column("saving_rate_six")]
        public double SavingRateSix { get; set; }
        [Column("saving_rate_twelve")]
        public double SavingRateTwelve { get; set; }
        [Column("saving_rate_twenty_four")]
        public double SavingRateTwentyFour { get; set; }
        [Column("icon")]
        public string Icon { get; set; }
        [Column("created_date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }
    }
}
