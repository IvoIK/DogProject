using DogsApp.Core.Contracts;
using DogsApp.Infrastructure.Data.Entities;
using DogsApp.Infrastructure.Data;

namespace DogsApp.Core.Services
{
    public class DogService : IDogService
    {
        private readonly ApplicationDbContext _context;

        public DogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Create(string name, int age, int breedId, string? picture)
        {
            Dog item = new Dog
            {
                Name = name,
                Age = age,
                Breed = _context.Breeds.Find(breedId),  
                Picture = picture
            };

            _context.Dogs.Add(item);
            return _context.SaveChanges() != 0;
        }

        public Dog GetDogById(int dogId)
        {
            return _context.Dogs.Find(dogId);
        }

        public List<Dog> GetDogs()
        {
            List<Dog> dogs = _context.Dogs.ToList();
            return dogs;
        }

        public List<Dog> GetDogs(string searchStringBreed, string searchStringName)
        {
            List<Dog> dogs = _context.Dogs.ToList();
            if (!String.IsNullOrEmpty(searchStringBreed) && !String.IsNullOrEmpty(searchStringName))
            {
                dogs = dogs.Where(x => x.Breed.Name.ToLower().Contains(searchStringBreed.ToLower()) && x.Name.ToLower().Contains(searchStringName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringBreed))
            {
                dogs = dogs.Where(x => x.Breed.Name.ToLower().Contains(searchStringBreed.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringName))
            {
                dogs = dogs.Where(x => x.Name.ToLower().Contains(searchStringName.ToLower())).ToList();
            }
            return dogs;
        }

        public bool RemoveById(int dogId)
        {
            var dog = GetDogById(dogId);
            if (dog == default(Dog))
            {
                return false;
            }
            _context.Remove(dog);
            return _context.SaveChanges() != 0;
        }

        public bool UpdateDog(int dogId, string name, int age, int breedId, string? picture)
        {
            var dog = GetDogById(dogId);
            if (dog == default(Dog))
            {
                return false;
            }
            dog.Name = name;
            dog.Age = age;
            dog.Breed = _context.Breeds.Find(breedId);
            dog.Picture = picture;
            _context.Update(dog);
            return _context.SaveChanges() != 0;
        }
    }
}