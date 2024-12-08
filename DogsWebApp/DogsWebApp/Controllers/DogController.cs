using DogsApp.Core.Contracts;
using DogsApp.Infrastructure.Data;
using DogsApp.Infrastructure.Data.Entities;
using DogsWebApp.Models.Dog;
using DogsWebApp.Models.Breed;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogsWebApp.Controllers
{
    public class DogController : Controller
    {
        private readonly IDogService _dogService;
        private readonly IBreedService _breedService;

        public DogController(IDogService dogsService, IBreedService breedService)
        {
            this._dogService = dogsService;
            this._breedService = breedService;
        }

        // GET: DogController
        public ActionResult Index(string searchStringBreed, string searchStringName)
        {
            List<DogAllViewModel> dogs = _dogService.GetDogs(searchStringBreed, searchStringName).Select(item => new DogAllViewModel
            {

                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                BreedName = item.Breed.Name,
                Picture = item.Picture,

            }).ToList();

            if (!String.IsNullOrEmpty(searchStringBreed) && !String.IsNullOrEmpty(searchStringName))
            {
                dogs = dogs.Where(x => x.BreedName.ToLower().Contains(searchStringBreed.ToLower()) && x.Name.ToLower().Contains(searchStringName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringBreed))
            {
                dogs = dogs.Where(x => x.BreedName.ToLower().Contains(searchStringBreed.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringName))
            {
                dogs = dogs.Where(x => x.Name.ToLower().Contains(searchStringName.ToLower())).ToList();
            }

            return View(dogs);
        }

        // GET: DogController/Create
        public ActionResult Create()
        {
            var dog = new DogCreateViewModel();
            dog.Breeds = _breedService.GetAllBreeds().Select(x => new BreedPairViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToList();
            return View(dog);
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] DogCreateViewModel dog)
        {
            if (ModelState.IsValid)
            {
                var createdId = _dogService.Create(dog.Name, dog.Age, dog.BreedId, dog.Picture);

                if (createdId)
                {
                    return this.RedirectToAction("Success");
                }
            }
            return View();
        }

        public IActionResult Success()
        {
            return this.View();
        }

        // GET: DogController/Edit/5
        public ActionResult Edit(int id)
        {
            Dog item = _dogService.GetDogById(id);

            if (item == null)
            {
                return NotFound();
            }

            DogEditViewModel dog = new DogEditViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                BreedId = item.BreedId,
                Picture = item.Picture
            };

            dog.Breeds = _breedService.GetAllBreeds().Select(x => new BreedPairViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            })
             .ToList();

            return View(dog);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DogEditViewModel blindingModel)
        {
            if (ModelState.IsValid)
            {
                var updated = _dogService.UpdateDog(id, blindingModel.Name, blindingModel.Age, blindingModel.BreedId, blindingModel.Picture);

                if (updated)
                {
                    return this.RedirectToAction("Index");
                }
            }
            return View(blindingModel);
        }

        // GET: DogController/Details/5
        public ActionResult Details(int id)
        {
            Dog item = _dogService.GetDogById(id);

            if (item == null)
            {
                return NotFound();
            }

            DogDetailsViewModel dog = new DogDetailsViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                BreedName = item.Breed.Name,
                Picture = item.Picture
            };
            return View(dog);
        }

        // GET: DogController/Delete/5
        public ActionResult Delete(int id)
        {
            Dog item = _dogService.GetDogById(id);

            if (item == null)
            {
                return NotFound();
            }

            DogDetailsViewModel dog = new DogDetailsViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                BreedName = item.Breed.Name,
                Picture = item.Picture
            };
            return View(dog);
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var deleted = _dogService.RemoveById(id);

            if (deleted)
            {
                return this.RedirectToAction("Index", "Dog");
            }
            else
            {
                return View();
            }
        }
    }
}