namespace STGeneticsWebApp.Data
{
    public class AnimalViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short BreedId { get; set; }
        public string BreedName { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirtdDateText { get { return (BirthDate.ToString("MMM/dd/yyyy")); } }
        public byte SexId { get; set; }
        public string SexName { get; set; }
        public decimal Price { get; set; }
        public byte StatusId { get; set; }
        public string StatusName { get; set; }

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
