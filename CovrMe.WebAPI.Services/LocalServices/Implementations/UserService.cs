using CovrMe.WebAPI.Data;
using CovrMe.WebAPI.Data.Context;
using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Domain.Repos.Contracts;
using CovrMe.WebAPI.Models.Request.Users;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Models.Result.Users;
using CovrMe.WebAPI.Models.Result.Users.Payloads;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Shared.Constants;
using CovrMe.WebAPI.Shared.Enums;
using CovrMe.WebAPI.Shared.Helpers;
using CovrMe.WebAPI.Shared.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CovrMe.WebAPI.Services.LocalServices.Implementations
{
    public class UserService : IUserService
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IRepository<User> _userRepository;
        private readonly ApplicationDbContext _dbContext;
        private IReadWriteService _readWriteService;
        private IVehicleService _vehicleService;

        public UserService(ILogger<UserService> logger, UserManager<ApplicationUser> userManager,
            IConfiguration configuration, IRepository<User> userRepository, ApplicationDbContext dbContext, IReadWriteService readWriteService,
            IVehicleService vehicleService)
        {
            this._logger = logger;
            this._userManager = userManager;
            this._configuration = configuration;
            this._userRepository = userRepository;
            this._dbContext = dbContext;
            this._readWriteService = readWriteService;
            this._vehicleService = vehicleService;
        }
        public IQueryable<UserModel> GetUserById(string userId)
        {
            var currentUserId = Helpers.ParseStringToGuid(userId);

            var query = this._userRepository
                .AllAsNoTracking()
                .Where(x => x.Id == currentUserId)
                .Select(x => new UserModel
                {
                    Id = x.Id.ToString(),
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    LastName = x.LastName,
                    SurName = x.SurName,
                    FirstName = x.FirstName,
                    UiNumber = x.UiNumber,
                    Address = x.Address,
                    VatNumber = x.VatNumber,
                    BirthDate = x.BirthDate,
                    DrivingExperience = x.DrivingExperience,
                    LatinFirstName = x.LatinFirstName,
                    LatinSurName = x.LatinSurName,
                    LatinLastName = x.LatinLastName,
                    CountryId = x.CountryId,
                    RegionId = x.RegionId,
                    MunicipalityId = x.MuniciplalityId,
                    CityId = x.CityId,
                    LatinAddress = x.LatinAddress,
                    LatinCompanyName = x.LatinCompanyName,
                    CompanyName = x.CompanyName,
                    PostCode = x.PostCode,
                    ParentUserId = x.ParentUserId,
                    AspNetUserId = x.AspNetUserId
                });

            return query;
        }      
        public IQueryable<UserModel> GetUsers()
        {
            var query = this._userRepository
                .AllAsNoTracking()
                .Where(x => x.IsDeleted == false)
                .Select(x => new UserModel
                {
                    Id = x.Id.ToString(),
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    LastName = x.LastName,
                    SurName = x.SurName,
                    FirstName = x.FirstName,
                    UiNumber = x.UiNumber,
                    Address = x.Address,
                    VatNumber = x.VatNumber,
                    BirthDate = x.BirthDate,
                    DrivingExperience = x.DrivingExperience,
                    LatinFirstName = x.LatinFirstName,
                    LatinSurName = x.LatinSurName,
                    LatinLastName = x.LatinLastName,
                    CountryId = x.CountryId,
                    RegionId = x.RegionId,
                    MunicipalityId = x.MuniciplalityId,
                    CityId = x.CityId,
                    LatinAddress = x.LatinAddress,
                    LatinCompanyName = x.LatinCompanyName,
                    CompanyName = x.CompanyName,
                    PostCode = x.PostCode,
                    ParentUserId = x.ParentUserId,
                    AspNetUserId = x.AspNetUserId
                });

            return query;
        }
        public int GetUsersCount()
        {
            var usersCount = this._userRepository
                .AllAsNoTracking()
                .Where(x => x.IsDeleted == false)
                .Count();

            return usersCount;
        }
        public IQueryable<UserModel> GetUsersForPage(UsersForPageInput usersForPageInput)
        {
            int usersToSkip = (usersForPageInput.Page - 1) * usersForPageInput.UsersPerPage;

            var usersForPage = this._userRepository
                .AllAsNoTracking()
                .Where(x => x.IsDeleted == false)
                .Skip(usersToSkip)
                .Take(usersForPageInput.UsersPerPage)
                .Select(x => new UserModel
                {
                    Id = x.Id.ToString(),
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    LastName = x.LastName,
                    SurName = x.SurName,
                    FirstName = x.FirstName,
                    UiNumber = x.UiNumber,
                    Address = x.Address,
                    VatNumber = x.VatNumber,
                    BirthDate = x.BirthDate,
                    DrivingExperience = x.DrivingExperience,
                    LatinFirstName = x.LatinFirstName,
                    LatinSurName = x.LatinSurName,
                    LatinLastName = x.LatinLastName,
                    CountryId = x.CountryId,
                    RegionId = x.RegionId,
                    MunicipalityId = x.MuniciplalityId,
                    CityId = x.CityId,
                    LatinAddress = x.LatinAddress,
                    LatinCompanyName = x.LatinCompanyName,
                    CompanyName = x.CompanyName,
                    PostCode = x.PostCode,
                    ParentUserId = x.ParentUserId,
                    AspNetUserId = x.AspNetUserId
                })
                .OrderBy(x => x.FirstName);

            return usersForPage;
        }
        public IQueryable<T> GetAspUsers<T>()
        {
            var query = this._userRepository
                .AllAsNoTracking()
                .Where(x => x.AspNetUserId != null && x.IsDeleted == false)
                .To<T>();

            return query;
        }
        public IQueryable<T> GetUsersFamilyAndFriends<T>(string userId)
        {
            var currentUserId = Helpers.ParseStringToGuid(userId);

            var query = this._userRepository
                .AllAsNoTracking()
                .Where(x => x.ParentUserId == currentUserId && x.IsDeleted == false)
                .To<T>();

            return query;
        }
        public IQueryable<UserModel> GetUsersFamilyAndFriends(string userId)
        {
            var currentUserId = Helpers.ParseStringToGuid(userId);

            var query = this._userRepository
                .AllAsNoTracking()
                .Where(x => x.ParentUserId == currentUserId && x.IsDeleted == false)
                .Select(x => new UserModel
                {
                    Id = x.Id.ToString(),
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    LastName = x.LastName,
                    SurName = x.SurName,
                    FirstName = x.FirstName,
                    UiNumber = x.UiNumber,
                    Address = x.Address,
                    VatNumber = x.VatNumber,
                    BirthDate = x.BirthDate,
                    DrivingExperience = x.DrivingExperience,
                    LatinFirstName = x.LatinFirstName,
                    LatinSurName = x.LatinSurName,
                    LatinLastName = x.LatinLastName,
                    CountryId = x.CountryId,
                    RegionId = x.RegionId,
                    MunicipalityId = x.MuniciplalityId,
                    CityId = x.CityId,
                    LatinAddress = x.LatinAddress,
                    LatinCompanyName = x.LatinCompanyName,
                    CompanyName = x.CompanyName,
                    PostCode = x.PostCode
                });

            return query;
        }
        public UserModel GetCurrentUserById(string userId)
        {
            var currentUserId = Helpers.ParseStringToGuid(userId);

            var user = this._userRepository
                .AllAsNoTracking()
                .Where(x => x.Id == currentUserId && x.IsDeleted == false)
                .Select(x => new UserModel
                {
                    Id = x.Id.ToString(),
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    LastName = x.LastName,
                    SurName = x.SurName,
                    FirstName = x.FirstName,
                    CompanyName = x.CompanyName,
                    UiNumber = x.UiNumber,
                    Address = x.Address,
                    VatNumber = x.VatNumber,
                    BirthDate = x.BirthDate,
                    DrivingExperience = x.DrivingExperience,
                    LatinFirstName = x.LatinFirstName,
                    LatinSurName = x.LatinSurName,
                    LatinLastName = x.LatinLastName,
                    CountryId = x.CountryId,
                    RegionId = x.RegionId,
                    MunicipalityId = x.MuniciplalityId,
                    CityId = x.CityId,
                    LatinAddress = x.LatinAddress,
                    LatinCompanyName = x.LatinCompanyName,
                    PostCode = x.PostCode
                })
                .FirstOrDefault();

            return user;
        }
        public async Task<UserModel> EditUserInfo(EditUserInfoInput req)
        {
            var result = new UserModel();
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var currentUserId = Helpers.ParseStringToGuid(req.UserId);

                var user = this._userRepository
                    .AllAsNoTracking()
                    .FirstOrDefault(x => x.Id == currentUserId && x.IsDeleted == false);

                var aspNetUserId = user != null ? user.AspNetUserId : string.Empty;
                var currentAspNetUser = await this._userManager.FindByIdAsync(aspNetUserId);

                if (user != null && currentAspNetUser != null)
                {
                    var userExistingEmail = await this._userManager.FindByEmailAsync(req.Email);

                    if (userExistingEmail != null && userExistingEmail.Id != currentAspNetUser.Id)
                    {
                        result.Message = MessageConstants.DuplicateEmailError;
                        throw new Exception(MessageConstants.DuplicateEmailError);
                    }

                    if (!string.IsNullOrEmpty(req.FirstName))
                    {
                        user.FirstName = req.FirstName;
                    }

                    if (!string.IsNullOrEmpty(req.SurName))
                    {
                        user.SurName = req.SurName;
                    }

                    if (!string.IsNullOrEmpty(req.LastName))
                    {
                        user.LastName = req.LastName;
                    }

                    if (req.DrivingExperience.HasValue)
                    {
                        user.DrivingExperience = req.DrivingExperience.Value;
                    }
                    if (!string.IsNullOrEmpty(req.LatinFirstName))
                    {
                        user.LatinFirstName = req.LatinFirstName;
                    }

                    if (!string.IsNullOrEmpty(req.LatinLastName))
                    {
                        user.LatinLastName = req.LatinLastName;
                    }

                    if (!string.IsNullOrEmpty(req.LatinSurName))
                    {
                        user.LatinSurName = req.LatinSurName;
                    }

                    if (!string.IsNullOrEmpty(req.UiNumber))
                    {
                        user.UiNumber = req.UiNumber;
                    }

                    if (!string.IsNullOrEmpty(req.Phone))
                    {
                        user.PhoneNumber = req.Phone;
                    }

                    if (req.BirthDate.HasValue)
                    {
                        user.BirthDate = req.BirthDate.Value;
                    }

                    if (!string.IsNullOrEmpty(req.LatinAddress))
                    {
                        user.LatinAddress = req.LatinAddress;
                    }

                    if (!string.IsNullOrEmpty(req.Address))
                    {
                        user.Address = req.Address;
                    }

                    if (!string.IsNullOrEmpty(req.CountryId))
                    {
                        user.CountryId = req.CountryId;
                    }

                    if (!string.IsNullOrEmpty(req.CityId))
                    {
                        user.CityId = req.CityId;
                    }

                    if (!string.IsNullOrEmpty(req.MuniciplalityId))
                    {
                        user.MuniciplalityId = req.MuniciplalityId;
                    }

                    if (!string.IsNullOrEmpty(req.RegionId))
                    {
                        user.RegionId = req.RegionId;
                    }

                    if (!string.IsNullOrEmpty(req.PostCode))
                    {
                        user.PostCode = req.PostCode;
                    }

                    string currentCyrillicCompanyName = user.CompanyName;
                    user.CompanyName = req.CompanyName;

                    if (!string.IsNullOrEmpty(req.LatinCompanyName))
                    {
                        user.LatinCompanyName = req.LatinCompanyName;
                        user.CompanyName = currentCyrillicCompanyName;
                    }

                    user.VatNumber = req.VatNumber;
                    
                    currentAspNetUser.Email = req.Email;
                    currentAspNetUser.UserName = req.Email;
                    user.Email = req.Email;

                    await this._userManager.UpdateAsync(currentAspNetUser);

                    this._userRepository.Update(user);
                    await this._userRepository.SaveChangesAsync();

                    transaction.Commit();

                    result = this.GetCurrentUserById(user.Id.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                transaction.Rollback();               
            }

            return result;
        }
        public async Task<UserModel> EditFamilyFriendsUser(EditUserInfoInput req)
        {
            var result = new UserModel();

            try
            {
                var currentUserId = Helpers.ParseStringToGuid(req.UserId);
                var currentParentUserId = Helpers.ParseStringToGuid(req.ParentUserId);

                var user = this._userRepository
                    .AllAsNoTracking()
                    .FirstOrDefault(x => x.Id == currentUserId && x.ParentUserId == currentParentUserId);

                if (user != null)
                {
                    if (!string.IsNullOrEmpty(req.Phone))
                    {
                        user.PhoneNumber = req.Phone;
                    }

                    if (!string.IsNullOrEmpty(req.Email))
                    {
                        user.Email = req.Email;
                    }

                    if (!string.IsNullOrEmpty(req.FirstName))
                    {
                        user.FirstName = req.FirstName;
                    }

                    if (!string.IsNullOrEmpty(req.SurName))
                    {
                        user.SurName = req.SurName;
                    }

                    if (!string.IsNullOrEmpty(req.LastName))
                    {
                        user.LastName = req.LastName;
                    }

                    if (req.DrivingExperience.HasValue)
                    {
                        user.DrivingExperience = req.DrivingExperience;
                    }

                    if (!string.IsNullOrEmpty(req.LatinFirstName))
                    {
                        user.LatinFirstName = req.LatinFirstName;
                    }

                    if (!string.IsNullOrEmpty(req.LatinLastName))
                    {
                        user.LatinLastName = req.LatinLastName;
                    }

                    if (!string.IsNullOrEmpty(req.LatinSurName))
                    {
                        user.LatinSurName = req.LatinSurName;
                    }

                    if (!string.IsNullOrEmpty(req.UiNumber))
                    {
                        user.UiNumber = req.UiNumber;
                    }

                    if (req.BirthDate.HasValue)
                    {
                        user.BirthDate = req.BirthDate.Value;
                    }

                    if (!string.IsNullOrEmpty(req.LatinAddress))
                    {
                        user.LatinAddress = req.LatinAddress;
                    }

                    if (!string.IsNullOrEmpty(req.LatinCompanyName))
                    {
                        user.LatinCompanyName = req.LatinCompanyName;
                    }

                    if (!string.IsNullOrEmpty(req.Address))
                    {
                        user.Address = req.Address;
                    }

                    if (!string.IsNullOrEmpty(req.CountryId))
                    {
                        user.CountryId = req.CountryId;
                    }

                    if (!string.IsNullOrEmpty(req.CityId))
                    {
                        user.CityId = req.CityId;
                    }

                    if (!string.IsNullOrEmpty(req.MuniciplalityId))
                    {
                        user.MuniciplalityId = req.MuniciplalityId;
                    }

                    if (!string.IsNullOrEmpty(req.RegionId))
                    {
                        user.RegionId = req.RegionId;
                    }

                    if (!string.IsNullOrEmpty(req.PostCode))
                    {
                        user.PostCode = req.PostCode;
                    }

                    user.VatNumber = req.VatNumber;
                    user.CompanyName = req.CompanyName;

                    this._userRepository.Update(user);
                    await this._userRepository.SaveChangesAsync();

                    result = this.GetCurrentUserById(user.Id.ToString());
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }
        public async Task<AddUserToFamilyAndFriendsPayload> AddUserToFamilyAndFriends(AddUserToFamilyAndFriendsInput req)
        {
            var result = new AddUserToFamilyAndFriendsPayload();
            try
            {
                var currentUser = Helpers.ParseStringToGuid(req.UserId);

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = req.FirstName,
                    SurName = req.SurName,
                    LastName = req.LastName,
                    LatinFirstName = req.LatinFirstName,
                    LatinSurName = req.LatinSurName,
                    LatinLastName = req.LatinLastName,
                    PhoneNumber = req.Phone,
                    Email = req.Email,
                    UiNumber = req.UiNumber,
                    VatNumber = req.VatNumber,
                    Address = req.Address,
                    ParentUserId = currentUser,
                    DrivingExperience = req.DrivingExperience.HasValue ? req.DrivingExperience.Value : 0,
                    BirthDate = req.Birthdate.HasValue ? req.Birthdate.Value : null,
                    CompanyName = req.CompanyName,
                    LatinAddress = req.LatinAddress,
                    LatinCompanyName = req.LatinCompanyName,
                    CountryId = req.CountryId,
                    CityId = req.CityId,
                    MuniciplalityId = req.MuniciplalityId,
                    RegionId = req.RegionId,
                    PostCode = req.PostCode
                };

                await this._userRepository.AddAsync(user);
                await this._userRepository.SaveChangesAsync();

                var resultUser = this.GetCurrentUserById(user.Id.ToString());

                result.User = resultUser;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }
        public async Task<DeleteUserPayload> DeleteUser(DeleteUserInput req)
        {
            var result = new DeleteUserPayload();
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var currentUserId = Helpers.ParseStringToGuid(req.UserId);

                var user = this._userRepository
                    .AllAsNoTracking()
                    .FirstOrDefault(x => x.Id == currentUserId && x.IsDeleted == false);

                var aspNetUserId = user != null ? user.AspNetUserId : string.Empty;
                var currentAspNetUser = await this._userManager.FindByIdAsync(aspNetUserId);
                string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");

                if (user != null && currentAspNetUser != null)
                {
                    user.IsDeleted = true;

                    this._userRepository.Update(user);
                    currentAspNetUser.UserName = dateStr + "_" + req.Email;
                    currentAspNetUser.Email = dateStr + "_" + req.Email;
                    user.Email = dateStr + "_" + req.Email;

                    this.DeleteDeletingUserFamilyAndFriends(user.Id);
                    await this._vehicleService.DeleteUserVehicles(user.Id.ToString());
                    await _userManager.UpdateAsync(currentAspNetUser);
                    await this._userRepository.SaveChangesAsync();

                    result.Code = (int)GeneralStatusEnum.Success;
                }


                transaction.Commit();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                transaction.Rollback();
                return result;
            }
        }
        public async Task<string> AddUser(UserModel req)
        {
            string result = string.Empty;

            try
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    DrivingExperience = req.DrivingExperience,
                    FirstName = req.FirstName,
                    SurName = req.SurName,
                    LastName = req.LastName,
                    LatinAddress = req.LatinAddress,
                    LatinFirstName = req.LatinFirstName,
                    LatinSurName = req.LatinSurName,
                    LatinLastName = req.LatinLastName,
                    UiNumber = req.UiNumber,
                    VatNumber = req.VatNumber,
                    Address = req.Address,
                    CityId = null,
                    AspNetUserId = null,
                    PhoneNumber = req.PhoneNumber,
                    ParentUserId = null,
                    Email = req.Email,
                    BirthDate = req.BirthDate,
                    IsDeleted = false,
                    CompanyName = req.CompanyName,
                    CountryId = null,
                    RegionId = null,
                    MuniciplalityId = null,
                    PostCode = null,
                    LatinCompanyName = req.LatinCompanyName
                };

                await this._userRepository.AddAsync(user);
                await this._userRepository.SaveChangesAsync();

                result = user.Id.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }
        public UserModel GetUserByAspNetUserId(string aspNetUserId)
        {
            var user = this._userRepository
                .AllAsNoTracking()
                .Where(x => x.AspNetUserId == aspNetUserId)
                .Select(x => new UserModel
                {
                    Id = x.Id.ToString(),
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    LastName = x.LastName,
                    SurName = x.SurName,
                    FirstName = x.FirstName,
                    UiNumber = x.UiNumber,
                    Address = x.Address,
                    VatNumber = x.VatNumber,
                    BirthDate = x.BirthDate,
                    DrivingExperience = x.DrivingExperience,
                    LatinFirstName = x.LatinFirstName,
                    LatinSurName = x.LatinSurName,
                    LatinLastName = x.LatinLastName,
                    CountryId = x.CountryId,
                    RegionId = x.RegionId,
                    MunicipalityId = x.MuniciplalityId,
                    CityId = x.CityId,
                    LatinAddress = x.LatinAddress,
                    LatinCompanyName = x.LatinCompanyName,
                    PostCode = x.PostCode
                })
                .FirstOrDefault();


            return user;
        }
        public async Task<BaseResultModel> AddMultipleUserToFamilyAndFriends(List<AddUserToFamilyAndFriendsInput> users)
        {
            var result = new BaseResultModel();
            try
            {
                foreach (var user in users)
                {
                    var currentUser = Helpers.ParseStringToGuid(user.UserId);

                    var newUser = new User
                    {
                        Id = Guid.NewGuid(),
                        FirstName = user.FirstName,
                        SurName = user.SurName,
                        LastName = user.LastName,
                        LatinFirstName = user.LatinFirstName,
                        LatinSurName = user.LatinSurName,
                        LatinLastName = user.LatinLastName,
                        PhoneNumber = user.Phone,
                        Email = user.Email,
                        UiNumber = user.UiNumber,
                        VatNumber = user.VatNumber,
                        Address = user.Address,
                        ParentUserId = currentUser,
                        DrivingExperience = user.DrivingExperience.HasValue ? user.DrivingExperience.Value : 0,
                        BirthDate = user.Birthdate.HasValue ? user.Birthdate.Value : null,
                        CompanyName = user.CompanyName,
                        LatinAddress = user.LatinAddress,
                        LatinCompanyName = user.LatinCompanyName
                    };

                    if (!string.IsNullOrEmpty(user.CountryId))
                    {
                        newUser.CountryId = user.CountryId;
                    }

                    if (!string.IsNullOrEmpty(user.CityId))
                    {
                        newUser.CityId = user.CityId;
                    }

                    if (!string.IsNullOrEmpty(user.MuniciplalityId))
                    {
                        newUser.MuniciplalityId = user.MuniciplalityId;
                    }

                    if (!string.IsNullOrEmpty(user.RegionId))
                    {
                        newUser.RegionId = user.RegionId;
                    }

                    if (!string.IsNullOrEmpty(user.PostCode))
                    {
                        newUser.PostCode = user.PostCode;
                    }

                    await this._userRepository.AddAsync(newUser);
                }

                await this._userRepository.SaveChangesAsync();
                result.Code = (int)GeneralStatusEnum.Success;
            }
            catch (Exception ex)
            {

                this._logger.LogError(ex.Message);
                result.Code = (int)GeneralStatusEnum.Unsuccess;
            }

            return result;
        }
        public async Task<BaseResultModel> EditMultipleFamilyFriendsUser(List<EditUserInfoInput> users)
        {
            var result = new BaseResultModel();

            try
            {
                foreach (var user in users)
                {
                    var currentUserId = Helpers.ParseStringToGuid(user.UserId);

                    var currentUser = this._userRepository
                            .AllAsNoTracking()
                            .FirstOrDefault(x => x.Id == currentUserId);

                    if (currentUser != null)
                    {
                        if (!string.IsNullOrEmpty(user.Phone))
                        {
                            currentUser.PhoneNumber = user.Phone;
                        }

                        if (!string.IsNullOrEmpty(user.Email))
                        {
                            currentUser.Email = user.Email;
                        }

                        if (!string.IsNullOrEmpty(user.FirstName))
                        {
                            currentUser.FirstName = user.FirstName;
                        }

                        if (!string.IsNullOrEmpty(user.SurName))
                        {
                            currentUser.SurName = user.SurName;
                        }

                        if (!string.IsNullOrEmpty(user.LastName))
                        {
                            currentUser.LastName = user.LastName;
                        }

                        if (!string.IsNullOrEmpty(user.CountryId))
                        {
                            currentUser.CountryId = user.CountryId;
                        }

                        if (!string.IsNullOrEmpty(user.CityId))
                        {
                            currentUser.CityId = user.CityId;
                        }

                        if (!string.IsNullOrEmpty(user.MuniciplalityId))
                        {
                            currentUser.MuniciplalityId = user.MuniciplalityId;
                        }

                        if (!string.IsNullOrEmpty(user.RegionId))
                        {
                            currentUser.RegionId = user.RegionId;
                        }

                        if (!string.IsNullOrEmpty(user.PostCode))
                        {
                            currentUser.PostCode = user.PostCode;
                        }

                        if (user.DrivingExperience.HasValue)
                        {
                            currentUser.DrivingExperience = user.DrivingExperience;
                        }

                        if (!string.IsNullOrEmpty(user.LatinFirstName))
                        {
                            currentUser.LatinFirstName = user.LatinFirstName;
                        }

                        if (!string.IsNullOrEmpty(user.LatinLastName))
                        {
                            currentUser.LatinLastName = user.LatinLastName;
                        }

                        if (!string.IsNullOrEmpty(user.LatinSurName))
                        {
                            currentUser.LatinSurName = user.LatinSurName;
                        }

                        if (!string.IsNullOrEmpty(user.UiNumber))
                        {
                            currentUser.UiNumber = user.UiNumber;
                        }

                        if (!string.IsNullOrEmpty(user.VatNumber))
                        {
                            currentUser.VatNumber = user.VatNumber;
                        }

                        if (!string.IsNullOrEmpty(user.CompanyName))
                        {
                            currentUser.CompanyName = user.CompanyName;
                        }

                        if (user.BirthDate.HasValue)
                        {
                            currentUser.BirthDate = user.BirthDate.Value;
                        }

                        if (!string.IsNullOrEmpty(user.LatinAddress))
                        {
                            currentUser.LatinAddress = user.LatinAddress;
                        }

                        if (!string.IsNullOrEmpty(user.Address))
                        {
                            currentUser.Address = user.Address;
                        }

                        if (!string.IsNullOrEmpty(user.LatinCompanyName))
                        {
                            currentUser.LatinCompanyName = user.LatinCompanyName;
                        }

                        this._userRepository.Update(currentUser);
                        await this._userRepository.SaveChangesAsync();
                    }
                }

                await this._userRepository.SaveChangesAsync();

                result.Code = (int)GeneralStatusEnum.Success;
            }
            catch (Exception ex)
            {

                this._logger.LogError(ex.Message);
                result.Code = (int)GeneralStatusEnum.Unsuccess;
            }

            return result;
        }
        public string GetUsersGuiltTypes(int hashCode)
        {
            string path = this._configuration.GetSection("ReadWriteOptions").GetSection("UserGuiltTypes").Value;

            string guiltTypes = this._readWriteService.ReadFile(path);

            int currentHashCode = guiltTypes.GetHashCode();

            if (currentHashCode == hashCode)
            {
                return string.Empty;
            }
            else
            {
                return guiltTypes;
            }
        }
        public string GetUsersGuiltTypes()
        {
            string path = this._configuration.GetSection("ReadWriteOptions").GetSection("UserGuiltTypes").Value;

            string guiltTypes = this._readWriteService.ReadFile(path);

            return guiltTypes;
        }
        private bool CheckForDuplicatedEmail(string email, string userId)
        {
            var currentUserId = Helpers.ParseStringToGuid(userId);

            var user = this._userRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Email == email && x.Id != currentUserId);              

            if(user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void DeleteDeletingUserFamilyAndFriends(Guid userId)
        {
            var friends = this.GetDeletingUserFamilyAndFriends(userId);

            foreach(var friend in friends)
            {
                string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                string email = friend.Email;

                var currentUserId = Helpers.ParseStringToGuid(friend.Id);

                var user = this._userRepository
                    .AllAsNoTracking()
                    .FirstOrDefault(x => x.Id == currentUserId && x.IsDeleted == false);

                user.IsDeleted = true;

                if (!string.IsNullOrEmpty(email))
                {
                    user.Email = dateStr + "_" + email;
                }
                
                this._userRepository.Update(user);
            }           
        }
        private List<UserModel> GetDeletingUserFamilyAndFriends(Guid userId)
        {
            var users = this._userRepository
                .AllAsNoTracking()
                .Where(x => x.ParentUserId == userId && x.IsDeleted == false)
                .Select(x => new UserModel
                {
                    Id = x.Id.ToString(),
                    Email = x.Email
                })
                .ToList();

            return users;
        }
    }
}
