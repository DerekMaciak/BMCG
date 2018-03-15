using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.Permissions;
using System;

namespace BMCGMobile.Droid
{
    [Activity(Label = "Bloomfield Greenway", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private StepCounterServiceBinder _Binder;
        private bool _Registered;
        public bool IsBound { get; set; }
        private StepCounterServiceConnection _ServiceConnection;

        public StepCounterServiceBinder Binder
        {
            get { return _Binder; }
            set
            {
                _Binder = value;
                if (_Binder == null)
                    return;

                _Registered = true;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsGoogleMaps.Init(this, bundle); // initialize for Xamarin.Forms.GoogleMaps
            LoadApplication(new App());

        _StartStepCounterService();
        }

        protected override void OnResume()
        {
            base.OnResume();
            //CrashManager.Register(this, "4d7260837e894ed2bc5ae5851c1b325");
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void _StartStepCounterService()
        {
            try
            {
                var service = new Intent(this, typeof(StepCounterService));
                var componentName = StartService(service);
            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            if (IsBound)
            {
                try
                {
                    UnbindService(_ServiceConnection);
                    IsBound = false;
                }
                catch (Exception ex)
                {
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (IsBound)
            {
                try
                {
                    UnbindService(_ServiceConnection);
                    IsBound = false;
                }
                catch (Exception ex)
                {
                }
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            //if (!Utils.IsKitKatWithStepCounter(PackageManager))
            //{
            //    Console.WriteLine("Not compatible with sensors, stopping service.");
            //    return;
            //}

            // if (!firstRun)
            try
            {
                _StartStepCounterService();

                if (IsBound)
                    return;

                var serviceIntent = new Intent(this, typeof(StepCounterService));
                _ServiceConnection = new StepCounterServiceConnection(this);
                BindService(serviceIntent, _ServiceConnection, Bind.AutoCreate);
            }
            catch (Exception ex)
            {
            }
        }
    }
}