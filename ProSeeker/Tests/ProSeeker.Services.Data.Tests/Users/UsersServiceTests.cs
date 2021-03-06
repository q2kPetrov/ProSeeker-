﻿namespace ProSeeker.Services.Data.Tests.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ProSeeker.Data.Models;
    using ProSeeker.Data.Repositories;
    using ProSeeker.Services.Data.UsersService;
    using Xunit;

    public sealed class UsersServiceTests : BaseServiceTests
    {
        private readonly IUsersService service;

        private List<ApplicationUser> users;

        public UsersServiceTests()
        {
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(this.DbContext);
            var citiesRepository = new EfRepository<City>(this.DbContext);
            var specialistsRepository = new EfDeletableEntityRepository<Specialist_Details>(this.DbContext);
            var categoriesRepository = new EfDeletableEntityRepository<JobCategory>(this.DbContext);
            var servicesRepository = new EfDeletableEntityRepository<Service>(this.DbContext);
            var opinionsRepository = new EfDeletableEntityRepository<Opinion>(this.DbContext);
            var ratingsRepository = new EfRepository<Rating>(this.DbContext);
            this.service = new UsersService(
                usersRepository,
                citiesRepository,
                specialistsRepository,
                categoriesRepository,
                servicesRepository,
                opinionsRepository,
                ratingsRepository);

            this.users = new List<ApplicationUser>();
            this.InitializeRepositoriesData();
        }

        [Fact]
        public async Task GetUserFirstNameByIdAsync_ShouldReturnCorrectFirstNameByGivenUserId()
        {
            var userId = "1";
            var expectedUserFirstName = "Ivo";
            var actualFirstName = await this.service.GetUserFirstNameByIdAsync(userId);

            Assert.Equal(expectedUserFirstName, actualFirstName);
        }

        [Fact]
        public async Task GetUserProfilePictureAsync_ShouldReturnUsersProfilePictureByGivenId()
        {
            var userId = "1";
            var expectedResult = "SomeProfilePicture";
            var actualResult = await this.service.GetUserProfilePictureAsync(userId);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetAllSpecialistsCountAsync_ShouldReturnCorrectCountOfAllSpecialists()
        {
            var expectedCount = 1;
            var actualCount = await this.service.GetAllSpecialistsCountAsync();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task GetAllClientsCountAsync_ShouldReturnCorrectCountOfAllRegularUsers()
        {
            var expectedCount = 1;
            var actualCount = await this.service.GetAllClientsCountAsync();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task GetUserIdBySpecialistIdAsync_ShouldReturnCoorectUserIdByGivenSpecialistId()
        {
            var specialistId = "specialistId";

            var expectedUserId = "2";
            var actualUserId = await this.service.GetUserIdBySpecialistIdAsync(specialistId);

            Assert.Equal(expectedUserId, actualUserId);
        }

        [Fact]
        public async Task MakeUserVip_ShouldMakeUserVipAndSetExpirationDateOfOneWeek()
        {
            var userId = "1";
            await this.service.MakeUserVip(userId);
            var user = this.users.FirstOrDefault(x => x.Id == userId);
            var isVip = user.IsVip == true;
            var hasNewExpirationDate = DateTime.UtcNow.AddDays(6);

            Assert.True(isVip);
            Assert.True(hasNewExpirationDate < user.VipExpirationDate);
        }

        private void InitializeRepositoriesData()
        {
            this.users.AddRange(new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", FirstName = "Ivo", LastName = "Ivov", CityId = 1, Email = "u@u", ProfilePicture = "SomeProfilePicture", IsSpecialist = false, IsVip = false, VipExpirationDate = DateTime.UtcNow },
                new ApplicationUser { Id = "2", FirstName = "Gosho", LastName = "Goshev", CityId = 1, Email = "s@s", ProfilePicture = "SpecProfilePicture", IsSpecialist = true, SpecialistDetailsId = "specialistId" },
            });

            this.DbContext.AddRange(this.users);
            this.DbContext.SaveChanges();
        }
    }
}
