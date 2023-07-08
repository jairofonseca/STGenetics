using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STGeneticsWebApp.Data
{
    [Table("Status")]
    public class Status
    {
        [Key]
        [Column("Id")]
        public byte Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
    }
}
