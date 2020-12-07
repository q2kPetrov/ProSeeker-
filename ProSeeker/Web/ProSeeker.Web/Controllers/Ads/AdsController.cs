﻿namespace ProSeeker.Web.Controllers.Ads
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ProSeeker.Common;
    using ProSeeker.Data.Models;
    using ProSeeker.Services.Data.Ads;
    using ProSeeker.Services.Data.CategoriesService;
    using ProSeeker.Services.Data.Cities;
    using ProSeeker.Web.ViewModels.Ads;
    using ProSeeker.Web.ViewModels.Categories;
    using ProSeeker.Web.ViewModels.Cities;

    public class AdsController : BaseController
    {
        private readonly IAdsService adsService;
        private readonly ICategoriesService categoriesService;
        private readonly ICitiesService citiesService;
        private readonly UserManager<ApplicationUser> userManager;

        public AdsController(
            IAdsService adsService,
            ICategoriesService categoriesService,
            ICitiesService citiesService,
            UserManager<ApplicationUser> userManager)
        {
            this.adsService = adsService;
            this.categoriesService = categoriesService;
            this.citiesService = citiesService;
            this.userManager = userManager;
        }

        // [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            var allCities = this.citiesService.GetAllCities<CitySimpleViewModel>();
            var allcategories = this.categoriesService.GetAllCategories<CategorySimpleViewModel>();

            var createModel = new CreateAdInputModel
            {
                Categories = allcategories,
                Cities = allCities,
            };

            return this.View(createModel);
        }

        // [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAdInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var createModel = new CreateAdInputModel
                {
                    Categories = this.categoriesService.GetAllCategories<CategorySimpleViewModel>(),
                    Cities = this.citiesService.GetAllCities<CitySimpleViewModel>(),
                };

                return this.View(createModel);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var newAdId = await this.adsService.CreateAsync(input, user.Id);

            this.TempData["Message"] = "Успешно създадохте нова обява!";
            return this.RedirectToAction(nameof(this.MyAds));

            // return this.RedirectToAction("GetDetails", new { id = id });
            // TODO: USE SANITIZER WHEN SHOWING AD DETAILS !!!!
        }

        // [Authorize]
        public IActionResult GetByCategory(string categoryName, int page = 1)
        {
            // Explicitly check the page in case someone wants to cheat :)
            page = page < 1 ? 1 : page;

            var model = new GetAllViewModel
            {
                CategoryName = categoryName,
                PageNumber = page,
                AdsCount = this.adsService.AllAdsByCategoryCount(categoryName),
                Ads = this.adsService.GetByCategory<AdsShortDetailsViewModel>(categoryName, page),
            };

            return this.View(model);
        }

        //[Authorize]
        // Is in role RegularUser
        public async Task<IActionResult> MyAds(int page = 1)
        {
            page = page < 1 ? 1 : page;

            var user = await this.userManager.GetUserAsync(this.User);
            var adsPerPage = this.adsService.GetMyAds<AdsShortDetailsViewModel>(user.Id, page);
            var allMyAdsCount = this.adsService.GetAdsCountByUserId(user.Id);

            var model = new GetAllViewModel
            {
                Ads = adsPerPage,
                AdsCount = allMyAdsCount,
                PageNumber = page,
            };

            return this.View(model);
        }

        // [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            await this.adsService.DeleteById(id);
            return this.RedirectToAction(nameof(this.MyAds));
        }

        // [Authorize]
        public IActionResult Edit(string id)
        {
            var model = this.adsService.GetAdDetailsById<UpdateInputModel>(id);
            var allCities = this.citiesService.GetAllCities<CitySimpleViewModel>();
            var allcategories = this.categoriesService.GetAllCategories<CategorySimpleViewModel>();

            model.Cities = allCities;
            model.Categories = allcategories;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                inputModel.Cities = this.citiesService.GetAllCities<CitySimpleViewModel>();
                inputModel.Categories = this.categoriesService.GetAllCategories<CategorySimpleViewModel>();
                return this.View(inputModel);
            }

            await this.adsService.UpdateAdAsync(inputModel);

            // TODO Find a way to show this meesage only after the ad's been adjusted
            //this.TempData["Message"] = "Успешно коригирахте своята обява!";
            return this.RedirectToAction(nameof(this.MyAds));
        }

        public IActionResult GetById(string id)
        {
            var ad = this.adsService.GetAdDetailsById<AdsFullDetailsViewModel>(id);
            return this.View(ad);
        }
    }
}
