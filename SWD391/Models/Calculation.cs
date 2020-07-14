using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Index = Microsoft.EntityFrameworkCore.Metadata.Internal.Index;

namespace SWD391.Models
{
    public class Calculation
    {
        [Table("tbl_operand")]
        public class Operand
        {
            [Key]
            [JsonProperty(PropertyName = "id")]
            public int ID { get; set; }
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "value")]
            public double Value { get; set; }
            [JsonProperty(PropertyName = "type")]
            public int Type { get; set; }
            [Column("description")]
            [JsonProperty(PropertyName = "description")]
            public string Desc { get; set; }
            [JsonProperty(PropertyName = "sequence")]
            public int Sequence { get; set; }
            [Column("base_formula_id")]
            [JsonProperty(PropertyName = "base_formula_id")]
            public int BaseFormulaID { get; set; }

            [System.Text.Json.Serialization.JsonIgnore]
            public virtual BaseFormula BaseFormula { get; set; }

            [System.Text.Json.Serialization.JsonIgnore]
            public virtual ICollection<SubFormula> SubFormulas { get; set; }

            //[System.Text.Json.Serialization.JsonIgnore]
            //public virtual  ICollection<> { get; set; }


        }
        [Table("tbl_sub_formula")]
        public class SubFormula
        {
            [Key]
            public int ID { get; set; }
            [Column("expression")]
            [JsonProperty(PropertyName = "expression")]
            public string Expression { get; set; }
            [Column("operand_id")]
            [JsonProperty(PropertyName = "operand_id")]
            public int OperandID { get; set; }

            [System.Text.Json.Serialization.JsonIgnore]
            public virtual Operand Operand { get; set; }

        }

        [Table("tbl_base_formula")]
        public class BaseFormula
        {
            [Key]
            [Column("id")]
            [JsonProperty(PropertyName = "id")]
            public int ID { get; set; }

            [Column("name")]
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

            [Column("expression")]
            [JsonProperty(PropertyName = "expression")]
            public string Expression { get; set; }

            [System.Text.Json.Serialization.JsonIgnore]
            public virtual ICollection<Operand> Operands { get; set; }
        }

        [Table("tbl_group_value")]
        public class GroupValue
        {
            [Key]
            public int ID { get; set; }
            [Column("expression")]
            [JsonProperty(PropertyName = "expression")]
            public string Expression { get; set; }
        }
        [Table("tbl_value")]
        public class Value
        {
            [Key]
            public int ID { get; set; }
        }
        public class Explanation
        {
            public int ID { get; set; }
            public int FormulaID { get; set; }
            [System.Text.Json.Serialization.JsonIgnore]
            public virtual SubFormula Formula { get; set; }

        }
        public enum OperandTypeValue : int
        {
            INPUT = 1,
            EXPRESSION = 2,
            GROUP_VALUE = 3,
            STATIC = 4
        }
    }
}
