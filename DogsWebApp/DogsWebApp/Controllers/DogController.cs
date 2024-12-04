using DogsApp.Core.Contracts;
using DogsApp.Infrastructure.Data;
using DogsApp.Infrastructure.Data.Entities;
using DogsWebApp.Models.Dog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogsWebApp.Controllers
{
    public class DogController : Controller
    {
        private readonly IDogService _dogService;

        public DogController(IDogService dogsService)
        {
            this._dogService = dogsService;
        }

        // GET: DogController
        public ActionResult Index(string searchStringBreed, string searchStringName)
        {
            List<DogAllViewModel> dogs = _dogService.GetDogs(searchStringBreed, searchStringName).Select(dogFromDb => new DogAllViewModel
            {

                Id = dogFromDb.Id,
                Name = dogFromDb.Name,
                Age = dogFromDb.Age,
                Breed = dogFromDb.Breed,
                Picture = dogFromDb.Picture,

            }).ToList();

            if (!String.IsNullOrEmpty(searchStringBreed) && !String.IsNullOrEmpty(searchStringName))
            {
                dogs = dogs.Where(x => x.Breed.ToLower().Contains(searchStringBreed.ToLower()) && x.Name.ToLower().Contains(searchStringName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringBreed))
            {
                dogs = dogs.Where(x => x.Breed.ToLower().Contains(searchStringBreed.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringName))
            {
                dogs = dogs.Where(x => x.Name.ToLower().Contains(searchStringName.ToLower())).ToList();
            }

            return this.View(dogs);
        }

        // GET: DogController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DogCreateViewModel blindingModel)
        {
            if (ModelState.IsValid)
            {
                var created = _dogService.Create(blindingModel.Name, blindingModel.Age, blindingModel.Breed, blindingModel.Picture);

                if (created)
                {
                    return this.RedirectToAction("Success");
                }
            }
            return this.View();
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
                Breed = item.Breed,
                Picture = item.Picture
            };

            return View(dog);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DogEditViewModel blindingModel)
        {
            if (ModelState.IsValid)
            {
                var updated = _dogService.UpdateDog(id, blindingModel.Name, blindingModel.Age, blindingModel.Breed, blindingModel.Picture);

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
                Breed = item.Breed,
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

            DogDeleteViewModel dog = new DogDeleteViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                Breed = item.Breed,
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