using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace VkPlayer
{
    public partial class Form1 : Form
    {
        static public VkApi vkapi = new VkApi();
        static string login = "";
        static string password = "";
        static public bool? isAuth { get; set; }
        public Form1()
        {
            InitializeComponent();
            try
            {
                JObject parse = Parse();
                JObject select = (JObject)parse;
                login = select["logindata"]["login"].ToString();
                password = select["logindata"]["password"].ToString();
                textBox1.Text = login;
                textBox2.Text = password;
            }
            catch (Exception)
            {
                login = textBox1.Text;
                password = textBox2.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                vkapi.Authorize(new ApiAuthParams
                {
                    Login = login,
                    Password = password,
                    Settings = Settings.All,
                    ApplicationId = 6365497,
                });
                isAuth = true;
            }
            catch (Exception ex)
            {
                isAuth = false;
                MessageBox.Show(ex.Message);
            }
            if (isAuth == true)
            {
                MessageBox.Show("Успех!");
                Balance(textBox1.Text, textBox2.Text);
                Form2 f2 = new Form2();
                f2.ShowDialog();
                this.Close();
            }
            }

        public void Balance(string log, string pass)
        {
            JObject parse = Parse();
            JObject select = (JObject)parse;
            parse["logindata"] = new JObject
             {
                { "login", log },
                { "password", pass }
              };
            //parse.Add(new JProperty(json, new JObject()));
                File.WriteAllText("data.json", JsonConvert.SerializeObject(parse, Formatting.Indented));
                select = (JObject)parse;

            string login = select["logindata"]["login"].ToString();
            string password = select["logindata"]["password"].ToString();

            return;
        }

        public JObject Parse(string file = null)
        {
            if(!File.Exists("data.json"))
            {
                var Create = File.Create("data.json");
                Create.Close();
                File.WriteAllText("data.json", "{\n}");
            }
            if (file == null)
                file = "data.json";

            string dbText = File.ReadAllText(@file);
            JObject parse = JObject.Parse(dbText);
            return parse;
        }

    }
}
