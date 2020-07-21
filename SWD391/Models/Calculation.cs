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
            public string Expression { get; set; }
            [Column("operand_id")]
            [JsonProperty(PropertyName = "operand_id")]
            public int OperandID { get; set; }
            //-----------------------------------------------------
            [System.Text.Json.Serialization.JsonIgnore]
            public virtual BaseFormula BaseFormula { get; set; }
            [System.Text.Json.Serialization.JsonIgnore]
            public virtual ICollection<GroupValue> GroupValues { get; set; }
            [System.Text.Json.Serialization.JsonIgnore]
            public virtual Operand Parent { get; set; }
            [System.Text.Json.Serialization.JsonIgnore]
            public virtual ICollection<Operand> Childs { get; set; }

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
            [JsonProperty(PropertyName = "id")]
            public int ID { get; set; }

            [JsonProperty(PropertyName = "max")]
            public double Max { get; set; }
            [JsonProperty(PropertyName = "value")]
            public string Value { get; set; }
            [Column("operand_id")]
            [JsonProperty(PropertyName = "operand_id")]
            public int OperandID { get; set; }
            [System.Text.Json.Serialization.JsonIgnore]
            public virtual Operand Operand { get; set; }
        }
        public class Explanation
        {
            public int ID { get; set; }
            public int FormulaID { get; set; }

        }
        public class FormulaRequest
        {
            public BaseFormula baseFormula { get; set; }
            public List<Operand> Operands { get; set; }
            public List<GroupValue> GroupValues { get; set; }
            public List<int> testNumber { get; set; }
        }
        
    }
}
