using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using asprise_ocr_api;
using HttpHelper;

namespace Main
{
    public partial class Form1 : Form
    {
        private readonly CookieContainer container = new CookieContainer();
        private Dictionary<string, string> postData;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ResetImage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            postData["username"] = txtUserName.Text;
            postData["password"] = txtPassword.Text;
            postData["authcode"] = txtVerifyCode.Text;

            var resposne = WebRequestFactory.Create(
                "https://sso.juneyaoair.com/cas/login?service=http://www.juneyaoair.com/", "POST")
                .SetCookieContainer(container)
                .Post(postData);

            MessageBox.Show(resposne.ResponseUri.ToString() == "http://www.juneyaoair.com/" ? "登陆成功" : "登陆失败");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ResetImage();
        }

        private void ResetImage()
        {
            var homePageRequest = WebRequestFactory.Create("https://sso.juneyaoair.com/cas/login?service=http://www.juneyaoair.com/", "GET");
            postData = homePageRequest.SetCookieContainer(container)
                .GetResponse()
                .GetFormData();

            var stream = WebRequestFactory.Create("https://sso.juneyaoair.com/cas/captcha.jpg", "GET")
                .SetCookieContainer(container)
                .ForImage("jpeg")
                .GetResponse()
                .GetResponseStream();
            pictureBox1.Image = Image.FromStream(stream);
            pictureBox1.Image.Save("file.jpg");
           

            stream.Close();
        }
    }
}