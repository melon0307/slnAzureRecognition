using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace prjVision
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            // using 自動資源管理
            using (HttpResponseMessage response = await client.GetAsync(textBox1.Text)) // 網際網路發送圖片網址需要時間，建議用await
            {
                response.EnsureSuccessStatusCode(); // 判斷Http呼叫的statusCode 400以下(成功)
                using (Stream ImageStream = await response.Content.ReadAsStreamAsync()) // 讀內容假如是字串，用ReadAsStringAsync()
                {
                    pictureBox1.Image = Image.FromStream(ImageStream);
                }
            }

            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "c3b95ede298c455e80f20aa99e972245");

            // Request parameters
            queryString["visualFeatures"] = "Tags";
            //queryString["details"] = "{string}"; // Celebrities 名人, Landmarks 地標
            queryString["language"] = "en";
            queryString["model-version"] = "latest";
            var uri = "https://msit141-03-vision.cognitiveservices.azure.com/vision/v3.2/analyze?" + queryString;

            JObject data = new JObject { ["url"] = textBox1.Text };
            string JsonImage = JsonConvert.SerializeObject(data);
            StringContent stringContent = new StringContent(JsonImage, Encoding.UTF8, "application/json");
            HttpResponseMessage responseImage = await client.PostAsync(uri, stringContent);
            responseImage.EnsureSuccessStatusCode();// 判斷Http呼叫的statusCode 400以下(成功)
            string result = await responseImage.Content.ReadAsStringAsync();
            //MessageBox.Show(result);

            dynamic tags = JsonConvert.DeserializeObject(result); //dynamic 動態型別，等號右邊什麼左邊就是什麼 (var的話塞變數的當下型別就固定,也就是說之後假如在塞東西給faces型別不是原本的就會出錯)
            JObject jTags = tags as JObject;
            string tag = jTags["tags"][0]["name"].ToString();
            double confidence = Convert.ToDouble(jTags["tags"][0]["confidence"]);
            MessageBox.Show($"內容: {tag}, 信心指數: {(confidence * 100).ToString("0.#")}%");
            //string text = jFaces["description"]["captions"][0]["text"].ToString();
            //double confidence = Convert.ToDouble(jFaces["description"]["captions"][0]["confidence"]);
            //MessageBox.Show($"內容: {text}, 信心指數: {(confidence * 100).ToString("0.#")}%");
        }
    }
}
