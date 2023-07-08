using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STGeneticsWebApp.Data
{
    [Table("Breed")]
    public class Breed
    {
        [Key]
        [Column("Id")]
        public short Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
    }
}
