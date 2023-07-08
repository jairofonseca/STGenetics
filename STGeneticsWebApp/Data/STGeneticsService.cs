namespace STGeneticsWebApp.Data
{
    public class STGeneticsService
    {
        STGeneticsDBContext _stGeneticsDBContext;

        public STGeneticsService(STGeneticsDBContext stGeneticsDBContext)
        {
            _stGeneticsDBContext = stGeneticsDBContext;
        }

        public Task<List<AnimalViewModel>> GetAnimalListAsync()
        {
            List<AnimalViewModel> animalList = new List<AnimalViewModel>();

            var animals = (from tbl in _stGeneticsDBContext.Animals
                           join bre in _stGeneticsDBContext.Breeds on tbl.BreedId equals bre.Id
                           join sex in _stGeneticsDBContext.Sexes on tbl.SexId equals sex.Id
                           join sts in _stGeneticsDBContext.Statuses on tbl.StatusId equals sts.Id
                           select new
                           {
                               tbl,
                               bre,
                               sex,
                               sts
                           }).ToList();

            if (animals?.Count > 0)
            {
                animals.ForEach(animal =>
                {
                    animalList.Add(new AnimalViewModel
                    {
                        Id = animal.tbl.Id,
                        Name = animal.tbl.Name,
                        BreedId = animal.tbl.BreedId,
                        BreedName = animal.bre.Name,
                        BirthDate = animal.tbl.BirthDate,
                        Price = animal.tbl.Price,
                        SexId = animal.tbl.SexId,
                        SexName = animal.sex.Name,
                        StatusId = animal.tbl.StatusId,
                        StatusName = animal.sts.Name
                    });
                });
            }

            return (Task.FromResult(animalList));
        }

        public Task<bool> InsertAnimal(Animal animal)
        {
            bool result = false;

            if (animal != null)
            {
                _stGeneticsDBContext.Animals.Add(animal);

                result = _stGeneticsDBContext.SaveChanges() > 0;
            }

            return Task.FromResult(result);
        }

        public Task<bool> UpdateAnimal(Animal animal)
        {
            bool result = false;

            if (animal != null)
            {
                var animalToUpdate = _stGeneticsDBContext.Animals.Find(animal.Id);

                if (animalToUpdate != null)
                {
                    animalToUpdate.Name = animal.Name;
                    animalToUpdate.BreedId = animal.BreedId;
                    animalToUpdate.BirthDate = animal.BirthDate;
                    animalToUpdate.SexId = animal.SexId;
                    animalToUpdate.Price = animal.Price;
                    animalToUpdate.StatusId = animal.StatusId;

                    _stGeneticsDBContext.Animals.Update(animalToUpdate);

                    result = _stGeneticsDBContext.SaveChanges() > 0;
                }
            }

            return Task.FromResult(result);
        }

        public Task<bool> DeleteAnimal(Animal animal)
        {
            bool result = false;

            if (animal != null)
            {
                var animalToDelete = _stGeneticsDBContext.Animals.Find(animal.Id);

                if (animalToDelete != null)
                {
                    _stGeneticsDBContext.Animals.Remove(animalToDelete);

                    result = _stGeneticsDBContext.SaveChanges() > 0;
                }
            }

            return Task.FromResult(result);
        }

        public Task<List<Breed>> GetBreedListAsync()
        {
            List<Breed> breedList = new List<Breed>();

            breedList = _stGeneticsDBContext.Breeds.ToList();

            return (Task.FromResult(breedList));
        }

        public Task<List<Sex>> GetSexListAsync()
        {
            List<Sex> sexList = new List<Sex>();

            sexList = _stGeneticsDBContext.Sexes.ToList();

            return (Task.FromResult(sexList));
        }

        public Task<List<Status>> GetStatusListAsync()
        {
            List<Status> statusList = new List<Status>();

            statusList = _stGeneticsDBContext.Statuses.ToList();

            return (Task.FromResult(statusList));
        }
    }
}
