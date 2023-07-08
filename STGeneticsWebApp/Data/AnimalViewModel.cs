using System.Diagnostics;

namespace STGeneticsWebApp.Data
{
    [DebuggerDisplay("{Id} - {NameWithDiscount}")]
    public class AnimalViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameWithDiscount { get { return (DiscountApplied ? this.Name + " (5 % Discount Applied)" : this.Name); } }
        public short BreedId { get; set; }
        public string BreedName { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthDateText { get { return (BirthDate.ToString("MMM/dd/yyyy")); } }
        public byte SexId { get; set; }
        public string SexName { get; set; }
        public decimal Price { get; set; }
        public byte StatusId { get; set; }
        public string StatusName { get; set; }
        public bool DiscountApplied { get; set; } = false;

        public Animal ToAnimal()
        {
            return (new Animal
            {
                Id = Id,
                Name = Name,
                BirthDate = BirthDate,
                BreedId = BreedId,
                SexId = SexId,
                Price = Price,
                StatusId = StatusId
            });
        }
    }
}
