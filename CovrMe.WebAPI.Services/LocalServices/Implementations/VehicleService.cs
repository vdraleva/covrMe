using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Domain.Repos.Contracts;
using CovrMe.WebAPI.Models.Request.Vehicles;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Models.Result.Sirma.Vehicle;
using CovrMe.WebAPI.Models.Result.Vehicles;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Services.Sirma.Contracts;
using CovrMe.WebAPI.Shared.Constants;
using CovrMe.WebAPI.Shared.Enums;
using CovrMe.WebAPI.Shared.Helpers;
using CovrMe.WebAPI.Shared.Mapping;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
//using UniqaSoapService;

namespace CovrMe.WebAPI.Services.LocalServices.Implementations
{
    public class VehicleService : IVehicleService
    {
        private IRepository<Vehicle> _vehicleRepository;
        private IReadWriteService _readWriteService;
        private readonly ILogger _logger;
        private IConfiguration _configuration;
        private ISirmaVehicleService _sirmavehicleService;

        public VehicleService(ILogger<VehicleService> logger, IRepository<Vehicle> vehicleRepository, IConfiguration configuration,
            IReadWriteService readWriteService, ISirmaVehicleService sirmavehicleService)
        {
            this._logger = logger;
            this._vehicleRepository = vehicleRepository;
            this._configuration = configuration;
            this._readWriteService = readWriteService;
            this._sirmavehicleService = sirmavehicleService;
        }

        public IQueryable<VehicleResultModel> GetVehicleByVehicleId(string vehicleId)
        {
            var currentVehicleId = Helpers.ParseStringToGuid(vehicleId);

            var vehicle = this._vehicleRepository
                .AllAsNoTracking()
                .Where(x => x.Id == currentVehicleId && (x.IsDeleted == null || x.IsDeleted == false))
                .Select(x => new VehicleResultModel
                {
                    PlateNumber = x.PlateNumber,
                    RegistrationCertificateNumber = x.RegistrationCertificateNumber,
                    BrandId = x.BrandId,
                    Brand = x.Brand,
                    ModelId = x.ModelId,
                    Model = x.Model,
                    VehicleType = x.VehicleType,
                    VehicleTypeId = x.VehicleTypeId,
                    VehicleUsage = x.VehicleUsage,
                    VehicleUsageId = x.VehicleUsageId,
                    FirstRegistrationDate = x.FirstRegistrationDate,
                    Id = x.Id.ToString(),
                    Vin = x.Vin,
                    EngineVolume = x.EngineVolume.HasValue ? x.EngineVolume.Value : 0,
                    BatteryCapacity = x.BatteryCapacity.HasValue ? x.BatteryCapacity.Value : 0,
                    BodyType = x.BodyType.HasValue ? x.BodyType.Value : (byte)0,
                    Places = x.Places.HasValue ? x.Places.Value : (byte)0,
                    EngineType = x.EngineType.HasValue ? x.EngineType.Value : (byte)0,
                    NetWeight = x.NetWeight.HasValue ? x.NetWeight.Value : 0,
                    GrossWeight = x.GrossWeight.HasValue ? x.GrossWeight.Value : 0,
                    VehicleKilowatts = x.VehicleKilowatts.HasValue ? x.VehicleKilowatts.Value : 0,
                    SteeringWheel = x.SteeringWheel.HasValue ? x.SteeringWheel.Value : (byte)0,
                    Color = x.Color.HasValue ? x.Color.Value : (byte)0
                });

            return vehicle;
        }
        public async Task<VehicleResultModel> CreateVehicleAsync(AddVehicleInput req)
        {
            var result = new VehicleResultModel();

            try
            {
                var currentUserId = Helpers.ParseStringToGuid(req.UserId);

                var vehicle = new Vehicle
                {
                    Id = Guid.NewGuid(),
                    RegistrationCertificateNumber = req.RegistrationCertificateNumber,
                    PlateNumber = req.VehiclePlateNumber,
                    UserId = currentUserId,
                    BrandId = req.BrandId,
                    Brand = req.Brand,
                    ModelId = req.ModelId,
                    Model = req.Model,
                    VehicleUsageId = req.VehicleUsageId,
                    VehicleUsage = req.VehicleUsage,
                    VehicleTypeId = req.VehicleTypeId,
                    VehicleType = req.VehicleType,
                    FirstRegistrationDate = req.FirstRegistrationDate,
                    Vin = req.Vin,
                    EngineVolume = req.EngineVolume,
                    BatteryCapacity = req.BatteryCapacity,
                    EngineType = req.EngineType,
                    BodyType = req.BodyType,
                    Places = req.Places,
                    Color = req.Color,
                    NetWeight = req.NetWeight,
                    GrossWeight = req.GrossWeight,
                    VehicleKilowatts = req.VehicleKilowatts,
                    SteeringWheel = req.SteeringWheel,
                    IsDeleted = false
                };

                await this._vehicleRepository.AddAsync(vehicle);
                await this._vehicleRepository.SaveChangesAsync();

                result = this.GetVehicleById(vehicle.Id.ToString());
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == GlobalConstants.SqlUniqueErrorCode)
                {
                    result.ExceptionType = GlobalConstants.SqlUniqueErrorCode;
                    result.Message = MessageConstants.ExistingVehicleNumbers;
                }

                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return result;
        }
        public async Task<VehicleResultModel> EditUserVehicle(EditVehicleInput req)
        {
            var result = new VehicleResultModel();

            try
            {
                var currentUserId = Helpers.ParseStringToGuid(req.UserId);
                var currentVehicleId = Helpers.ParseStringToGuid(req.VehicleId);

                var vehicle = this._vehicleRepository
                    .AllAsNoTracking()
                    .Where(x => x.UserId == currentUserId && x.Id == currentVehicleId && (x.IsDeleted == null || x.IsDeleted == false))
                    .FirstOrDefault();

                if (vehicle != null)
                {
                    vehicle.RegistrationCertificateNumber = req.RegistrationCertificateNumber;
                    vehicle.PlateNumber = req.VehiclePlateNumber;
                    vehicle.Model = req.Model;
                    vehicle.ModelId = req.ModelId;
                    vehicle.Brand = req.Brand;
                    vehicle.BrandId = req.BrandId;
                    vehicle.VehicleType = req.VehicleType;
                    vehicle.VehicleTypeId = req.VehicleTypeId;
                    vehicle.VehicleUsage = req.VehicleUsage;
                    vehicle.VehicleUsageId = req.VehicleUsageId;
                    vehicle.FirstRegistrationDate = req.FirstRegistrationDate;
                    vehicle.IsDeleted = false;

                    if (!string.IsNullOrEmpty(req.Vin))
                    {
                        vehicle.Vin = req.Vin;
                    }

                    if (req.EngineVolume != 0)
                    {
                        vehicle.EngineVolume = req.EngineVolume;
                    }

                    if (req.BatteryCapacity != 0)
                    {
                        vehicle.BatteryCapacity = req.BatteryCapacity;
                    }

                    if (req.EngineType != 0)
                    {
                        vehicle.EngineType = req.EngineType;
                    }

                    if (req.BodyType != 0)
                    {
                        vehicle.BodyType = req.BodyType;
                    }

                    if (req.Places != 0)
                    {
                        vehicle.Places = req.Places;
                    }

                    if (req.Color != 0)
                    {
                        vehicle.Color = req.Color;
                    }

                    if (req.NetWeight != 0)
                    {
                        vehicle.NetWeight = req.NetWeight;
                    }

                    if (req.GrossWeight != 0)
                    {
                        vehicle.GrossWeight = req.GrossWeight;
                    }

                    if (req.VehicleKilowatts != 0)
                    {
                        vehicle.VehicleKilowatts = req.VehicleKilowatts;
                    }

                    if (req.SteeringWheel != 0)
                    {
                        vehicle.SteeringWheel = req.SteeringWheel;
                    }

                    this._vehicleRepository.Update(vehicle);
                    await this._vehicleRepository.SaveChangesAsync();

                    result = this.GetVehicleById(req.VehicleId);
                }

            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == GlobalConstants.SqlUniqueErrorCode)
                {
                    result.ExceptionType = GlobalConstants.SqlUniqueErrorCode;
                    result.Message = MessageConstants.ExistingVehicleNumbers;
                }

                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                
            }

            return result;
        }
        public IQueryable<VehicleResultModel> GetVehicleByUserId(string userId)
        {
            var currentUserId = Helpers.ParseStringToGuid(userId);

            var query = this._vehicleRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == currentUserId && (x.IsDeleted == null || x.IsDeleted == false))
                .Select(x => new VehicleResultModel
                {
                    Id = x.Id.ToString(),
                    RegistrationCertificateNumber = x.RegistrationCertificateNumber,
                    PlateNumber = x.PlateNumber,
                    UserId = x.UserId.ToString(),
                    FirstRegistrationDate = x.FirstRegistrationDate,
                    BrandId = x.BrandId,
                    Brand = x.Brand,
                    ModelId = x.ModelId,
                    Model = x.Model,
                    VehicleType = x.VehicleType,
                    VehicleTypeId = x.VehicleTypeId,
                    VehicleUsage = x.VehicleUsage,
                    VehicleUsageId = x.VehicleUsageId,
                    Vin = x.Vin,
                    EngineVolume = x.EngineVolume.HasValue ? x.EngineVolume.Value : 0,
                    BatteryCapacity = x.BatteryCapacity.HasValue ? x.BatteryCapacity.Value : 0,
                    BodyType = x.BodyType.HasValue ? x.BodyType.Value : (byte)0,
                    Places = x.Places.HasValue ? x.Places.Value : (byte)0,
                    Color = x.Color.HasValue ? x.Color.Value : (byte)0,
                    EngineType = x.EngineType.HasValue ? x.EngineType.Value : (byte)0,
                    NetWeight = x.NetWeight.HasValue ? x.NetWeight.Value : 0,
                    GrossWeight = x.GrossWeight.HasValue ? x.GrossWeight.Value : 0,
                    VehicleKilowatts = x.VehicleKilowatts.HasValue ? x.VehicleKilowatts.Value : 0,
                    SteeringWheel = x.SteeringWheel.HasValue ? x.SteeringWheel.Value : (byte)0
                });

            return query;
        }
        public T GetVehicleByPlateNumber<T>(string vehiclePlateNumber, string userId)
        {
            var currentUserId = Helpers.ParseStringToGuid(userId);

            var vehicle = _vehicleRepository
                .AllAsNoTracking()
                .Where(x => x.PlateNumber == vehiclePlateNumber && x.UserId == currentUserId && (x.IsDeleted == null || x.IsDeleted == false))
                .To<T>()
                .FirstOrDefault();

            return vehicle;
        }
        public IQueryable<T> GetVehicles<T>()
        {

            var query = this._vehicleRepository
                .AllAsNoTracking()
                .Where(x => (x.IsDeleted == null || x.IsDeleted == false))
                .To<T>();

            return query;
        }
        public VehicleResultModel GetVehicleById(string id)
        {
            var currentVehicleId = Helpers.ParseStringToGuid(id);

            var vehicle = _vehicleRepository
                .AllAsNoTracking()
                .Where(x => x.Id == currentVehicleId && (x.IsDeleted == null || x.IsDeleted == false))
                .Select(x => new VehicleResultModel
                {
                    Id = x.Id.ToString(),
                    RegistrationCertificateNumber = x.RegistrationCertificateNumber,
                    PlateNumber = x.PlateNumber,
                    UserId = x.UserId.ToString(),
                    BrandId = x.BrandId,
                    Brand = x.Brand,
                    ModelId = x.ModelId,
                    Model = x.Model,
                    VehicleType = x.VehicleType,
                    VehicleTypeId = x.VehicleTypeId,
                    VehicleUsage = x.VehicleUsage,
                    VehicleUsageId = x.VehicleUsageId,
                    FirstRegistrationDate = x.FirstRegistrationDate,
                    Vin = x.Vin,
                    EngineVolume = x.EngineVolume.HasValue ? x.EngineVolume.Value : 0,
                    BatteryCapacity = x.BatteryCapacity.HasValue ? x.BatteryCapacity.Value : 0,
                    BodyType = x.BodyType.HasValue ? x.BodyType.Value : (byte)0,
                    Places = x.Places.HasValue ? x.Places.Value : (byte)0,
                    EngineType = x.EngineType.HasValue ? x.EngineType.Value : (byte)0,
                    NetWeight = x.NetWeight.HasValue ? x.NetWeight.Value : 0,
                    GrossWeight = x.GrossWeight.HasValue ? x.GrossWeight.Value : 0,
                    VehicleKilowatts = x.VehicleKilowatts.HasValue ? x.VehicleKilowatts.Value : 0,
                    SteeringWheel = x.SteeringWheel.HasValue ? x.SteeringWheel.Value : (byte)0,
                    Color = x.Color.HasValue ? x.Color.Value : (byte)0
                })
                .FirstOrDefault();

            return vehicle;
        }
        public string GetVehiclesBodyTypes(int hashCode)
        {
            string path = this._configuration.GetSection("ReadWriteOptions").GetSection("VehicleBodyTypes").Value;

            string bodyTypes = this._readWriteService.ReadFile(path);

            int currentHashCode = bodyTypes.GetHashCode();

            if (currentHashCode == hashCode)
            {
                return string.Empty;
            }
            else
            {
                return bodyTypes;
            }
        }
        public string GetVehiclesBodyTypes()
        {
            string path = this._configuration.GetSection("ReadWriteOptions").GetSection("VehicleBodyTypes").Value;

            string bodyTypes = this._readWriteService.ReadFile(path);

            return bodyTypes;
        }
        public string GetVehiclesEngineTypes(int hashCode)
        {
            string path = this._configuration.GetSection("ReadWriteOptions").GetSection("VehicleEngineTypes").Value;

            string engineTypes = this._readWriteService.ReadFile(path);

            int currentHashCode = engineTypes.GetHashCode();

            if (currentHashCode == hashCode)
            {
                return string.Empty;
            }
            else
            {
                return engineTypes;
            }
        }
        public string GetVehiclesEngineTypes()
        {
            string path = this._configuration.GetSection("ReadWriteOptions").GetSection("VehicleEngineTypes").Value;

            string engineTypes = this._readWriteService.ReadFile(path);

            int currentHashCode = engineTypes.GetHashCode();

            return engineTypes;
        }
        public string GetVehiclesColors(int hashCode)
        {
            string path = this._configuration.GetSection("ReadWriteOptions").GetSection("VehicleColors").Value;

            string colors = this._readWriteService.ReadFile(path);

            int currentHashCode = colors.GetHashCode();

            if (currentHashCode == hashCode)
            {
                return string.Empty;
            }
            else
            {
                return colors;
            }
        }
        public string GetVehiclesColors()
        {
            string path = this._configuration.GetSection("ReadWriteOptions").GetSection("VehicleColors").Value;

            string colors = this._readWriteService.ReadFile(path);

            return colors;
        }
        public string GetVehicleBrands()
        {
            string path = this._configuration.GetSection("ReadWriteOptions").GetSection("VehicleMarks").Value;

            string brands = this._readWriteService.ReadFile(path);

            return brands;
        }
        public async Task<BaseResultModel> GetVehicleModelsFromSirma()
        {
            var result = new BaseResultModel();

            try
            {
                var brands = this.GetVehicleBrands();

                if (!string.IsNullOrEmpty(brands))
                {
                    var brandsData = JsonConvert.DeserializeObject<GetVehicleBrandsResultModel>(brands);
                    int counter = 1;
                    foreach (var brand in brandsData.Brands)
                    {
                        var models = await this._sirmavehicleService.GetVehicleModelsByBrandId(brand.Id);

                        models.Models = models.Models.Where(x => x.Name != "--" && x.Name != "-").ToList();

                        foreach (var model in models.Models)
                        {
                            model.Id = counter.ToString();
                            counter++;
                        }

                        var modelsAsString = JsonConvert.SerializeObject(models);
                        string modelsBasePath = this._configuration.GetSection("ReadWriteOptions").GetSection("VehicleModels").Value;

                        string modelsPath = $"{modelsBasePath}{brand.Name}.json";

                        this._readWriteService.WriteTextToFile(modelsPath, modelsAsString);
                    }
                }

                result.Code = (int)GeneralStatusEnum.Success;

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
            }

            return result;
        }
        public string GetVehicleModelsByBrandName(string brand)
        {
            string modelsBasePath = this._configuration.GetSection("ReadWriteOptions").GetSection("VehicleModels").Value;
            string modelsPath = $"{modelsBasePath}{brand}.json";
            string models = this._readWriteService.ReadFile(modelsPath);
            return models;
        }
        public async Task DeleteUserVehicles(string userId)
        {
            var currentUserId = Helpers.ParseStringToGuid(userId);

            var vehicles = this._vehicleRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == currentUserId && x.IsDeleted == null || x.IsDeleted == false);

            foreach (var vehicle in vehicles)
            {
                string dateStr = DateTime.Now.ToString("yyyyMMddHHmm");
                var plateNumber = vehicle.PlateNumber;
                var certNumber = vehicle.RegistrationCertificateNumber;

                vehicle.IsDeleted = true;
                vehicle.PlateNumber = plateNumber + "_" + dateStr;
                vehicle.RegistrationCertificateNumber = certNumber + "_" + dateStr;

                this._vehicleRepository.Update(vehicle);
            }

            await this._vehicleRepository.SaveChangesAsync();
        }
    }
}
