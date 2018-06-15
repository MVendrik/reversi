using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace reversi
{
    public partial class SpeelReversi : Form
    {
        Label Label_Beurt;
        Button Knop_Help;
        Label Label_RodeSteen;
        Label Label_BlauweSteen;

        private bool Help = false;

        const int cellsize = 60;

        Steen[,] speelveld;
        int Xspeelveld = 100;
        int Yspeelveld = 200;

        int HuidigeSpeler = 2;

        public SpeelReversi()
        {
            this.Text = "Reversi";
            this.BackColor = Color.White;
            this.Size = new Size(650, 650);

            #region label beurt
            Label_Beurt = new Label();
            Label_Beurt.Text = "Blauw is aan zet";
            Label_Beurt.Location = new Point(100, 150);
            Label_Beurt.Size = new Size(200, 50);
            Label_Beurt.Font = new Font("Arial", 12);
            #endregion

            #region knop help
            Knop_Help = new Button();
            Knop_Help.Text = "Help";
            Knop_Help.Location = new Point(150, 10);
            Knop_Help.Size = new Size(100, 35);
            Knop_Help.Font = new Font("Arial", 10);
            Knop_Help.BackColor = Color.LightGray;
            Knop_Help.Click += Knop_Help_Click;
            #endregion

            #region label rode stenen
            Label_RodeSteen = new Label();
            Label_RodeSteen.Text = "2 stenen";
            Label_RodeSteen.Location = new Point(150, 100);
            Label_RodeSteen.Size = new Size(200, 50);
            Label_RodeSteen.Font = new Font("Arial", 12);
            Label_RodeSteen.ForeColor = Color.Red;
            #endregion

            #region label blauwe stenen
            Label_BlauweSteen = new Label();
            Label_BlauweSteen.Text = "2 stenen";
            Label_BlauweSteen.Location = new Point(350, 100);
            Label_BlauweSteen.Size = new Size(200, 50);
            Label_BlauweSteen.Font = new Font("Arial", 12);
            Label_BlauweSteen.ForeColor = Color.Blue;
            #endregion

            this.Controls.Add(Label_Beurt);
            this.Controls.Add(Knop_Help);
            this.Controls.Add(Label_RodeSteen);
            this.Controls.Add(Label_BlauweSteen);

            this.speelveld = new Steen[6, 6];
            this.initialiseer_speelveld();

            this.Paint += tekengrid;
            this.MouseClick += controleer_geldige_klik;
        }

        private void Knop_Help_Click(object sender, EventArgs e)
        {
            Help = true;
            this.Invalidate();
        }

        private void initialiseer_speelveld()
            // Het speelveld krijgt standaard vier speelstenen aan het begin. Initialiseer deze in deze methode. 
        {
            Point vak1 = new Point(Xspeelveld + (cellsize * 2), Yspeelveld + (cellsize * 2));
            speelveld[2, 2] = new Steen(2, vak1, cellsize);

            Point vak2 = new Point(Xspeelveld + (cellsize * 3), Yspeelveld + (cellsize * 2));
            speelveld[3, 2] = new Steen(1, vak2, cellsize);

            Point vak3 = new Point(Xspeelveld + (cellsize * 2), Yspeelveld + (cellsize * 3));
            speelveld[2, 3] = new Steen(1, vak3, cellsize);

            Point vak4 = new Point(Xspeelveld + (cellsize * 3), Yspeelveld + (cellsize * 3));
            speelveld[3, 3] = new Steen(2, vak4, cellsize);

            //Point vak5 = new Point(Xspeelveld + (cellsize * 4), Yspeelveld + (cellsize * 2));
            //speelveld[4, 2] = new Steen(1, vak5, cellsize);
        }

        private void tekengrid(object sender, PaintEventArgs pea)
            // Deze methode tekent het speeldbord, met 6 bij 6 vierkanten.
        {
            Graphics gr = pea.Graphics;
            Pen linepen = new Pen(Color.Black);
            
            for (int x = Xspeelveld; x < 460; x += cellsize)
            {
                for (int y = Yspeelveld; y < 560; y += cellsize)
                {
                    gr.DrawRectangle(linepen, x, y, cellsize, cellsize);
                }
            }
            this.tekenSteen(gr);

            if (Help)
                teken_help(gr);

        }

        private void tekenSteen(Graphics gr)
        {

            for (int x = 0; x < speelveld.GetLength(0); x++)
            {
                for (int y = 0; y < speelveld.GetLength(1); y++)
                {
                    if (speelveld[x, y] != null)
                    {
                        speelveld[x, y].kleurSteen(gr);
                    }
                }
            }
        }

        private void teken_help(Graphics gr)
        {
            for(int x =0; x < speelveld.GetLength(0); x ++)
                for (int y = 0; y < speelveld.GetLength(1); y++)
                {
                    if (is_geldige_zet(new Point(x, y)))
                    {
                        if (speelveld[x, y] == null)
                        {
                            Pen pen = new Pen(Color.Black);
                            gr.DrawEllipse(pen, (cellsize * x + Xspeelveld + cellsize / 2), (cellsize * y + Yspeelveld + cellsize / 2), (cellsize / 2), (cellsize / 2));
                        }
                    }
                }
        }

        private void controleer_geldige_klik(object sender, MouseEventArgs e)
        // mouseevent functie die muiscoordinaten omzet naar de array-waardes van het speelveld.
        {
            Point arraywaarde = this.bereken_array_coordinaat(e.X, e.Y);

            //kijk of het een geldige klik is
            if(arraywaarde.X>=0 && arraywaarde.X < speelveld.GetLength(0) && arraywaarde.Y>=0 && arraywaarde.Y < speelveld.GetLength(1))
            {
                //geldige klik, controleer nu geldige zet
                if(is_geldige_zet(arraywaarde))
                {
                    //zet is geldig dus doe de zet en maak een steen aan.
                    speelveld[arraywaarde.X, arraywaarde.Y] = new Steen(HuidigeSpeler, new Point((arraywaarde.X*cellsize+Xspeelveld) ,arraywaarde.Y*cellsize + 
                        Yspeelveld), cellsize);

                    //check voor elke richting voor het flippen van de stenen.
                    this.check_and_flip(arraywaarde, -1, 0);
                    this.check_and_flip(arraywaarde, 0, -1);
                    this.check_and_flip(arraywaarde, 1, 0);
                    this.check_and_flip(arraywaarde, 0, 1);
                    this.check_and_flip(arraywaarde, 1, 1);
                    this.check_and_flip(arraywaarde, -1, -1);
                    this.check_and_flip(arraywaarde, -1, 1);
                    this.check_and_flip(arraywaarde, 1, -1);

                    this.update_steenlabels();
                    this.einde_beurt();
                    

                }
                else
                {
                    // zet is niet geldig doe niks.
                }
               
            }
            else
            {
                //geen geldige klik
            }

            this.Invalidate();
        }
        private void einde_beurt()
        {
            //update huidige speler
            if (HuidigeSpeler == 1)
            {
                HuidigeSpeler = 2;
                this.Label_Beurt.Text = "Blauw is aan zet";
            }
            else
            {
                HuidigeSpeler = 1;
                this.Label_Beurt.Text = "Rood is aan zet";
            }

        }

        private void update_steenlabels()
        {
            int aantal_rodestenen = 0;
            int aantal_blauwestenen = 0;

            for (int x = 0; x < speelveld.GetLength(0); x++)
                for (int y = 0; y < speelveld.GetLength(1); y++)
                {
                    if (speelveld[x, y] != null)
                    {
                        if (speelveld[x, y].speler == 1)
                            aantal_rodestenen++;
                        else
                            aantal_blauwestenen++;

                    }
                }
            Label_BlauweSteen.Text =  aantal_blauwestenen.ToString() + " stenen";
            Label_RodeSteen.Text = aantal_rodestenen.ToString() + " stenen";
        }

        private void check_and_flip(Point arraywaarde,int x_richting, int y_richting)
        {
            // kijk of de zet in deze richting een valide zet was.
            if(check_richting(arraywaarde, x_richting, y_richting))
            {
                int x = x_richting;
                int y = y_richting;
                // als valide zet doe flippen
                while (arraywaarde.X + x >= 0 && arraywaarde.X + x < speelveld.GetLength(0) &&
                arraywaarde.Y + y >= 0 && arraywaarde.Y + y < speelveld.GetLength(1) &&
                speelveld[arraywaarde.X + x, arraywaarde.Y + y] != null)
                {
                    //update het vakje
                    speelveld[arraywaarde.X + x, arraywaarde.Y + y].update_eigenaar(HuidigeSpeler);
                    //update x en y
                    x += x_richting;
                    y += y_richting;
                }
            }
            else
            {
                // doe niks
            }

        }

        private Point bereken_array_coordinaat(int muisX,int muisY)
            // Helperfunctie bij heet bepalen van een geldige klik. 
        {
            //gebruik een leeg antwoord.
            Point answer = new Point(0, 0);

            double answerX = ((muisX - 100.00) / cellsize);
            if (answerX < 0)
            {
                answer.X = -1;
            }
            else
            {
                answer.X = (int)answerX;
            }

            double answerY = ((muisY - 200.00) / cellsize);
            if (answerY < 0)
            {
                answer.Y = -1;
            }
            else
            {
                answer.Y = (int)answerY;
            }

            return answer;
        }

        private bool check_richting(Point gekliktvakje, int x_richting, int y_richting)
            // Functie die gebruikt kan werden om te controleren in welke richting aanliggende stenen staan bij een zet.
        {
            int x = x_richting;
            int y = y_richting;
            bool is_geldige_zet = false;

            while (gekliktvakje.X + x >= 0 && gekliktvakje.X + x < speelveld.GetLength(0) &&
                  gekliktvakje.Y + y >= 0 && gekliktvakje.Y + y < speelveld.GetLength(1) &&
                   speelveld[gekliktvakje.X + x, gekliktvakje.Y + y] != null)
            {
                if (speelveld[gekliktvakje.X + x, gekliktvakje.Y + y].speler != HuidigeSpeler)
                {
                    if (gekliktvakje.X + x + x_richting >= 0 && gekliktvakje.X + x + x_richting < speelveld.GetLength(0) &&
                     gekliktvakje.Y + y + y_richting >= 0 && gekliktvakje.Y + y + y_richting < speelveld.GetLength(1) &&
                     speelveld[gekliktvakje.X + x + x_richting, gekliktvakje.Y + y + y_richting] != null &&
                     speelveld[gekliktvakje.X + x + x_richting, gekliktvakje.Y + y + y_richting].speler == HuidigeSpeler)
                    {
                        return true;
                    }
                }
                x += x_richting;
                y += y_richting;
            }
           
            return is_geldige_zet;
        }


        private bool is_geldige_zet(Point gekliktvakje)
            // Check met een boolean of de zet geldig is, maakt hierbij gebruikt van check_richting
        {
            if (speelveld[gekliktvakje.X, gekliktvakje.Y] != null)
                return false;
            //check naar links horizontaal
            else if(check_richting(gekliktvakje, -1, 0))
                return true;
            else if (check_richting(gekliktvakje, 0, -1))
                return true;
            else if (check_richting(gekliktvakje, +1, 0))
                return true;
            else if (check_richting(gekliktvakje, 0, +1))
                return true;
            else if (check_richting(gekliktvakje, -1, -1))
                return true;
            else if (check_richting(gekliktvakje, +1, +1))
                return true;
            else if (check_richting(gekliktvakje, -1, +1))
                return true;
            else if (check_richting(gekliktvakje, +1, -1))
                return true;
            else
                return false;
  
        }


    }
    public class Steen
    {
        public int speler;
        private Point steenlocatie;
        Size steengrootte;
   
        //constructor
        public Steen(int speler, Point steenlocatie, int cellsize)
        {
            this.speler = speler;
            this.steenlocatie = steenlocatie;
            
            steengrootte = new Size(cellsize, cellsize);

        }

        public void update_eigenaar(int huidigespeler)
        {
            if (this.speler != huidigespeler)
                this.speler = huidigespeler;
        }

        public void kleurSteen(Graphics gr)
            //De methode tekent de stenen op basis van de array-waardes. Deze methode heeft een helperfunctie nodig, om te bepalen op welke coordinaten de 
            // stenen getekend moeten worden.
        {
            Brush brush;
            if (speler == 1)
                brush = new SolidBrush(Color.Red);
            else
                brush = new SolidBrush(Color.Blue);

            gr.FillEllipse(brush, steenlocatie.X, steenlocatie.Y, steengrootte.Width, steengrootte.Height);
           
        }
    }
}

