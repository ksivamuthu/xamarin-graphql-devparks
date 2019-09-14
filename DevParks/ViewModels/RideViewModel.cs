using System;
namespace DevParks.ViewModels
{
    public class RideViewModel : BaseViewModel
    {
        string _id;
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        string _waitTime;
        public string WaitTime
        {
            get { return _waitTime; }
            set { SetProperty(ref _waitTime, value); }
        }

        string _logo;
        public string Logo
        {
            get { return _logo; }
            set { SetProperty(ref _logo, value); }
        }
    }
}
