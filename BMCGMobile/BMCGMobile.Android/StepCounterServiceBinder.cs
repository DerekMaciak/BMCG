using Android.OS;

namespace BMCGMobile.Droid
{
    public class StepCounterServiceBinder : Binder
    {
        private StepCounterService _StepCounterService;

        public StepCounterServiceBinder(StepCounterService service)
        {
            this._StepCounterService = service;
        }

        public StepCounterService StepCounterService
        {
            get { return _StepCounterService; }
        }
    }
}