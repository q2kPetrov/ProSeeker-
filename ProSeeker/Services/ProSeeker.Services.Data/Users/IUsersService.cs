﻿namespace ProSeeker.Services.Data.UsersService
{
    using System.Threading.Tasks;

    using ProSeeker.Web.ViewModels.Users;

    public interface IUsersService
    {
        Task<string> GetUserProfilePictureAsync(string userId);

        Task<string> GetUserFirstNameByIdAsync(string userId);

        Task<T> GetUserByIdAsync<T>(string id);

        Task<int> GetAllSpecialistsCountAsync();

        Task<int> GetAllClientsCountAsync();

        Task<string> GetUserIdBySpecialistIdAsync(string specialistId);

        Task MakeUserVip(string userId);

        Task<UserViewModel> GetUserProfileAsync(string currentUserId, string specialistId);

        Task UpdateUserPhoneNumberAsync(string userId, string phoneNumber);
    }
}
