using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CookieClickers
{
    class CookieClickers : Form
    {
        private PictureBox Cookie = new PictureBox();
        private float Cookieamount = 0;
        private float CookieRate = 1;
        private float MoneyAmount = 0;
        private float DoubleMultiplierPrice = 50;
        private float TripleMultiplierPrice = 150;
        private TextReader CookieNumbers;
        private bool EnoughMoney = false;
        SoundPlayer SoundPlayer = new SoundPlayer();
        Button Doublemultiplier = new Button();
        Button Triplemultiplier = new Button();
        Image cookieimage = Image.FromFile("Resources/richsfoodservice.png");
        StringFormat centernumbers = new StringFormat();
        Button savebutton = new Button();
        static void Main() => Application.Run(new CookieClickers());
        public CookieClickers()
        {
            using (CookieNumbers = File.OpenText("numvalues.txt"))
            {
                Cookieamount = float.Parse(CookieNumbers.ReadLine());
                CookieRate = float.Parse(CookieNumbers.ReadLine());
                MoneyAmount = float.Parse(CookieNumbers.ReadLine());
                DoubleMultiplierPrice = float.Parse(CookieNumbers.ReadLine());
                TripleMultiplierPrice = float.Parse(CookieNumbers.ReadLine());
            }
            Controls.Add(Cookie);
            Text = "Cookie Clicker";
            centernumbers.Alignment = StringAlignment.Center;
            centernumbers.LineAlignment = StringAlignment.Center;
            CookieData();
            UpgradeArea();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Size = new Size(500, 500);
            savebutton.Text = "save";
            savebutton.Location = new Point(50, 50);
            savebutton.Size = new Size(70, 50);
            savebutton.Click += new EventHandler(Save);
            Controls.Add(savebutton);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics grfx = e.Graphics;
            Brush brush = new SolidBrush(Color.Black);
            grfx.DrawString(
                Cookieamount.ToString("################################" +
                "########################################################" +
                "#########################################################" +
                "#####################################################################################################"),
                Font,
                brush,
                (ClientSize.Width / 2) - 95,
                (ClientSize.Height / 3) - 35,
                centernumbers);

            grfx.DrawString
                ($"Money: {MoneyAmount.ToString("######################################################################################################################################################################################################################################################")}",
                Font,
                brush,
                0, 0);
            Doublemultiplier.Text = "Cookie Tree (x2 multiplier)" + Environment.NewLine + $"{FormatMoneyNumber(DoubleMultiplierPrice)}";
            Triplemultiplier.Text = "Cookie Field (x3 multiplier)" + Environment.NewLine + $"{FormatMoneyNumber(TripleMultiplierPrice)}";
        }
        void MouseDownEvent(object obj, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Cookie.Size = new Size(95, 95);
                Cookieamount += CookieRate;
                MoneyAmount += CookieRate;
                Invalidate();
            }
        }
        void MouseUpEvent(object obj, EventArgs e)
        {
            Cookie.Size = new Size(100, 100);
        }
        void Save(object obj, EventArgs e)
        {
            File.WriteAllText("numvalues.txt", Cookieamount + Environment.NewLine + CookieRate + Environment.NewLine + MoneyAmount + Environment.NewLine + DoubleMultiplierPrice + Environment.NewLine + TripleMultiplierPrice);
        }
        void UpgradeArea()
        {
            Size buttonsize = new Size(140, 50);
            FlowLayoutPanel upgradePanel = new FlowLayoutPanel();
            upgradePanel.BorderStyle = BorderStyle.Fixed3D;
            upgradePanel.Location = new Point(ClientSize.Width - 20, 80);
            upgradePanel.Size = new Size(150, 300);
            upgradePanel.FlowDirection = FlowDirection.TopDown;
            upgradePanel.AutoScroll = true;
            Controls.Add(upgradePanel);
            Doublemultiplier.Size = buttonsize;
            Triplemultiplier.Size = buttonsize;
            Doublemultiplier.MouseDown += DoubleMultiplierHandler;
            upgradePanel.Controls.Add(Doublemultiplier);
            Triplemultiplier.MouseDown += TripleMultiplierHandler;
            upgradePanel.Controls.Add(Triplemultiplier);
        }
        void CookieData()
        {
            Cookie.Location = new Point((ClientSize.Width / 2) - 45, ClientSize.Height / 2);
            Cookie.Size = new Size(100, 100);
            Cookie.Image = cookieimage;
            Cookie.SizeMode = PictureBoxSizeMode.Zoom;
            Cookie.BackColor = Color.Transparent;
            Cookie.MouseDown += MouseDownEvent;
            Cookie.MouseUp += MouseUpEvent;
        }
        float MultiplierAndPriceCalculator(float MultiplierPrice, float MultiplierValue) //Increases the rate of earned cookies, as well as price. Subtracts price from money total.
        {
            if (MoneyAmount >= MultiplierPrice)
            {
                CookieRate = CookieRate + MultiplierValue;
                MoneyAmount = MoneyAmount - MultiplierPrice;
                EnoughMoney = true;
                Invalidate();
            }
            else
            {
                MessageBox.Show("Sorry, you do not have enough money to purchase this.", ":(");
            }
            return CookieRate;
        }
        void DoubleMultiplierHandler(object obj, EventArgs e)
        {
            MultiplierAndPriceCalculator(DoubleMultiplierPrice, 2);
            if (EnoughMoney)
            {
                DoubleMultiplierPrice *= 2;
                EnoughMoney = false;
            }
        }
        void TripleMultiplierHandler(object obj, EventArgs e)
        {
            MultiplierAndPriceCalculator(TripleMultiplierPrice, 3);
            if (EnoughMoney)
            {
                TripleMultiplierPrice *= 3;
                EnoughMoney = false;
            }
        }
        string FormatMoneyNumber(float money)
        {
            // Septillions
            if (money > 1000000000000000000)
            {
                return Math.Round(money / 1000000000000000000, 2).ToString() + "S";
            }
            // Quadrillions
            if (money > 1000000000000000)
            {
                return Math.Round(money / 1000000000000000, 2).ToString() + "Q";
            }
            // Trillions
            if (money > 1000000000000)
            {
                return Math.Round(money / 1000000000000, 2).ToString() + "T";
            }
            // Billions
            if (money > 1000000000)
            {
                return Math.Round(money / 1000000000, 2).ToString() + "B";
            }
            // Millions
            if (money > 1000000)
            {
                return Math.Round(money / 1000000, 2).ToString() + "M";
            }
            return money.ToString();
        }
    }
}