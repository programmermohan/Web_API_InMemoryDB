using APITest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winform_API_Test
{
    public partial class frmAPITest : Form
    {
        public static string JWTToken { get; set; }
        public static string APIUrl { get; set; }
        public frmAPITest()
        {
            InitializeComponent();
        }

        private void frmAPITest_Load(object sender, EventArgs e)
        {
            APIUrl = ConfigurationManager.AppSettings["APIUrl"].ToString();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        public async static void Login()
        {
            try
            {
                TokenManage tokenManage = new TokenManage();
                User user = new User() { UserName = "MohanC" };
                var json = JsonConvert.SerializeObject(user);
                using (HttpClient httpclient = new HttpClient())
                {
                    string url = APIUrl + "Users/Login";

                    ASCIIEncoding encoding = new ASCIIEncoding();
                    byte[] data = encoding.GetBytes(json);

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "Post";
                    request.ContentLength = data.Length;
                    request.ContentType = "application/json";

                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response != null)
                    {
                        using (var StreamReader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = StreamReader.ReadToEnd();
                            tokenManage = JsonConvert.DeserializeObject<TokenManage>(result);

                            JWTToken = tokenManage.Token;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btnGetEmployees_Click(object sender, EventArgs e)
        {
            GetEmployees();
        }

        public async void GetEmployees()
        {
            try
            {
                string url = APIUrl + "Employees/GetAllEmployees";
                using(HttpClient httpclient = new HttpClient())
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";
                    request.ContentType = "application/json";
                    httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JWTToken);
                    var response = await httpclient.GetStringAsync(url);
                    if(response != null)
                    {
                        var employees = JsonConvert.DeserializeObject<List<Employee>>(response);
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async void GetEmployeeByID(int id)
        {
            try
            {
                string url = APIUrl + "Employees/GetEmployeeById?id=" + id;
                using (HttpClient httpclient = new HttpClient())
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";
                    request.ContentType = "application/json";

                    //no need to pass as this method is annonymous method
                    //httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JWTToken); 
                    var response = await httpclient.GetStringAsync(url);
                    if (response != null)
                    {
                        var employee = JsonConvert.DeserializeObject<Employee>(response);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btnGetEmployee_Click(object sender, EventArgs e)
        {
            GetEmployeeByID(10);
        }
    }
}
