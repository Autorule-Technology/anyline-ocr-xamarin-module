using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Anyline.Droid
{
    [Activity(Label = "Anyline.XamarinForms.Android", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Instance = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());


            try
            {
                // INSERT YOUR LICENSE KEY HERE
                string licenseKey = "eyJzY29wZSI6WyJBTEwiXSwicGxhdGZvcm0iOlsiaU9TIiwiQW5kcm9pZCIsIldpbmRvd3MiLCJKUyIsIldlYiJdLCJ2YWxpZCI6IjIwMjAtMTItMTkiLCJtYWpvclZlcnNpb24iOjMsIm1heERheXNOb3RSZXBvcnRlZCI6NSwic2hvd1dhdGVybWFyayI6dHJ1ZSwicGluZ1JlcG9ydGluZyI6dHJ1ZSwiZGVidWdSZXBvcnRpbmciOiJvcHQtb3V0IiwidG9sZXJhbmNlRGF5cyI6NSwic2hvd1BvcFVwQWZ0ZXJFeHBpcnkiOnRydWUsImlvc0lkZW50aWZpZXIiOlsiYXIudGVzdCJdLCJhbmRyb2lkSWRlbnRpZmllciI6WyJhci50ZXN0Il0sIndpbmRvd3NJZGVudGlmaWVyIjpbImFyLnRlc3QiXSwid2ViSWRlbnRpZmllciI6WyJhci50ZXN0Il0sImpzSWRlbnRpZmllciI6WyJhci50ZXN0Il0sImltYWdlUmVwb3J0Q2FjaGluZyI6dHJ1ZSwibGljZW5zZUtleVZlcnNpb24iOjJ9CllHT0NBVjVmc0MzOE5leEdyeTFDTU5SWDlBalV6VjJMWU1WMkZ6MkhYZzRna3pucy9KRVlkdXZha2Z4YVZ2RHhaZXp2bGllclZFTkZab1hCbEhwN2t1bWcvK3ZzVHhkam9hVW1wOUZ0UUtKYjFrVWdqaDEySXE5ZkRtOUF5WDBTaWtrTS94SEpJUnB3NzdrWWxlSEozdVZ5THNNVFpCbFdnRysrWUhYbXNSRTl1aEFySEl1dGxoUTd0ZVNaZm92MUt6aTZTODdrNHpDQnZuejBlM2w3TWR1VEhubFdmRWJSYUFRTzlRSFRtU1BCRUwwWG5UVSt3Slc1NHpJWHF3cmJISVkzTy9qZHkvWDJrekFvcDZYZ2JUeXQ5Q2JvNnBVSHZhbUtNcFFCZHRnc1E3Z0tNZTV4cXBHV3FQbGhSSzR1KyttaVp6QnBNUkdXRGVBR2djbHdOUT09";
                IO.Anyline.AnylineSDK.Init(licenseKey, this);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}