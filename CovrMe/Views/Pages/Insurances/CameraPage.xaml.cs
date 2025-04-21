using CommunityToolkit.Maui.Views;
using Controls.UserDialogs.Maui;
using Dynamsoft.CameraEnhancer.Maui;
using Dynamsoft.CaptureVisionRouter.Maui;
using Dynamsoft.Core.Maui;
using Dynamsoft.DocumentNormalizer.Maui;
using Dynamsoft.Utility.Maui;
using CovrMe.Models.Vehicles.Result;
using CovrMe.Services.Contracts;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using CovrMe.Views.ContentViews;
using CovrMe.Views.Pages.Insurances.Casco;
using CovrMe.Views.Pages.Insurances.CivilInsurance;
using Maui.Plugins.PageResolver;
using System.ComponentModel;

namespace CovrMe.Views.Pages.Insurances;

public partial class CameraPage : ContentPage, ICapturedResultReceiver, ICompletionListener, INotifyPropertyChanged
{
    private RegCertificateResultModel regCertificateModel;
    private HudDialogConfig hudConfig = new HudDialogConfig
    {
        Message = MessageConstants.FocusDocument,
        BackgroundColor = Colors.Black.WithAlpha(0.0f),
        Image = "scanner.png",
        MaskType = MaskType.Black,
        MessageFontSize = 16
    };
    private InsuranceTypeEnum insuranceType;
    private DocumentTypeEnum documentType;
    private IRegExtractorRegexService regRegexService;
    private bool isModelEmpty = true;
    private bool isPopupOpen = false;
    public static CameraEnhancer enhancer;
    CaptureVisionRouter router;

    public CameraPage(IOcrService os, IRegExtractorRegexService vs, Dictionary<string, object> query)
    {
        InitializeComponent();
        regCertificateModel = new RegCertificateResultModel();
        enhancer = new CameraEnhancer();
        router = new CaptureVisionRouter();
        router.SetInput(enhancer);
        router.AddResultReceiver(this);
        var filter = new MultiFrameResultCrossFilter();
        regRegexService = vs;
        this.insuranceType = (InsuranceTypeEnum)query.FirstOrDefault(x => x.Key == "insuranceType").Value;
        this.documentType = (DocumentTypeEnum)query.FirstOrDefault(x => x.Key == "documentType").Value;
        var inputModel = (RegCertificateResultModel)query.FirstOrDefault(x => x.Key == "ocrRegCertificateModel").Value;
        CombineRegCertificateModels(inputModel);
        isModelEmpty = regRegexService.IsPhotoBad(regCertificateModel, documentType);
    }


    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        if (this.Handler != null)
        {
            enhancer.SetCameraView(camera);
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        isReady = false;
        await Permissions.RequestAsync<Permissions.Camera>();
        enhancer?.Open();
        router?.StartCapturing(EnumPresetTemplate.PT_DETECT_AND_NORMALIZE_DOCUMENT, this);

        if (!isPopupOpen && documentType == DocumentTypeEnum.Small)
        {
            var isFront = regRegexService.IsFront(regCertificateModel);
            var guidePopup = new CameraGuidePopUp(isFront);
            isPopupOpen = true;

            guidePopup.Closed += (s, e) =>
            {
                isPopupOpen = false;
                _ = ShowHint();
            };

            this.ShowPopup(guidePopup);
        }

        if (documentType == DocumentTypeEnum.Big)
        {
            await ShowHint();
        }
    }

    protected override async void OnDisappearing()
    {
        await HideHud();
        base.OnDisappearing();
        enhancer?.Close();
        router?.StopCapturing();
    }

    bool isReady = false;

    public async void OnNormalizedImagesReceived(NormalizedImagesResult result)
    {
        if (result.Items != null)
        {
            if (result.Items[0].ImageData.Height < 600)
            {
                await ShowHint();
                isReady = false;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                return;
            }
            await HideHud();

            if (isReady)
            {
                ShowLoading();
                router?.StopCapturing();
                enhancer?.ClearBuffer();

                var imageSource = result.Items[0].ImageData.ToImageSource();

                if (imageSource is StreamImageSource streamImageSource)
                {
                    var stream = await streamImageSource.Stream(CancellationToken.None);
                    var tempFile = Path.Combine(FileSystem.CacheDirectory, "shared_image.jpg");

                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        await File.WriteAllBytesAsync(tempFile, memoryStream.ToArray());
                    }

                    var bytesImageData = File.ReadAllBytes(tempFile);

                    var IsOcrRequestTimeExpired = false;
                    RegCertificateResultModel newModel = new RegCertificateResultModel();
                    try
                    {
                        newModel = await regRegexService.ParseToModel(bytesImageData, documentType);
                    }
                    catch (TaskCanceledException)
                    {
                        IsOcrRequestTimeExpired = true;
                        await GoBackToScanPage(MessageConstants.OcrRequestTimeExpired);
                    }
                    catch (Exception ex)
                    {
                        await GoBackToScanPage(ex.Message);
                    }

                    var isPhotoBad = regRegexService.IsPhotoBad(newModel, documentType);
                    if (isPhotoBad && !IsOcrRequestTimeExpired)
                    {
                        await GoBackToScanPage(MessageConstants.BadPhoto);
                    }
                    else if (!isPhotoBad && !IsOcrRequestTimeExpired)
                    {
                        var parameters = new Dictionary<string, object>();
                        parameters.Add("insuranceType", insuranceType);
                        parameters.Add("documentType", documentType);
                        
                        if (isModelEmpty && documentType == DocumentTypeEnum.Small)
                        {
                            parameters.Add("ocrRegCertificateModel", newModel);
                            MainThread.BeginInvokeOnMainThread(async () =>
                            {
                                await Navigation.PushAsync<CameraPage>(parameters);
                            });
                        }
                        else if (regRegexService.IsFront(newModel) == regRegexService.IsFront(regCertificateModel) && documentType == DocumentTypeEnum.Small)
                        {
                            await GoBackToScanPage(MessageConstants.SameSide);
                        }
                        else
                        {
                            CombineRegCertificateModels(newModel);

                            parameters.Add("ocrRegCertificateModel", regCertificateModel);
                            if (insuranceType == InsuranceTypeEnum.Civil)
                            {
                                bool hasCivilPage = this.Navigation.NavigationStack.Any(x => x is CivilInsuranceVehiclePage);

                                if (hasCivilPage)
                                {
                                    this.Navigation.RemovePage(this.Navigation.NavigationStack.FirstOrDefault(x => !(x is null) && (x is CivilInsuranceVehiclePage)));
                                }

                                MainThread.BeginInvokeOnMainThread(async () =>
                                {
                                    await Navigation.PushAsync<CivilInsuranceVehiclePage>(parameters);
                                });
                            }

                            if (insuranceType == InsuranceTypeEnum.Casco)
                            {
                                bool hasCascoPage = this.Navigation.NavigationStack.Any(x => x is CascoInsuranceVehiclePage);

                                if (hasCascoPage)
                                {
                                    this.Navigation.RemovePage(this.Navigation.NavigationStack.FirstOrDefault(x => !(x is null) && (x is CascoInsuranceVehiclePage)));
                                }

                                MainThread.BeginInvokeOnMainThread(async () =>
                                {
                                    await Navigation.PushAsync<CascoInsuranceVehiclePage>(parameters);
                                });
                            }
                        }
                    }
                }
                await HideHud();
            }
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }


    private void OnCaptureButtonClicked(object sender, EventArgs e)
    {
        isReady = true;
    }

    public void OnFailure(int errorCode, string errorMessage)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            DisplayAlert("Error", errorMessage, "OK");
        });
    }

    protected async Task ShowHint()
    {
        if (isPopupOpen)
        {
            return; 
        }
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            UserDialogs.Instance.CreateOrUpdateHud(hudConfig);
        });
    }

    protected static void ShowLoading(string message = null)
    {
        string msg = "���������";
        UserDialogs.Instance.ShowLoading(msg);
    }
    protected async static Task HideHud()
    {
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            UserDialogs.Instance.HideHud();
        });
    }

    public void CombineRegCertificateModels(RegCertificateResultModel inputModel)
    {
        if (inputModel == null) return;

        regCertificateModel.RegistrationCertificateNumber ??= inputModel.RegistrationCertificateNumber;
        regCertificateModel.PlateNumber ??= inputModel.PlateNumber;
        regCertificateModel.FirstRegistrationDate ??= inputModel.FirstRegistrationDate;
        regCertificateModel.Model ??= inputModel.Model;
        regCertificateModel.Brand ??= inputModel.Brand;
        regCertificateModel.VehicleType ??= inputModel.VehicleType;
        regCertificateModel.Vin ??= inputModel.Vin;
        regCertificateModel.EngineVolume ??= inputModel.EngineVolume;
        regCertificateModel.BodyType ??= inputModel.BodyType;
        regCertificateModel.Color ??= inputModel.Color;
        regCertificateModel.EngineType ??= inputModel.EngineType;
        regCertificateModel.NetWeight ??= inputModel.NetWeight;
        regCertificateModel.GrossWeight ??= inputModel.GrossWeight;
        regCertificateModel.VehicleKilowatts ??= inputModel.VehicleKilowatts;
        regCertificateModel.SteeringWheel ??= inputModel.SteeringWheel;
        regCertificateModel.Places ??= inputModel.Places;
        regCertificateModel.FirstName ??= inputModel.FirstName;
        regCertificateModel.Surname ??= inputModel.Surname;
        regCertificateModel.LastName ??= inputModel.LastName;
        regCertificateModel.Uin ??= inputModel.Uin;
        regCertificateModel.Region ??= inputModel.Region;
        regCertificateModel.Municipality ??= inputModel.Municipality;
        regCertificateModel.City ??= inputModel.City;
        regCertificateModel.Address ??= inputModel.Address;
    }

    public async Task GoBackToScanPage(string displayMessage)
    {
        bool hasScanPage = this.Navigation.NavigationStack.Any(x => x is ScanOrFillVehicleDataPage);

        if (hasScanPage)
        {
            this.Navigation.RemovePage(this.Navigation.NavigationStack.FirstOrDefault(x => !(x is null) && (x is ScanOrFillVehicleDataPage)));
        }

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var parameters = new Dictionary<string, object>
                            {
                                { "insuranceType", insuranceType }
                            };
            await Navigation.PushAsync<ScanOrFillVehicleDataPage>(parameters);
            await DisplayAlert(App.MESSAGE_HEADER_ERROR, displayMessage, App.MESSAGE_OK);
        });
    }
}