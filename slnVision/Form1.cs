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
            using (HttpResponseMessage response = await client.GetAsync(textBox1.Text)) // 網際網路發送圖片網址需要時間，建議用await
            {
                response.EnsureSuccessStatusCode(); // 判斷Http呼叫的statusCode 400以下(成功)
                using (Stream ImageStream = await response.Content.ReadAsStreamAsync()) // 讀內容假如是字串，用ReadAsStringAsync()
                {
                    pictureBox1.Image = Image.FromStream(ImageStream);
                }
            }

            NameValueCollection QueryString = HttpUtility.ParseQueryString(string.Empty);

        }
    }
}
