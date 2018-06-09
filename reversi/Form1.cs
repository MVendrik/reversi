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
        Button Knop_Nieuwspel;
        Label Label_RodeSteen;
        Label Label_BlauweSteen;

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

            #region knop nieuw spel
            Knop_Nieuwspel = new Button();
            Knop_Nieuwspel.Text = "Nieuw spel";
            Knop_Nieuwspel.Location = new Point(150, 10);
            Knop_Nieuwspel.Size = new Size(100, 35);
            Knop_Nieuwspel.Font = new Font("Arial", 10);
            Knop_Nieuwspel.BackColor = Color.Gray;
            //NieuwSpel.Click += ;
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
            this.Controls.Add(Knop_Nieuwspel);
            this.Controls.Add(Label_RodeSteen);
            this.Controls.Add(Label_BlauweSteen);

            this.speelveld = new Steen[6, 6];
            this.initialiseer_speelveld();

            this.Paint += tekengrid;
            this.Paint += tekenSteen;
            this.MouseClick += controleer_geldige_klik;
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

            Point vak5 = new Point(Xspeelveld + (cellsize * 4), Yspeelveld + (cellsize * 2));
            speelveld[4, 2] = new Steen(1, vak5, cellsize);
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
        }

        private void tekenSteen(object sender, PaintEventArgs pea)
        {
            Graphics gr = pea.Graphics;

            for (int x=0; x < speelveld.GetLength(0); x++)
            {
                for (int y=0; y < speelveld.GetLength(1); y++)
                {
                    if (speelveld[x, y] != null)
                    {
                        speelveld[x, y].kleurSteen(gr);
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
                //geldige klik
                if(is_geldige_zet(arraywaarde))
                {
                    //zet is geldig dus doe de zet.
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

        }
        private Point bereken_array_coordinaat(int muisX,int muisY)
            // Helperfunctie bij heet bepalen van een geldige klik. 
        {
            //maak leeg antwoord aan.
            Point answer = new Point(0, 0);

            double answerX = ((muisX - 100.00) / 60);
            if (answerX < 0)
            {
                answer.X = -1;
            }
            else
            {
                answer.X = (int)answerX;
            }

            double answerY = ((muisY - 200.00) / 60);
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

        
        private bool is_geldige_zet(Point gekliktvakje)
        {
            bool geldigeZet = false;

            int x = 1;

            while (gekliktvakje.X - x >= 0
                   && speelveld[gekliktvakje.X - 1, gekliktvakje.Y] != null)
            {
                if (speelveld[gekliktvakje.X - x, gekliktvakje.Y].speler != HuidigeSpeler)
                    if (speelveld[gekliktvakje.X - x - 1, gekliktvakje.Y].speler == HuidigeSpeler)
                        return true;
                else
                    x++;
            }

          /* if( gekliktvakje.X-2 >= 0 
                && speelveld[gekliktvakje.X - 1, gekliktvakje.Y]!= null
                && speelveld[gekliktvakje.X-1, gekliktvakje.Y].speler != HuidigeSpeler && speelveld[gekliktvakje.X-2, gekliktvakje.Y].speler == HuidigeSpeler)
            {
                 return true;
            } */
            
            return geldigeZet;
        }

    }
    public class Steen
    {
       public int speler;
        Point steenlocatie;
        Size steengrootte;
   
        //constructor
        public Steen(int speler, Point steenlocatie, int cellsize)
        {
            this.speler = speler;
            this.steenlocatie = steenlocatie;
            
            steengrootte = new Size(cellsize, cellsize);

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

