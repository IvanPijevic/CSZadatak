using System.Linq;

//Napraviti grafičku aplikaciju na koju korisnik može dodavati točke klikanjem lijeve tipke miša,
//a brisati postojeću točku tako da klikne desnom tipkom miša na nju.
//Svaki puta kada se doda ili ukloni točka, program mora izračunati i iscrtati novi pravac
//linearne regresije izračunat metodom najmanjih kvadrata.

namespace CSZadatak
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "Zadatak";
            this.Text = "Zadatak";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Collections.Generic.List<System.Drawing.PointF> tocke = new System.Collections.Generic.List<System.Drawing.PointF>();
        private System.Drawing.Pen crvenaLinija = new System.Drawing.Pen(System.Drawing.Color.Red, 2);
        private System.Drawing.Pen plavaTocka = new System.Drawing.Pen(System.Drawing.Color.Blue, 5);

        private void Form1_Paint(object o, System.Windows.Forms.PaintEventArgs evnt)
        {
            //crtaj sve tocke
            foreach (var point in tocke)
            {
                evnt.Graphics.DrawEllipse(plavaTocka, point.X - 2, point.Y - 2, 4, 4);
            }

            //Crtanje pravca kod >= 2 tocke
            if (tocke.Count >= 2)
            {
                var (nagib, dio) = CalculateLinearRegression(tocke);

                float x1 = 0;
                float y1 = nagib * x1 + dio;
                float x2 = this.ClientSize.Width;
                float y2 = nagib * x2 + dio;

                evnt.Graphics.DrawLine(crvenaLinija, x1, y1, x2, y2);
            }
        }

        private void Form1_MouseClick(object o, System.Windows.Forms.MouseEventArgs evnt)
        {
            if (evnt.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //Dodaj tocku
                tocke.Add(new System.Drawing.PointF(evnt.X, evnt.Y));
            }

            else if (evnt.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //Makni tocku
                var ukloniTocku = tocke.FirstOrDefault(p => System.Math.Abs(p.X - evnt.X) < 5 && System.Math.Abs(p.Y - evnt.Y) < 5);

                if (ukloniTocku != null)
                {
                    tocke.Remove(ukloniTocku);
                }
            }

            this.Invalidate();
        }

        private (float nagib, float dio) CalculateLinearRegression(System.Collections.Generic.List<System.Drawing.PointF> tocke)
        {
            //Prosjek x-a i y-a
            float prosjekX = tocke.Average(p => p.X);
            float prosjekY = tocke.Average(p => p.Y);

            // izracunaj nagib,dioi
            float brojnik = 0;
            float nazivnik = 0;

            foreach (var tocka in tocke)
            {
                brojnik += (tocka.X - prosjekX) * (tocka.Y - prosjekY);
                nazivnik += (tocka.X - prosjekX) * (tocka.X - prosjekX);
            }

            float nagib = brojnik / nazivnik;
            float dio = prosjekY - nagib * prosjekX;

            return (nagib, dio);
        }
    }
}

