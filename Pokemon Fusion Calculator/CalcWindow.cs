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
        }
        void textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                fuseButton(this, new EventArgs());
                e.SuppressKeyPress = true;
            }
        }

        void fuseButton(object sender, EventArgs e)
        {
            if (headComponent.Text != "" && bodyComponent.Text != "")
            {
                string a = char.ToUpper(headComponent.Text[0]) + headComponent.Text.Substring(1);
                string b = char.ToUpper(bodyComponent.Text[0]) + bodyComponent.Text.Substring(1);
                int hc;
                int bc;
              
                if (DexData.pokestats.ContainsKey(a) && DexData.pokestats.ContainsKey(b))
                {
                    hc = DexData.pokestats[a].DexNumber;
                    bc = DexData.pokestats[b].DexNumber;
                }
                else
                {
                    return;
                }

                headLabelL.Text = a;
                headLabelR.Text = b;
                bodyLabelL.Text = b;
                bodyLabelR.Text = a;
                string fusionPath = Properties.Settings.Default.GameDirectory + @"\Graphics\CustomBattlers\" + hc + "." + bc + ".png";
                string reverseFusionPath = Properties.Settings.Default.GameDirectory + @"\Graphics\CustomBattlers\" + bc + "." + hc + ".png";
                Console.WriteLine(fusionPath);

                if (File.Exists(fusionPath))
                {
                    Console.WriteLine("fusion found");
                    pictureBox1.Image = Image.FromFile(fusionPath);
                }
                else
                {
                    Console.WriteLine("fusion not found");
                    pictureBox1.Image = Image.FromFile($@"{Properties.Settings.Default.GameDirectory}\Graphics\Battlers\{hc}\{hc}.{bc}.png");
                }

                if (File.Exists(reverseFusionPath))
                {
                    Console.WriteLine("reverse found");
                    pictureBox2.Image = Image.FromFile(reverseFusionPath);
                }
                else
                {
                    Console.WriteLine("reverse not found");
                    pictureBox2.Image = Image.FromFile($@"{Properties.Settings.Default.GameDirectory}\Graphics\Battlers\{bc}\{bc}.{hc}.png");
                }
            }
        }
    }
}
