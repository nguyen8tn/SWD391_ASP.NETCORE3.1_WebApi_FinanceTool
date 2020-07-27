using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static SWD391.Models.Transaction;

namespace SWD391.Models
{
    [Table("tbl_user")]
    public class User
    {
        [Key]
        [Column("id")]
        [JsonProperty(PropertyName = "id")]
        public string Uid { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [Column("createdDate")]
        [JsonProperty(PropertyName = "createdDate")]
        public DateTime? CreatedDate { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
