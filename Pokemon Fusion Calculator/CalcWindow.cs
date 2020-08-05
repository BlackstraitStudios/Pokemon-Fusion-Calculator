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
        Label[] leftMonLabels;
        Label[] rightMonLabels;
        Label[] rightPureLabels;
        Label[] leftPureLabels;
        Label[][] labelLists;

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

            leftMonLabels = new Label[] { lHP, lATK, lDEF,
                                         lSPA, lSPD, lSPE,
                                         lpType, lsType };

            rightMonLabels = new Label[] { rHP, rATK, rDEF,
                                          rSPA, rSPD, rSPE,
                                          rpType, rsType };

            rightPureLabels = new Label[] { prHP, prATK, prDEF,
                                            prSPA, prSPD, prSPE };

            leftPureLabels = new Label[] { plHP, plATK, plDEF,
                                           plSPA, plSPD, plSPE };

            labelLists = new Label[][] { leftMonLabels, rightMonLabels, leftPureLabels, rightPureLabels };
        }

        void textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && headComponent.Text != "" && headComponent.Text != "")
            {
                List<FusionData> temp = new List<FusionData>();
                string a = char.ToUpper(headComponent.Text[0]) + headComponent.Text.Substring(1);
                string b = char.ToUpper(bodyComponent.Text[0]) + bodyComponent.Text.Substring(1);
                int hc = 1;
                int bc = 1;

                if (DexData.pokestats.ContainsKey(a) && DexData.pokestats.ContainsKey(b))
                {
                    hc = DexData.pokestats[a].DexNumber;
                    bc = DexData.pokestats[b].DexNumber;
                }

                else
                {
                    return;
                }

                selectSprites(a, b, hc, bc);
                temp.Add(calcStat(a,b));
                temp.Add(calcStat(b,a));
                updateLabels(a, b, hc, bc, temp);
                e.SuppressKeyPress = true;
            }
        }

        FusionData calcStat(string a, string b)
        {
            PokeStat pokemonA = DexData.pokestats[a];
            PokeStat pokemonB = DexData.pokestats[b];
            DexData.types t1 = DexData.types.none;
            DexData.types t2 = DexData.types.none;

            if (a != b)
            {
                t1 = pokemonA.TypeException1 == DexData.types.none ? 
                                                                     pokemonA.PrimaryType : 
                                                                     pokemonA.TypeException1;

                t2 = pokemonB.TypeException1 == DexData.types.none ?
                                                                     pokemonB.SecondaryType == DexData.types.none ?
                                                                                                                    pokemonB.PrimaryType == t1 ?
                                                                                                                                                 DexData.types.none :
                                                                                                                                                 pokemonB.PrimaryType :
                                                                                                                    pokemonB.SecondaryType == t1 ?
                                                                                                                                                   pokemonB.PrimaryType :
                                                                                                                                                   pokemonB.SecondaryType :                             
                                                                     pokemonB.TypeException1;
            }
            else
            {
                t1 = pokemonA.TypeException2 == DexData.types.none ?
                                                                     pokemonA.PrimaryType :
                                                                     pokemonA.TypeException2;
                t2 = pokemonA.TypeException3 == DexData.types.none ?
                                                                     pokemonA.SecondaryType :
                                                                     pokemonA.TypeException3;
            }

            return new FusionData(((pokemonA.HP * 2) + pokemonB.HP) / 3,
                                  ((pokemonB.Attack * 2) + pokemonA.Attack) / 3,
                                  ((pokemonB.Defense * 2) + pokemonA.Defense) / 3,
                                  ((pokemonA.SpAtk * 2) + pokemonB.SpAtk) / 3,
                                  ((pokemonA.SpDef * 2) + pokemonB.SpDef) / 3,
                                  ((pokemonB.Speed * 2) + pokemonA.Speed) / 3,
                                    t1,
                                    t2);
        }

        void updateLabels(string a, string b, int hc, int bc, List<FusionData> fusion)
        {
            dexL.Text = hc.ToString();
            dexR.Text = bc.ToString();
            headLabelL.Text = a;
            headLabelR.Text = b;
            bodyLabelL.Text = b;
            bodyLabelR.Text = a;
            
            for (int i = 0; i < 2; i++)
            {
                for (int x = 0; x < 6; x++)
                {
                    labelLists[i][x].Text = fusion[i].stats[x].ToString();
                    labelLists[i][x].BackColor = fusion[i].stats[x] > fusion[Math.Abs(i - 1)].stats[x] ?
                                                                                                         Color.Green :
                                                                                                         fusion[i].stats[x] == fusion[Math.Abs(i - 1)].stats[x] ?
                                                                                                                                                                  Color.Orange :
                                                                                                                                                                  Color.Red;

                    plName.Text = a;
                    labelLists[2][x].Text = DexData.pokestats[b].statlist[x].ToString();
                    labelLists[2][x].BackColor = DexData.pokestats[b].statlist[x] > fusion[Math.Abs(i - 1)].stats[x] ?
                                                                                                                       Color.Green :
                                                                                                                       DexData.pokestats[b].statlist[x] == fusion[Math.Abs(i - 1)].stats[x] ?
                                                                                                                                                                                              Color.Orange :
                                                                                                                                                                                              Color.Red;
                    prName.Text = b;
                    labelLists[3][x].Text = DexData.pokestats[a].statlist[x].ToString();
                    labelLists[3][x].BackColor = DexData.pokestats[a].statlist[x] > fusion[Math.Abs(i - 1)].stats[x] ?
                                                                                                                       Color.Green :
                                                                                                                       DexData.pokestats[a].statlist[x] == fusion[Math.Abs(i - 1)].stats[x] ?
                                                                                                                                                                                              Color.Orange :
                                                                                                                                                                                              Color.Red;

                }
                labelLists[i][6].Text = fusion[i].pType.ToString();
                labelLists[i][7].Text = fusion[i].sType.ToString();
            }
        }

        void selectSprites(string a, string b, int hc, int bc)
        {
            string fusionPath = Properties.Settings.Default.GameDirectory + @"\Graphics\CustomBattlers\" + hc + "." + bc + ".png";
            string reverseFusionPath = Properties.Settings.Default.GameDirectory + @"\Graphics\CustomBattlers\" + bc + "." + hc + ".png";
            Console.WriteLine(fusionPath);
            Console.WriteLine(reverseFusionPath);

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
