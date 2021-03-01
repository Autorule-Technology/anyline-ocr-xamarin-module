using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text;
using System.Net;


namespace Anyline
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultsPage : ContentPage
    {
        public ResultsPage(Dictionary<string, dynamic> results, string configurationFile)
        {
            InitializeComponent();
            Task.Run(() => ShowResults(results));

            btHome.Clicked += async (s, e) => await Navigation.PopToRootAsync();
            btScanAgain.Clicked += async (s, e) =>
            {
                Navigation.InsertPageBefore(new ScanExamplePage(configurationFile), this);
                await Navigation.PopAsync();
            };
            btPost.Clicked += async (s, e) =>
            {
                //using \n
                Console.WriteLine("post");
                //foreach (var item in results)
                //    Console.WriteLine(item);
                //foreach (KeyValuePair<string, object> kvp in results)
                //{
                //    Console.WriteLine(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
                //}

                //DateTime saveUtcNow = DateTime.UtcNow;

                DateTime now = DateTime.Now;
                string timestamp = now.ToString("yyyy-MM-dd HH:mm:ss");

                string platestring = results["Result"];
                string confidencestring = results["Confidence"];
                string useridstring = results["PluginID"];
                string platestring1 = platestring.Replace(" ", "");
                Console.WriteLine(timestamp);
                Console.WriteLine(platestring1);
                Console.WriteLine(confidencestring);
                Console.WriteLine(useridstring);
                
                
                
                
                //PostResults(results);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://213.35.12.10:17479/sightings");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                try { 
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //string json = "{\"user\":\"test\"," +
                    //              "\"password\":\"bla\"}";


                    string json = "{\"vehicleid\":\""+ platestring1 + timestamp + "\"," +
                                  "\"confidence\":\"" + confidencestring +"\"," +
                                  "\"location\":\"Site01\"," +
                                  "\"manualread\":\"false\"," +
                                  "\"passcode\":\"passcod01\"," +
                                  "\"plate\":\""+ platestring1 +"\"," +
                                  "\"userid\":\""+ useridstring +"\"," +
                                  "\"timestamp\":\"" + timestamp +"\"}";
                    Console.WriteLine(json); 

                    streamWriter.Write(json);
                }
                }
                catch (Exception er)
                {
                    Console.Out.WriteLine("-----------------");
                    Console.Out.WriteLine(er.Message);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Console.WriteLine(httpResponse);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }


                if (httpResponse.StatusCode == HttpStatusCode.OK)
                    Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}",
                                         httpResponse.StatusDescription);
                    await DisplayAlert("Success", "Sighting Posted", "OK");
                    Navigation.PopToRootAsync();

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                    Console.WriteLine("\r\nResponse Status Code is BAD and StatusDescription is: {0}",
                                         httpResponse.StatusDescription);
                    await DisplayAlert("Failure", "Unable to post sighting: {0}", "OK",
                                         httpResponse.StatusDescription);
                //if success 

            };
        }

        private void ShowResults(Dictionary<string, object> results)
        {
            StackLayout slResults = new StackLayout();
            foreach (var item in results)
            {
                slResults.Children.Add(new Label { Text = item.Key, TextColor = Color.FromHex("77A22F"), FontSize = 15 });
                if (item.Value.GetType() == typeof(byte[]))
                {
                    var img = new Image()
                    {
                        Aspect = Aspect.AspectFit,
                        Source = ImageSource.FromStream(() => new MemoryStream(item.Value as byte[]))
                    };

                    slResults.Children.Add(img);
                }
                else
                {
                    slResults.Children.Add(new Label { Text = item.Value.ToString(), TextColor = Color.White, FontAttributes = FontAttributes.Bold, FontSize = 17 });
                }
            }

            Device.BeginInvokeOnMainThread(() => cvContent.Content = slResults);
        }

    }
}