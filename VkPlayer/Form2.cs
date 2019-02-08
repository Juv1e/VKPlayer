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
using VkNet.Model.RequestParams;

namespace VkPlayer
{
    public partial class Form2 : Form
    {
        static WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
        static VkApi vkapi = new VkApi();
        static string url = "";
        static string trackname = "";
        static string currentTime = "";
        static string samoTime = "";
        bool IsFirstClick = true;

        public Form2()
        {
            InitializeComponent();
            JObject parse = Parse();
            JObject select = (JObject)parse;
            string login = select["logindata"]["login"].ToString();
            string password = select["logindata"]["password"].ToString();
            vkapi.Authorize(new ApiAuthParams
            {
                Login = login,
                Password = password,
                Settings = Settings.All,
                ApplicationId = 6146827,
            });
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var audios = vkapi.Audio.Get(new AudioGetParams { });
            foreach (var audio in audios)
            {
                listBox1.Items.Add($"{audio.Artist} - {audio.Title}");
            }
        }

        public JObject Parse(string file = null)
        {
            if (file == null)
                file = "data.json";

            string dbText = File.ReadAllText(@file);
            JObject parse = JObject.Parse(dbText);
            return parse;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = listBox1.SelectedIndex;
            int k = 0;
            var audios = vkapi.Audio.Get(new AudioGetParams { });
            for (k = 0; k <= i; k++)
            {
                url = audios[k].Url.ToString();
                trackname = $"{audios[k].Artist} - {audios[k].Title}";
                if (checkBox1.Checked)
                {
                    vkapi.Audio.SetBroadcast(audios[k].OwnerId + "_" + audios[k].Id);
                }
            }
        }

            private void button1_Click(object sender, EventArgs e)
            {
            wplayer.URL = url;
            wplayer.controls.play();
            label1.Text = trackname;
            timer1.Start();
            wplayer.settings.volume = trackBar1.Value;
            }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double t = Math.Floor(wplayer.controls.currentPosition);
            double d = Math.Floor(wplayer.currentMedia.duration);
            int time = Convert.ToInt32(d);
            int timer = Convert.ToInt32(t);
            progressBar1.Maximum = time;
            progressBar1.Value = timer;
            int sec = timer;
            int minutes = sec / 60;
            int newSec = sec - minutes * 60;
            int hour = minutes / 60;
            int newMinnutes = minutes - hour * 60;
            TimeSpan times = new TimeSpan(hour, newMinnutes, newSec);

            int secs = time;
            int minutess = secs / 60;
            int newSecs = secs - minutess * 60;
            int hours = minutess / 60;
            int newMinnutess = minutess - hours * 60;
            TimeSpan timess = new TimeSpan(hours, newMinnutess, newSecs);
            label2.Text = times.ToString() + "/" + timess;
            currentTime = label2.Text.Split('/')[0].Trim();
            samoTime = label2.Text.Split('/')[1].Trim();
            if (samoTime != "00:00:00")
            {
                if (currentTime == samoTime)
                {
                    listBox1.SelectedIndex = listBox1.SelectedIndex + 1;
                    int i = listBox1.SelectedIndex;
                    int k = 0;
                    var audios = vkapi.Audio.Get(new AudioGetParams { });
                    for (k = 0; k <= i; k++)
                    {
                        url = audios[k].Url.ToString();
                        trackname = $"{audios[k].Artist} - {audios[k].Title}";
                        if (checkBox1.Checked)
                        {
                            vkapi.Audio.SetBroadcast(audios[k].OwnerId + "_" + audios[k].Id);
                        }
                    }
                    label1.Text = trackname;
                    wplayer.URL = url;
                    wplayer.controls.play();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "Title";
            wplayer.controls.stop();
        }

        private void trackBar1_Scroll_1(object sender, EventArgs e)
        {
            wplayer.settings.volume = trackBar1.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
