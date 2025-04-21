using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace CovrMe.ViewModels.ContentViews
{
    public class CameraGuidePopUpViewModel : BaseViewModel
    {
        private readonly Popup _popupInstance;
        private readonly bool _isFront;

        public CameraGuidePopUpViewModel(bool isFront, Popup popupInstance)
        {
            _isFront = isFront;
            _popupInstance = popupInstance;

            InitializeContent();
            OpenCameraCommand = new Command(OpenCamera);
        }

        private string _currentImage;
        public string CurrentImage
        {
            get => _currentImage;
            set => SetProperty(ref _currentImage, value);
        }

        private string _guideText;
        public string GuideText
        {
            get => _guideText;
            set => SetProperty(ref _guideText, value);
        }

        public ICommand OpenCameraCommand { get; }

        private void InitializeContent()
        {
            if (_isFront)
            {
                CurrentImage = "backdoc.png";
                GuideText = "Cнимайте страната с информация\nза колата.";
            }
            else
            {
                CurrentImage = "frontdoc.png";
                GuideText = "Снимайте страната с информация\nза човека.";
            }
        }

        private void OpenCamera()
        {
            _popupInstance.Close();
        }
    }
}
