using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace STGeneticsWebApp.Data
{
    [Table("Animal")]
    public class Animal
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("BreedId")]
        public short BreedId { get; set; }
        [Column("BirthDate")]
        public DateTime BirthDate { get; set; }
        [Column("SexId")]
        public byte SexId { get; set; }
        [Column("Price")]
        public decimal Price { get; set; }
        [Column("StatusId")]
        public byte StatusId { get; set; }
    }
}
