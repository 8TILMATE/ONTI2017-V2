using ONTI2017_V2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ONTI2017_V2
{
    public partial class RezervaAcum : Form
    {
        private int locuri = 0;
        private int locuriramase = 0;
        VacanteModel vacante = new VacanteModel();
        public static List<RezervareModel> rezervariUseful = new List<RezervareModel>();
        private Bitmap map = new Bitmap(600,800);
        public RezervaAcum(VacanteModel vacanta)
        {
            InitializeComponent();
            vacante = vacanta;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Int32.Parse(textBox1.Text) <= locuriramase)
            {
                int pretTotal = (dateTimePicker2.Value.DayOfYear - dateTimePicker1.Value.DayOfYear) * vacante.Pret * Int32.Parse(textBox1.Text);
                RezervareModel rezervare = new RezervareModel
                {
                    IdVacanta = vacante.Id,
                    IdUser = DatabaseHelper.LoggedUser.Id,
                    datastart = dateTimePicker1.Value,
                    dataend = dateTimePicker2.Value,
                    NrPersoane = Int32.Parse(textBox1.Text),
                    PretTotal = pretTotal
                };
                DatabaseHelper.InsertRezervari(rezervare);
                textBox1.Hide();
                button1.Hide();
                dateTimePicker1.Hide();
                dateTimePicker2.Hide();
                label3.Text = DatabaseHelper.LoggedUser.Name;
                label4.Text = DatabaseHelper.LoggedUser.Email;
                label5.Text = dateTimePicker1.Value.ToString();
                label6.Text=dateTimePicker2.Value.ToString();
                this.DrawToBitmap(map,new Rectangle( 0, 0, this.Width, this.Height));
                PrintDialog dialog = new PrintDialog();
                dialog.Document = new System.Drawing.Printing.PrintDocument();
                dialog.Document.PrintPage += Document_PrintPage;
                dialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("Locuri Insuficiente");
            }
        }

        private void Document_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            e.Graphics.DrawImage(map, 0, 0);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label7.Text = ((dateTimePicker2.Value.DayOfYear - dateTimePicker1.Value.DayOfYear) * vacante.Pret * Int32.Parse(textBox1.Text)).ToString();
        }

        private void RezervaAcum_Load(object sender, EventArgs e)
        {
            label2.Text = vacante.NumeVacanta;
            locuriramase = vacante.NrLocuri;
            label3.Text ="Pret: "+  vacante.Pret.ToString();
            GetData();
        
        }
        private void GetData()
        {
            locuriramase = vacante.NrLocuri;
            rezervariUseful.Clear();
            foreach (var x in DatabaseHelper.rezervareModels)
            {
                if (x.dataend > dateTimePicker1.Value)
                {
                    rezervariUseful.Add(x);
                    locuriramase -= x.NrPersoane;
                    label1.Text="Locuri Ramase: "+ locuriramase.ToString();
                }
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            GetData();

        }
    }
}
