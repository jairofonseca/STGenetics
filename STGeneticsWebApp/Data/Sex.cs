using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STGeneticsWebApp.Data
{
    [Table("Sex")]
    public class Sex
    {
        [Key]
        [Column("Id")]
        public byte Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
    }
}
