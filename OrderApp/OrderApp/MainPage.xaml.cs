using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrderApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnSend_Clicked(object sender, EventArgs e)
        {
            string Endpoint = "https://msit141-03-luis.cognitiveservices.azure.com";
            string AppId = "aa8db9b8-a5ad-4ab8-956d-3a8b8a40ef13";
            string Key = "effd11f724324528993a0e568341b254";
            string Content = txtContent.Text;

            string Url = $"{Endpoint}/luis/prediction/v3.0/apps/{AppId}/slots/production/predict?verbose=true&show-all-intents=true&log=true&subscription-key={Key}&query={Content}";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(Url);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            JObject jsonQuery = JsonConvert.DeserializeObject(result) as JObject;
            string Intent = jsonQuery["prediction"]["topIntent"].ToString();
            double Confidence = Convert.ToDouble(jsonQuery["prediction"]["intents"][Intent]["score"]);

            string UserIntent = $"意向: {Intent}, 信心指數: {(Confidence * 100).ToString(".#")}%";
            await DisplayAlert("回應", UserIntent, "關閉");
        }
    }
}
