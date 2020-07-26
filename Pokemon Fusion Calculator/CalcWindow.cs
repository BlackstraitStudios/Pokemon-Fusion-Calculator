using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime;
using System.Drawing.Text;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pokemon_Fusion_Calculator
{
    public partial class CalcWindow : Form
    {
        FolderBrowserDialog fb = new FolderBrowserDialog();
        Generatefile gf = new Generatefile();
        
        public CalcWindow()
        {
            InitializeComponent();
            fb.ShowNewFolderButton = false;
            fb.Description = "select game directory";
            if (!Directory.Exists(Properties.Settings.Default.GameDirectory))
            {
                fb.ShowDialog();    
                Properties.Settings.Default.GameDirectory = fb.SelectedPath;
                Properties.Settings.Default.Save();
            }
            headComponent.KeyDown += textbox_KeyDown;
            bodyComponent.KeyDown += textbox_KeyDown;
            //gf.dothing();
        }
        void textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                fuseButton(this, new EventArgs());
                e.SuppressKeyPress = true;
            }
        }

        string fetchData(string pokemon)
        {
            string bakenumber = "";
            if (pokemon.Length < 3)
            {
                for (int i = 0; i < 3-pokemon.Length; i++)
                {
                    bakenumber += "0";
                }
                bakenumber += pokemon;
            }
            string bakedUrl = "https://www.serebii.net/pokedex-sm/" + bakenumber + ".shtml";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(bakedUrl);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();
            Console.Write(result);
            return result;
        }

        void fuseButton(object sender, EventArgs e)
        {
            if (headComponent.Text != "" && bodyComponent.Text != "")
            {
                string a = char.ToUpper(headComponent.Text[0]) + headComponent.Text.Substring(1);
                string b = char.ToUpper(bodyComponent.Text[0]) + bodyComponent.Text.Substring(1);
                int hc = 1;
                int bc = 1;
                headLabelL.Text = a;
                headLabelR.Text = b;
                bodyLabelL.Text = b;
                bodyLabelR.Text = a;

                if (DexData.pokemon.ContainsKey(a) && DexData.pokemon.ContainsKey(b))
                {
                    hc = DexData.pokemon[a];
                    bc = DexData.pokemon[b];
                }

                string fusionString = "";
                fusionString = headComponent.Text + "." + bodyComponent.Text;

                string fusionPath = Properties.Settings.Default.GameDirectory + @"\Graphics\CustomBattlers\" + fusionString + ".png";

                if (bodyComponent.Text != "")
                {
                    if (File.Exists(fusionPath))
                    {
                        pictureBox1.Image = Image.FromFile(fusionPath);
                        pictureBox2.Image = Image.FromFile(fusionPath);
                    }
                    else
                    {
                        pictureBox1.Image = Image.FromFile($@"{Properties.Settings.Default.GameDirectory}\Graphics\Battlers\{hc}\{hc}.{bc}.png");
                        pictureBox2.Image = Image.FromFile($@"{Properties.Settings.Default.GameDirectory}\Graphics\Battlers\{bc}\{bc}.{hc}.png");
                    }
                }
                else
                {
                    pictureBox1.Image = Image.FromFile($@"{Properties.Settings.Default.GameDirectory}\Graphics\Battlers\{hc}\{bc}.png");
                    pictureBox2.Image = Image.FromFile($@"{Properties.Settings.Default.GameDirectory}\Graphics\Battlers\{bc}\{hc}.png");
                }
            }
        }
    }
}
