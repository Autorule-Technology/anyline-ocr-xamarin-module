using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Anyline
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var scanMode_configurations = new List<KeyValuePair<string, string>>()
            {
                //new KeyValuePair<string, string>("Barcode","others_config_barcode"),
                new KeyValuePair<string, string>("Scan Plate","vehicle_config_license_plate"),
                new KeyValuePair<string, string>("Manual Entry","vehicle_config_license_plate"),
                //new KeyValuePair<string, string>("Meter","energy_config_analog_digital"),
                //new KeyValuePair<string, string>("USNR","mro_config_usnr")
            };

            // YOUR LICENSE KEY HERE
            string licenseKey = "eyJzY29wZSI6WyJBTEwiXSwicGxhdGZvcm0iOlsiaU9TIiwiQW5kcm9pZCIsIldpbmRvd3MiLCJKUyIsIldlYiJdLCJ2YWxpZCI6IjIwMjAtMTItMjYiLCJtYWpvclZlcnNpb24iOjMsIm1heERheXNOb3RSZXBvcnRlZCI6NSwic2hvd1dhdGVybWFyayI6dHJ1ZSwicGluZ1JlcG9ydGluZyI6dHJ1ZSwiZGVidWdSZXBvcnRpbmciOiJvcHQtb3V0IiwidG9sZXJhbmNlRGF5cyI6NSwic2hvd1BvcFVwQWZ0ZXJFeHBpcnkiOnRydWUsImlvc0lkZW50aWZpZXIiOlsiY29tLnZpbmRleC5hdXRvcnVsZS5tb2JpbGUiXSwiYW5kcm9pZElkZW50aWZpZXIiOlsiY29tLnZpbmRleC5hdXRvcnVsZS5tb2JpbGUiXSwid2luZG93c0lkZW50aWZpZXIiOlsiY29tLnZpbmRleC5hdXRvcnVsZS5tb2JpbGUiXSwid2ViSWRlbnRpZmllciI6WyJjb20udmluZGV4LmF1dG9ydWxlLm1vYmlsZSJdLCJqc0lkZW50aWZpZXIiOlsiY29tLnZpbmRleC5hdXRvcnVsZS5tb2JpbGUiXSwiaW1hZ2VSZXBvcnRDYWNoaW5nIjp0cnVlLCJsaWNlbnNlS2V5VmVyc2lvbiI6Mn0KSC9GSkNBeWg5Mmd0NHcvM0hOQ1drcjlOSytSQWpvRjh3S2dBcm9tRzJyY3U0MGJjRTIrZ2o2cU15V0k2dTMvZXBpbWhtK1RBQkh6NXFpbnQrSkFSeWM1disyRDBPTFJBVVB1amxxM1F4K01BcldHbDNEOEQzMXk0Q1o2UDBjUnN1bVZDS25zdlhJSVJjRVBmWTFMa3FwcWhEN0psdTA5T1VvN0lJUUZvVWxmaTZDcU8wQzlNbm0wOFYzL0FnMDZNa3NOQjlTb09JQm5LanoxdXpYa0ZMQUNYWndCZmVmWHN1L201eUJrcG0xMHNUbmZEcUY5d0t4LzFyVkFNVGp6RWYyelZpdnJ5cWc3RC9UZi9yV1VGZHdLRmFFM08zeVVLV2c4QUpORkQva2l6OTlUZFdzOC8wSFFKM0JtTnoxRFhudnVYUVBtRHYrSDUydUpXMkl4a0pBPT0=";

            // Initializes the Anyline SDK natively in each platform and get the results back
            bool isAnylineInitialized = DependencyService.Get<IAnylineSDKService>().SetupWithLicenseKey(licenseKey, out string licenseErrorMessage);

            AddButtons(scanMode_configurations, isEnabled: isAnylineInitialized);

            if (!isAnylineInitialized)
            {
                DisplayAlert("License Exception", licenseErrorMessage, "OK");
            }
        }

        private void AddButtons(List<KeyValuePair<string, string>> scanModes_Configurations, bool isEnabled)
        {
            foreach (var item in scanModes_Configurations)
            {
                var btScan = new Button() { Text = item.Key, BackgroundColor = Color.FromHex("77A22F"), TextColor = Color.White }; //was 32ADFF
                btScan.Clicked += BtScan_Clicked;
                btScan.ClassId = item.Value;
                btScan.IsEnabled = isEnabled;
                slScanButtons.Children.Add(btScan);
            }
        }

        private async void BtScan_Clicked(object sender, EventArgs e)
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                await Permissions.RequestAsync<Permissions.Camera>();
            }

            (sender as Button).IsEnabled = false;
            await Navigation.PushAsync(new ScanExamplePage(((Button)sender).ClassId));
            (sender as Button).IsEnabled = true;
        }
    }
}