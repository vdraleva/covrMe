using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Microsoft.Maui.Platform;
using static Android.Views.View;

namespace CovrMe
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : MauiAppCompatActivity, IOnTouchListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.DecorView.ViewTreeObserver.GlobalFocusChange += FocusChanged;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void FocusChanged(object sender, ViewTreeObserver.GlobalFocusChangeEventArgs e)
        {
            if (e.OldFocus is not null)
            {
                e.OldFocus.SetOnTouchListener(null);
            }

            if (e.NewFocus is not null)
            {
                e.NewFocus.SetOnTouchListener(this);
            }


            if (e.NewFocus is null && e.OldFocus is not null)
            {
                InputMethodManager imm = InputMethodManager.FromContext(this);

                IBinder wt = e.OldFocus.WindowToken;

                if (imm is null || wt is null)
                    return;

                imm.HideSoftInputFromWindow(wt, HideSoftInputFlags.None);
            }
        }
        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            bool dispatch = base.DispatchTouchEvent(ev);

            if (ev.Action == MotionEventActions.Down && CurrentFocus is not null && CurrentFocus is not MauiPickerBase)
            {
                if (!KeepFocus)
                    CurrentFocus.ClearFocus();
                KeepFocus = false;
            }

            return dispatch;
        }

        bool KeepFocus { get; set; }

        public bool OnTouch(Android.Views.View v, MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down && CurrentFocus == v)
                KeepFocus = true;

            return v.OnTouchEvent(e);
        }
    }
}
