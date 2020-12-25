﻿namespace ProSeeker.Services.Data.Tests.Specialists
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using ProSeeker.Data;
    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.Specialists;
    using ProSeeker.Services.Mapping;
    using ProSeeker.Web.ViewModels.Users;
    using Xunit;

    public sealed class SpecialistsServiceTests : IDisposable
    {
        private readonly ISpecialistsService service;

        private ApplicationDbContext dbContext;

        private List<Specialist_Details> specialists;

        public SpecialistsServiceTests()
        {
            this.specialists = new List<Specialist_Details>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;
            this.dbContext = new ApplicationDbContext(options);

            var specialistsRepository = new EfDeletableEntityRepository<Specialist_Details>(this.dbContext);
            this.service = new SpecialistsService(specialistsRepository);

            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task ShouldReturnAllSpecialistsPerCategoryProperly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SpecialistShortDetailsViewModel).Assembly);
            var categoryId = 1;

            var allSpecialistsPerCategory =
                await this.service.GetAllSpecialistsPerCategoryAsync<SpecialistShortDetailsViewModel>(
                    categoryId, null, 0, 0);

            var expectedCount = 3;
            var actualCount = allSpecialistsPerCategory.Count();
            var isCategoryTheSame = allSpecialistsPerCategory.All(x => x.JobCategoryId == categoryId);

            Assert.Equal(expectedCount, actualCount);
            Assert.True(isCategoryTheSame);
        }

        [Fact]
        public async Task ShouldReturnCorrectSpecialistsCountByGivenCategoryId()
        {
            var categoryId = 1;
            var expectedCount = 3;

            var actualCount = await this.service.GetSpecialistsCountByCategoryAsync(categoryId, 0);

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task SortByOpinionsCountShouldWorkProperly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SpecialistShortDetailsViewModel).Assembly);

            var categoryId = 1;
            var sortBy = "OpinionsCount";
            var cityId = 0;

            var allSpecialistsPerCategory =
                await this.service.GetAllSpecialistsPerCategoryAsync<SpecialistShortDetailsViewModel>(
                    categoryId, sortBy, cityId, 0);

            var listedSpecialists = allSpecialistsPerCategory.ToList();
            var actualFirstSpecialistId = listedSpecialists[0].Id;
            var actualSecondSpecialist = listedSpecialists[1];
            var expectedFirstSpecialistId = "specialist3";
            var expectedSecondSpecialistId = "specialist2";

            Assert.Equal(expectedFirstSpecialistId, actualFirstSpecialistId);
        }

        public void Dispose()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        private void InitializeRepositoriesData()
        {
            this.specialists.AddRange(new List<Specialist_Details>
            {
                new Specialist_Details
                {
                    Id = "specialist1",
                    UserId = "1",
                    JobCategoryId = 1,
                },
                new Specialist_Details
                {
                    Id = "specialist2",
                    UserId = "2",
                    JobCategoryId = 1,
                    Opinions = new List<Opinion>
                        {
                        new Opinion { Id = 1, Content = "Hey", CreatorId = "2", },
                        },
                },
                new Specialist_Details
                {
                    Id = "specialist3",
                    UserId = "3",
                    JobCategoryId = 1,
                    Opinions = new List<Opinion>
                        {
                        new Opinion { Id = 2, Content = "Hey", CreatorId = "3", },
                        new Opinion { Id = 3, Content = "Hey", CreatorId = "3", },
                        },
                },
            });

            this.dbContext.AddRange(this.specialists);
            this.dbContext.SaveChanges();
        }
    }
}
