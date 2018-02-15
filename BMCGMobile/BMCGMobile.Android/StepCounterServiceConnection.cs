using Android.Content;
using Android.OS;

namespace BMCGMobile.Droid
{
    public class StepCounterServiceConnection : Java.Lang.Object, IServiceConnection
    {
        private MainActivity _Activity;

        public StepCounterServiceConnection(MainActivity activity)
        {
            this._Activity = activity;
        }

        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            var serviceBinder = service as StepCounterServiceBinder;
            if (serviceBinder != null)
            {
                _Activity.Binder = serviceBinder;
                _Activity.IsBound = true;
            }
        }

        public void OnServiceDisconnected(ComponentName name)
        {
            _Activity.IsBound = false;
        }
    }
}