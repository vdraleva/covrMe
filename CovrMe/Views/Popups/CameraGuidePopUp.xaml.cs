using CommunityToolkit.Maui.Views;
using CovrMe.ViewModels.ContentViews;

namespace CovrMe.Views.ContentViews
{
    public partial class CameraGuidePopUp : Popup
    {
        public CameraGuidePopUp(bool isFront)
        {
            InitializeComponent();
            BindingContext = new CameraGuidePopUpViewModel(isFront, this);
        }
    }
}
