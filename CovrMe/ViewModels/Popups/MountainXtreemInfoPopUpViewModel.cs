using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.ViewModels.Popups
{
    public partial class MountainXtreemInfoPopUpViewModel : BaseViewModel
    {
        private double _displayWidth;

        public MountainXtreemInfoPopUpViewModel()
        {
             SetScreenWidth();
        }

        #region Props

        public double DisplayWidth
        {
            get { return this._displayWidth; }
            set { SetProperty(ref this._displayWidth, value); }
        }

        #endregion

        private void SetScreenWidth()
        {
            this.DisplayWidth = (DeviceDisplay.MainDisplayInfo.Width / 2) - 200;
        }
    }
}
