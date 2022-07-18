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
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace VisionApiBinaryData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private async void button1_Click(object sender, EventArgs e)
        {
            byte[] ImageData = null;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                pictureBox1.ImageLocation = openFileDialog1.FileName;
                ImageData = File.ReadAllBytes(openFileDialog1.FileName);
            }
            HttpClient client = new HttpClient();
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "c3b95ede298c455e80f20aa99e972245");

            // Request parameters
            queryString["visualFeatures"] = "Categories,Description";
            queryString["details"] = "Landmarks"; // Celebrities 名人, Landmarks 地標
            queryString["language"] = "en";
            queryString["model-version"] = "latest";
            var uri = "https://msit141-03-vision.cognitiveservices.azure.com/vision/v3.2/analyze?" + queryString;

            // using自動資源管理
            using(ByteArrayContent ImageContent = new ByteArrayContent(ImageData))
            {
                ImageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream"); // 傳送2進位資料需用octet-stream(沒有指定格式(jpg,png...)
                HttpResponseMessage response = await client.PostAsync(uri, ImageContent);
                response.EnsureSuccessStatusCode(); // 判斷Http呼叫的statusCode 400以下(成功)
                string content = await response.Content.ReadAsStringAsync();
                //MessageBox.Show(content);
                dynamic result = JsonConvert.DeserializeObject(content); //dynamic 動態型別，等號右邊什麼左邊就是什麼 (var的話塞變數的當下型別就固定,也就是說之後假如在塞東西給faces型別不是原本的就會出錯)
                JObject jResult = result as JObject;

                string landmark = jResult["categories"][0]["detail"]["landmarks"][0]["name"].ToString();
                double confidence1 = Convert.ToDouble(jResult["categories"][0]["detail"]["landmarks"][0]["confidence"]);
                string description = jResult["description"]["captions"][0]["text"].ToString();
                double confidence2 = Convert.ToDouble(jResult["description"]["captions"][0]["confidence"]);

                MessageBox.Show($"地標: {landmark},\n信心指數: {(confidence1 * 100).ToString("0.#")}%\n描述: {description},\n信心指數: {(confidence2 * 100).ToString("0.#")}%");
            }
        }
    }
}
