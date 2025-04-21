using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using CovrMe.ApplicationHelpers;
using CovrMe.Factories;
using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.Request;
using CovrMe.Models.Insurances.Result;
using CovrMe.Models.Locations.Result;
using CovrMe.Models.Users;
using CovrMe.Models.Users.Result;
using CovrMe.Models.Vehicles;
using CovrMe.Services.Contracts;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.Pages;
using CovrMe.Views.Pages.Insurances;
using CovrMe.Views.Pages.Insurances.CivilInsurance;
using CovrMe.Views.Pages.Insurances.HealthInsurance;
using CovrMe.Views.Pages.Insurances.Mountain;
using CovrMe.Views.Pages.Insurances.MyThings;
using CovrMe.Views.Pages.Insurances.TravelInsurance;
using Maui.Plugins.PageResolver;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace CovrMe.ViewModels.Pages
{
    public partial class HomePageViewModel : BaseViewModel
    {
        #region Fields

        private readonly ICacheService _cacheService;
        private readonly IUserService _userService;
        private readonly IVehicleService _vehicleService;

        private IInsuranceService _insuranceService;

        private ObservableCollection<MyInsurancesModel> _activeInsurances;
        private ObservableCollection<MyInsurancesModel> _expiredInsurances;

        bool _firstCardOpened = false;
        string _firstCardArrow = GlobalConstants.ArrowImgMore;
        bool _secondCardOpened = false;
        string _secondCardArrow = GlobalConstants.ArrowImgMore;
        bool _thirdCardOpened = false;
        string _thirdCardArrow = GlobalConstants.ArrowImgMore;

        private string _selectedCountryShortCode;
        private string _userName;

        private string _activeInsuranceColor = "#D7D7D7";
        private string _expiredInsuranceColor = "#D7D7D7";

        private List<InsuranceModel> _insurances;

        private readonly IDeliveryService _deliveryService;

        #endregion
        public HomePageViewModel(ICacheService cacheService, IInsuranceService insuranceService, IUserService userService, IVehicleService vehicleService)
        {
            this._cacheService = cacheService;
            this._userService = userService;
            this._vehicleService = vehicleService;

            this._insuranceService = insuranceService;

            this.ActiveInsurances = new ObservableCollection<MyInsurancesModel>();
            this.ExpiredInsurances = new ObservableCollection<MyInsurancesModel>();

            App.IsGroupInsurance = false;
            UserName = App.UserName;

            //Task.Run(async () => { await GetCacheValues(); }).Wait();                 
        }

        #region Collections

        public ObservableCollection<MyInsurancesModel> ActiveInsurances
        {
            get => _activeInsurances;
            set => SetProperty(ref _activeInsurances, value);
        }

        public ObservableCollection<MyInsurancesModel> ExpiredInsurances
        {
            get => _expiredInsurances;
            set => SetProperty(ref _expiredInsurances, value);
        }

        #endregion

        #region Props

        public bool FirstCardOpened
        {
            get { return this._firstCardOpened; }
            set { SetProperty(ref this._firstCardOpened, value); }
        }

        public bool SecondCardOpened
        {
            get { return this._secondCardOpened; }
            set { SetProperty(ref this._secondCardOpened, value); }
        }

        public bool ThirdCardOpened
        {
            get { return this._thirdCardOpened; }
            set { SetProperty(ref this._thirdCardOpened, value); }
        }

        public string FirstCardArrow
        {
            get { return this._firstCardArrow; }
            set { SetProperty(ref this._firstCardArrow, value); }
        }

        public string SecondCardArrow
        {
            get { return this._secondCardArrow; }
            set { SetProperty(ref this._secondCardArrow, value); }
        }

        public string ThirdCardArrow
        {
            get { return this._thirdCardArrow; }
            set { SetProperty(ref this._thirdCardArrow, value); }
        }

        public string ActiveInsuranceColor
        {
            get { return this._activeInsuranceColor; }
            set { SetProperty(ref this._activeInsuranceColor, value); }
        }

        public string ExpiredInsuranceColor
        {
            get { return this._expiredInsuranceColor; }
            set { SetProperty(ref this._expiredInsuranceColor, value); }
        }

        public string SelectedCountryShortCode
        {
            get { return _selectedCountryShortCode; }
            set { SetProperty(ref _selectedCountryShortCode, value); }
        }
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }
        #endregion

        #region Commands

        [RelayCommand]
        void CardToggle(string toggleName)
        {
            switch (toggleName)
            {
                case "first":
                    this.FirstCardOpened = !this.FirstCardOpened;
                    this.FirstCardArrow = this.FirstCardOpened ? GlobalConstants.ArrowImgLess : GlobalConstants.ArrowImgMore;

                    // Close other cards
                    this.SecondCardOpened = false;
                    this.SecondCardArrow = GlobalConstants.ArrowImgMore;
                    this.ThirdCardOpened = false;
                    this.ThirdCardArrow = GlobalConstants.ArrowImgMore;
                    break;

                case "second":
                    this.SecondCardOpened = !this.SecondCardOpened;
                    this.SecondCardArrow = this.SecondCardOpened ? GlobalConstants.ArrowImgLess : GlobalConstants.ArrowImgMore;

                    // Close other cards
                    this.FirstCardOpened = false;
                    this.FirstCardArrow = GlobalConstants.ArrowImgMore;
                    this.ThirdCardOpened = false;
                    this.ThirdCardArrow = GlobalConstants.ArrowImgMore;
                    break;

                case "third":
                    this.ThirdCardOpened = !this.ThirdCardOpened;
                    this.ThirdCardArrow = this.ThirdCardOpened ? GlobalConstants.ArrowImgLess : GlobalConstants.ArrowImgMore;

                    // Close other cards
                    this.FirstCardOpened = false;
                    this.FirstCardArrow = GlobalConstants.ArrowImgMore;
                    this.SecondCardOpened = false;
                    this.SecondCardArrow = GlobalConstants.ArrowImgMore;
                    break;

                default:
                    this.FirstCardOpened = false;
                    this.FirstCardArrow = GlobalConstants.ArrowImgMore;
                    this.SecondCardOpened = false;
                    this.SecondCardArrow = GlobalConstants.ArrowImgMore;
                    this.ThirdCardOpened = false;
                    this.ThirdCardArrow = GlobalConstants.ArrowImgMore;
                    break;
            }
        }

        [RelayCommand]
        private async void GoToCivilInsurancePage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                var parameters = new Dictionary<string, object>
                {
                    {   "insuranceType", InsuranceTypeEnum.Civil }
                };
                await Navigation.PushAsync<ScanOrFillVehicleDataPage>(parameters);

            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }

        [RelayCommand]
        private async void GoToTravelInsurancePage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();
                await Navigation.PushAsync<TravelInsuranceLocationPage>();
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }

        [RelayCommand]
        private async void GoToMountainInsurancePage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();
                await Navigation.PushAsync<MountainInsuranceInsuredUsersPage>();
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }

        [RelayCommand]
        private async void GoToHealthInsurancePage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();
                await Navigation.PushAsync<HealthInsuranceInsuredUsersPage>();
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }

        [RelayCommand]
        private async void GoToMyThingsPage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();
                await Navigation.PushAsync<MyThingsInsuranceCategoryPage>();
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }

        [RelayCommand]
        private async void GoToRegisterPage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();
                await Navigation.PushAsync<RegisterPage>();
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }

        [RelayCommand]
        private async void GoToMyInsurances()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();
                await Navigation.PushAsync<MyInsurancesPage>();
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }


        [RelayCommand]
        private async void GoToCascoInsurancePage()
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                var parameters = new Dictionary<string, object>
                {
                    {   "insuranceType", InsuranceTypeEnum.Casco }
                };
                await Navigation.PushAsync<ScanOrFillVehicleDataPage>(parameters);
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }

        [RelayCommand]
        private async void InsurancePay(MyInsurancesModel input)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                if (input.InstallmentToPay != 0)
                {
                    await this.PayInstallment(input.InsuranceId, input.InstallmentToPay);
                }
                else
                {
                    await this.CreateNewInsurance(input.InsuranceId);
                }

            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }

        [RelayCommand]
        private async void DownloadPolicy(MyInsurancesModel policy)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                var httpClient = HttpClientFactory.Create();
                string jwt = App.JwtToken;

                var stream = await this._insuranceService.GetPolicyPdf(App.UserId, policy.InsuranceId, policy.PolicyNo, jwt, httpClient);

                string policyNo = policy.PolicyNo.Replace('/', '_');
                string fileName = $"{policyNo}.pdf";
                string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

                using (var fileStream = File.Create(filePath))
                {
                    stream.CopyTo(fileStream);
                }

                httpClient.Dispose();

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath)
                });
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }

        [RelayCommand]
        private async void DownloadReceipt(MyInsurancesModel policy)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                var httpClient = HttpClientFactory.Create();
                string jwt = App.JwtToken;

                var stream = await this._insuranceService.GetReceiptPdf(App.UserId, policy.InsuranceId, policy.PolicyNo, jwt, httpClient);

                string policyNo = policy.PolicyNo.Replace('/', '_');
                string fileName = $"{policyNo}.pdf";
                string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

                using (var fileStream = File.Create(filePath))
                {
                    stream.CopyTo(fileStream);
                }

                httpClient.Dispose();

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath)
                });
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }

        [RelayCommand]
        private async void DownloadGreenCard(MyInsurancesModel policy)
        {
            try
            {
                if (IsBusy)
                {
                    return;
                }

                IsBusy = true;

                ShowLoading();

                var httpClient = HttpClientFactory.Create();
                string jwt = App.JwtToken;

                var stream = await this._insuranceService.GetCivilInsuranceGreenCardPdf(App.UserId, policy.InsuranceId, policy.PolicyNo, jwt, httpClient);

                string policyNo = policy.PolicyNo.Replace('/', '_');
                string fileName = $"{policyNo}.pdf";
                string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

                using (var fileStream = File.Create(filePath))
                {
                    stream.CopyTo(fileStream);
                }

                httpClient.Dispose();

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath)
                });
            }
            catch (Exception ex)
            {
                await App.DisplayAlert(App.MESSAGE_HEADER_ERROR, ex.Message, App.MESSAGE_OK);
            }
            finally
            {
                IsBusy = false;
                HideLoading();
            }
        }

        #endregion

        #region Methods

        //private async Task GetCacheValues()
        //{
        //    try
        //    {
        //        var vehicleBrands = App.VehicleBrands;
        //        var regions = App.Regions;
        //        var countries = App.Countries;
        //        var userGuiltTypes = App.UserGuiltTypes;
        //        var vehicleColors = App.VehicleColors;
        //        var vehicleBodyTypes = App.VehicleBodyTypes;
        //        var vehicleEngineTypes = App.VehicleEngineTypes;

        //        var vehicleBrandsHashCode = vehicleBrands.GetHashCode();
        //        var regionsHashCode = regions.GetHashCode();
        //        var countriesHashCode = countries.GetHashCode();
        //        var userGuiltTypesHashCode = userGuiltTypes.GetHashCode();
        //        var vehicleColorsHashCode = vehicleColors.GetHashCode();
        //        var vehicleBodyTypesHashCode = vehicleBodyTypes.GetHashCode();
        //        var vehicleEngineTypesHashCode = vehicleEngineTypes.GetHashCode();

        //        var httpClient = HttpClientFactory.Create();
        //        var jwt = App.JwtToken;

        //        var cachedResult = await this._cacheService.GetValues(jwt,countriesHashCode, regionsHashCode, vehicleBrandsHashCode, userGuiltTypesHashCode,
        //            vehicleColorsHashCode, vehicleBodyTypesHashCode, vehicleEngineTypesHashCode,httpClient);

        //        if(cachedResult != null)
        //        {
        //            if (!string.IsNullOrEmpty(cachedResult.Regions))
        //            {
        //                App.Regions = cachedResult.Regions;
        //            }

        //            if (!string.IsNullOrEmpty(cachedResult.Countries))
        //            {
        //                App.Countries = cachedResult.Countries;
        //            }

        //            if (!string.IsNullOrEmpty(cachedResult.VehicleBrands))
        //            {
        //                App.VehicleBrands = cachedResult.VehicleBrands;
        //            }


        //            if (!string.IsNullOrEmpty(cachedResult.UserGuiltTypes))
        //            {
        //                App.UserGuiltTypes = cachedResult.UserGuiltTypes;
        //            }


        //            if (!string.IsNullOrEmpty(cachedResult.VehicleEngineTypes))
        //            {
        //                App.VehicleEngineTypes = cachedResult.VehicleEngineTypes;
        //            }


        //            if (!string.IsNullOrEmpty(cachedResult.VehicleBodyTypes))
        //            {
        //                App.VehicleBodyTypes = cachedResult.VehicleBodyTypes;
        //            }


        //            if (!string.IsNullOrEmpty(cachedResult.VehicleColors))
        //            {
        //                App.VehicleColors = cachedResult.VehicleColors;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return;
        //    }
        //}

        public async Task UserInsurances()
        {

            var jwt = App.JwtToken;
            var userId = App.UserId;
            var httpClient = HttpClientFactory.Create();

            var req = new UserInsurancesInput
            {
                UserId = userId
            };

            var insurances = await this._insuranceService.UserInsurances(req, jwt, httpClient);
            this._insurances = insurances;

            this.PopulateCollections(insurances);
        }

        private void PopulateCollections(List<InsuranceModel> insurances)
        {
            var collections = InsuranceHelper.PopulateInsuranceCollections(insurances);

            var active = collections[GlobalConstants.ActiveInsurance].OrderBy(x => x.ExpireDate).ToObservableCollection();
            var expired = collections[GlobalConstants.ExpiredInsurance].OrderBy(x => x.ExpireDate).ToObservableCollection();

            this.ExpiredInsurances.Clear();
            this.ActiveInsurances.Clear();

            if (expired.Count > 0)
            {
                this.ExpiredInsuranceColor = "#FFFFFF";
            }


            if (active.Count > 0)
            {
                this.ActiveInsuranceColor = "#FFFFFF";
            }

            this.ActiveInsurances = active.Take(4).ToObservableCollection();
            this.ExpiredInsurances = expired.Take(4).ToObservableCollection();
        }

        private async Task CreateNewInsurance(string insuranceId)
        {
            var insurance = this._insurances.FirstOrDefault(x => x.Id == insuranceId);

            if (insurance != null)
            {
                switch (insurance.Type)
                {
                    case (byte)InsuranceTypeEnum.Civil: await CreateCivilInsurance(insurance); break;
                    case (byte)InsuranceTypeEnum.Travel: await CreateTravelInsurance(insurance); break;
                    case (byte)InsuranceTypeEnum.Mountain: await CreateMountainInsurance(insurance); break;
                    case (byte)InsuranceTypeEnum.Health: await CreateHealthInsurance(insurance); break;
                    case (byte)InsuranceTypeEnum.MyThings: await CreateMyThingsInsurance(insurance); break;
                }
            }

        }

        private async Task PayInstallment(string insuranceId, int installmentToPay)
        {
            var insurance = this._insurances.FirstOrDefault(x => x.Id == insuranceId);

            if (insurance != null)
            {
                switch (insurance.Type)
                {
                    case (byte)InsuranceTypeEnum.Civil: await PayCivilInstallment(insurance, installmentToPay); break;
                    case (byte)InsuranceTypeEnum.Health: await PayHealthInstallment(insurance, installmentToPay); break;
                }
            }
        }

        private async Task PayHealthInstallment(InsuranceModel insurance, int installmentToPay)
        {
            decimal amount = GetInstallmentAmount(insurance, installmentToPay);

            var insurerInfo = new InsuranceUserInfoModel();

            string insuredUserId = insurance.InsuredUsers.Where(x => x.IsInsurer == true).Select(x => x.UserId).FirstOrDefault();

            if (!string.IsNullOrEmpty(insuredUserId))
            {
                insurerInfo = await this.PopulateInsuranceUserInfo(insuredUserId);
            }

            var selectedOffer = new InsuranceOfferModel
            {
                PolicyNo = insurance.PolicyNo,
                InsuranceId = insurance.Id,
                UserId = string.IsNullOrEmpty(insurerInfo.UserId) ? App.UserId : insurerInfo.UserId,
                NextInstallment = true,
                InsuranceType = insurance.Type,
                Price = amount,
                PriceFormatted = string.Format("{0:0.00}", amount) + " лв.",
                CompanyName = insurance.InsuranceCompany.CompanyName,
                CompanyLogo = Helpers.GetImageSrc(insurance.InsuranceCompany.CompanyName),
                InstallmentToPay = installmentToPay
            };

            var parameters = new Dictionary<string, object>
                    {
                        {"selectedOffer", selectedOffer },
                    };

            string vehicleId = insurance.Vehicle.Id;
            InsuranceVehicleInfo vehicleInfo = new InsuranceVehicleInfo();

            parameters.Add("userInfo", insurerInfo);

            await Navigation.PushAsync<CivilInsuranceDocumentsPage>(parameters);
        }

        private async Task PayCivilInstallment(InsuranceModel insurance, int installmentToPay)
        {
            decimal amount = GetInstallmentAmount(insurance, installmentToPay);

            var insurerInfo = new InsuranceUserInfoModel();

            string insuredUserId = insurance.InsuredUsers.Where(x => x.IsInsurer == true).Select(x => x.UserId).FirstOrDefault();

            if (!string.IsNullOrEmpty(insuredUserId))
            {
                insurerInfo = await this.PopulateInsuranceUserInfo(insuredUserId);
            }

            var selectedOffer = new InsuranceOfferModel
            {
                PolicyNo = insurance.PolicyNo,
                InsuranceId = insurance.Id,
                UserId = string.IsNullOrEmpty(insurerInfo.UserId) ? App.UserId : insurerInfo.UserId,
                NextInstallment = true,
                InsuranceType = insurance.Type,
                Price = amount,
                PriceFormatted = string.Format("{0:0.00}", amount) + " лв.",
                CompanyName = insurance.InsuranceCompany.CompanyName,
                CompanyLogo = Helpers.GetImageSrc(insurance.InsuranceCompany.CompanyName),
                InstallmentToPay = installmentToPay
            };

            var parameters = new Dictionary<string, object>
                    {
                        {"selectedOffer", selectedOffer },
                    };

            string vehicleId = insurance.Vehicle.Id;
            InsuranceVehicleInfo vehicleInfo = new InsuranceVehicleInfo();

            if (!string.IsNullOrEmpty(vehicleId))
            {
                vehicleInfo = await this.PopulateInsuranceVehicleInfo(insurance.Vehicle.Id);
            }
            else
            {
                vehicleInfo.PlateNumber = insurance.Vehicle.PlateNumber;
                vehicleInfo.VehicleBrand = insurance.Vehicle.Brand;
                vehicleInfo.VehicleModel = insurance.Vehicle.Model;
            }

            parameters.Add("userVehicleInfo", vehicleInfo);
            parameters.Add("ownerUserInfo", insurerInfo);

            var usualDriver = insurance.InsuredUsers.FirstOrDefault(x => x.IsUsualDriver);

            if (usualDriver != null)
            {
                var usualDriverInfo = await this.PopulateInsuranceUserInfo(usualDriver.UserId);
                parameters.Add("usualUserInfo", usualDriverInfo);
            }
            await Navigation.PushAsync<CivilInsuranceDocumentsPage>(parameters);
        }

        private async Task CreateCivilInsurance(InsuranceModel insurance)
        {
            App.NewInsuranceStartDate = insurance.CurrentEndDate.AddDays(1);
            string insuredUserId = insurance.InsuredUsers.Where(x => x.IsInsurer == true).Select(x => x.UserId).FirstOrDefault();

            InsuranceUserInfoModel insurerInfo = new InsuranceUserInfoModel();
            InsuranceVehicleInfo vehicleInfo = new InsuranceVehicleInfo();
            bool isValidVehicleInfo = false;
            bool isValidUserInfo = false;

            if (!string.IsNullOrEmpty(insuredUserId))
            {
                insurerInfo = await this.PopulateInsuranceUserInfo(insuredUserId);
                isValidUserInfo = Helpers.ValidateInsuranceUserInfo(insurerInfo);
            }

            if (!string.IsNullOrEmpty(insurance.Vehicle.Id))
            {
                vehicleInfo = await this.PopulateInsuranceVehicleInfo(insurance.Vehicle.Id);
                isValidVehicleInfo = Helpers.ValidateInsuranceVehicleInfo(vehicleInfo);
            }

            if (isValidUserInfo && isValidVehicleInfo)
            {
                var parameters = new Dictionary<string, object>
                    {
                        {"ownerUserInfo", insurerInfo },
                        {"userVehicleInfo", vehicleInfo }
                    };

                var usualDriver = insurance.InsuredUsers.FirstOrDefault(x => x.IsUsualDriver);

                if (usualDriver != null)
                {
                    var usualDriverInfo = await this.PopulateInsuranceUserInfo(usualDriver.UserId);
                    parameters.Add("usualUserInfo", usualDriverInfo);
                }

                await Navigation.PushAsync<CivilInsuranceOffersPage>(parameters);
            }
            else
            {
                var emptyParams = new Dictionary<string, object>();
                await Navigation.PushAsync<CivilInsuranceVehiclePage>(emptyParams);
            }
        }
        private async Task CreateTravelInsurance(InsuranceModel insurance)
        {
            if (insurance.TravelInsurance != null)
            {
                var territorialValidity = insurance.TravelInsurance.TerritorialValidity;
                var travelPurpose = insurance.TravelInsurance.TravelPurpose;

                var insuredUsers = insurance.InsuredUsers.Where(x => x.IsInsured == true).ToList();

                var userAge = this.CalculateInsuredUsersAge(insuredUsers);
                var travelInfo = new TravelInsuranceOfferModel
                {
                    TripPurpose = travelPurpose,
                    Territory = territorialValidity
                };

                var parameters = new Dictionary<string, object>
                    {
                        {"travelInfo", travelInfo},
                        {"userAgeList", userAge}
                    };

                await Navigation.PushAsync<TravelInsuranceCalendarPage>(parameters);
            }
        }
        private async Task CreateMountainInsurance(InsuranceModel insurance)
        {
            if (insurance.MountainInsurance != null)
            {
                var isExtreme = insurance.MountainInsurance.IsExtreme;

                var insuredUsers = insurance.InsuredUsers.Where(x => x.IsInsured == true).ToList();

                var userAge = this.CalculateInsuredUsersAge(insuredUsers);

                var mountainInfo = new MountainInsuranceOfferModel
                {
                    IsExtreme = isExtreme
                };

                var parameters = new Dictionary<string, object>
                    {
                        {"mountainInfo", mountainInfo},
                        {"userAgeList", userAge}
                    };

                await Navigation.PushAsync<MountainInsuranceCalendarPage>(parameters);
            }
        }
        private async Task CreateHealthInsurance(InsuranceModel insurance)
        {
            if (insurance.HealthInsurance != null)
            {
                var isFamily = insurance.HealthInsurance.IsFamily;
                var packageType = insurance.HealthInsurance.PackageType;

                var insuredUsers = insurance.InsuredUsers.Where(x => x.IsInsured == true).ToList();

                var userAge = this.CalculateInsuredUsersAge(insuredUsers);

                var healthInfo = new HealthInsuranceOfferModel
                {
                    IsFamily = isFamily,
                    PacketId = packageType
                };

                var parameters = new Dictionary<string, object>
                    {
                        {"healthInfo", healthInfo},
                        {"userAgeList", userAge}
                    };

                await Navigation.PushAsync<HealthInsurancePeriodPage>(parameters);
            }
        }
        private async Task CreateMyThingsInsurance(InsuranceModel insurance)
        {
            if (insurance.MyThingsInsurance != null)
            {
                var propertyTypeId = insurance.MyThingsInsurance.PropertyType;
                var objectTypeId = insurance.MyThingsInsurance.ObjectType;
                var insuranceSum = insurance.MyThingsInsurance.InsuranceSum;
                var brand = insurance.MyThingsInsurance.Brand;
                var model = insurance.MyThingsInsurance.Model;

                var questions = await this.GetMyThingsInsuranceQuestions(insurance.MyThingsInsurance.Id);

                var questionnaireModel = new QuestionnaireModel
                {
                    Questionnaire = questions
                };

                var myThingsInfo = new MyThingsInsuranceOfferModel
                {
                    PropertyTypeId = propertyTypeId,
                    ObjectTypeId = objectTypeId,
                    Questionnaire = questionnaireModel,
                    InsuranceSum = insuranceSum,
                    Brand = brand,
                    Model = model
                };

                var parameters = new Dictionary<string, object>
                    {
                        {"myThingsInfo", myThingsInfo}
                    };

                await Navigation.PushAsync<MyThingsInsuranceCalendarPage>(parameters);
            }

        }
        private decimal GetInstallmentAmount(InsuranceModel insurance, int installmentToPay)
        {
            decimal amount = 0;

            if (insurance.Type == (byte)InsuranceTypeEnum.Civil)
            {
                if (installmentToPay == 2)
                {
                    amount = insurance.CivilInsurance.SecondInstallmentPrice;
                }
                else if (installmentToPay == 3)
                {
                    amount = insurance.CivilInsurance.ThirdInstallmentPrice;
                }
                else if (installmentToPay == 4)
                {
                    amount = insurance.CivilInsurance.FourthInstallmentPrice;
                }
            }

            if (insurance.Type == (byte)InsuranceTypeEnum.Health)
            {

                amount = insurance.HealthInsurance.InstallmentPrice;
            }

            return amount;

        }
        private async Task<UserModel> GetUserInfo(string userId)
        {
            var jwt = App.JwtToken;
            var httpClient = HttpClientFactory.Create();

            var result = await this._userService.GetUserById(userId, httpClient, jwt);

            return result;
        }
        private async Task<InsuranceUserInfoModel> PopulateInsuranceUserInfo(string userId)
        {
            var info = new InsuranceUserInfoModel();
            var user = await this.GetUserInfo(userId);

            if (user != null)
            {
                info = new InsuranceUserInfoModel
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    SurName = user.SurName,
                    Uin = user.UiNumber,
                    VatNumber = user.VatNumber,
                    CompanyName = user.CompanyName,
                    Address = user.Address,
                    BirthDateString = user.BirthDate.HasValue ? user.BirthDate.Value.ToString("dd.MM.yyyy") : string.Empty,
                    BirthDate = user.BirthDate.HasValue ? user.BirthDate.Value : DateTime.Now,
                    RegionCode = user.RegionId,
                    MunicipalityCode = user.MunicipalityId,
                    PostalCode = user.PostCode,
                    CityCode = user.CityId,
                    CountryCode = user.CountryId,
                    DrivingExperiance = user.DrivingExperience.Value,
                    RegionName = !string.IsNullOrEmpty(user.RegionId) ? await this.GetRegionName(user.RegionId) : string.Empty,
                    MunicipalityName = await this.GetMunicipalityName(user.RegionId, user.MunicipalityId),
                    CityName = await this.GetCityName(user.MunicipalityId, user.CityId)
                };
            }

            return info;
        }
        private async Task<InsuranceVehicleInfo> PopulateInsuranceVehicleInfo(string vehicleId)
        {
            var result = new InsuranceVehicleInfo();

            var jwt = App.JwtToken;
            var httpClient = HttpClientFactory.Create();

            var vehicle = await this._vehicleService.GetVehicleById(vehicleId, httpClient, jwt);

            if (vehicle != null && !string.IsNullOrEmpty(vehicle.Id))
            {
                result.PlateNumber = vehicle.PlateNumber;
                result.RegistrationCertificateNumber = vehicle.RegistrationCertificateNumber;
                result.VehicleBrand = vehicle.Brand;
                result.VehicleModel = vehicle.Model;
                result.VehicleModelId = vehicle.ModelId;
                result.VehicleBrandId = vehicle.BrandId;
                result.FirstRegistrationDate = vehicle.FirstRegistrationDate.HasValue ? vehicle.FirstRegistrationDate.Value.ToString("dd.MM.yyyy") : null;
                result.VehicleType = vehicle.VehicleType;
                result.VehicleTypeId = vehicle.VehicleTypeId;
                result.VehicleUsage = vehicle.VehicleUsage;
                result.VehicleUsageId = vehicle.VehicleUsageId;
                result.Vin = vehicle.Vin;
                result.VehicleKilowatts = vehicle.VehicleKilowatts;
                result.EngineVolume = vehicle.EngineVolume;
                result.BodyType = vehicle.BodyType;
                result.Places = vehicle.Places;
                result.Color = vehicle.Color;
                result.EngineType = vehicle.EngineType;
                result.NetWeight = vehicle.NetWeight;
                result.GrossWeight = vehicle.GrossWeight;
                result.SteeringWheel = vehicle.SteeringWheel;
            }

            return result;
        }
        private List<InsuredUsersAgeTemplateModel> CalculateInsuredUsersAge(List<InsuredUserModel> insuredUsers)
        {
            var result = new List<InsuredUsersAgeTemplateModel>();

            foreach (var user in insuredUsers)
            {
                var birthDate = user.BirthDate.Value;
                var today = DateTime.Today;

                var age = today.Year - birthDate.Year;
                if (birthDate.Date > today.AddYears(-age)) age--;

                int number = 1;
                int index = 0;

                var current = new InsuredUsersAgeTemplateModel
                {
                    Age = age.ToString(),
                    Number = number,
                    Index = index
                };

                result.Add(current);
                number++;
                index++;
            }

            return result;
        }
        private async Task<List<QuestionModel>> GetMyThingsInsuranceQuestions(string myThingsInsuranceId)
        {
            var httpClient = HttpClientFactory.Create();
            var jwt = App.JwtToken;

            var questions = await this._insuranceService.GetMyThingsInsuranceQuestions(myThingsInsuranceId, jwt, httpClient);

            return questions;
        }
        private async Task<string> GetRegionName(string regionId)
        {
            string regionName = string.Empty;

            var regionsJson = await AssetsHelper.LoadAssets(GlobalConstants.Regions);
            var regions = JsonConvert.DeserializeObject<GetRegionsResultModel>(regionsJson);
            var region = regions.Regions.FirstOrDefault(x => x.Id == regionId);

            if (region != null)
            {
                regionName = region.Name;
            }

            return regionName;
        }
        public async Task<string> GetMunicipalityName(string regionId, string municipalityId)
        {
            if (!string.IsNullOrEmpty(regionId) && !string.IsNullOrEmpty(municipalityId))
            {
                string municipalityName = string.Empty;

                string munUrl = $"{GlobalConstants.Municipalities}{regionId}.json";

                var municipalitiesJson = await AssetsHelper.LoadAssets(munUrl);
                var municipalities = JsonConvert.DeserializeObject<GetMunicipalityResultModel>(municipalitiesJson);
                var municiplality = municipalities.Municipalities.FirstOrDefault(x => x.Id == municipalityId);

                if (municiplality != null)
                {
                    municipalityName = municiplality.Name;
                }

                return municipalityName;
            }
            else
            {
                return null;
            }

        }
        public async Task<string> GetCityName(string municipalityId, string cityId)
        {
            if (!string.IsNullOrEmpty(municipalityId) && !string.IsNullOrEmpty(cityId))
            {
                string cityName = string.Empty;
                string cityUrl = $"{GlobalConstants.Cities}{municipalityId}.json";

                var citiesJson = await AssetsHelper.LoadAssets(cityUrl);
                var cities = JsonConvert.DeserializeObject<GetCityResultModel>(citiesJson);
                var city = cities.Cities.FirstOrDefault(x => x.Id == cityId);

                if (city != null)
                {
                    cityName = city.Name;
                }

                return cityName;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
