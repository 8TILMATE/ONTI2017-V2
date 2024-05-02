using ONTI2017_V2.Properties;
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

namespace ONTI2017_V2
{
    public partial class FrmAutentificare : Form
    {
        public FrmAutentificare()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DatabaseHelper.InsertDB();
            StreamReader rdr = new StreamReader(Resources.rememberMeString);
            var line = rdr.ReadLine();
            if (line != null)
            {
                var line2 = line.Split(';');
                textBox1.Text = line2[0];   
                textBox2.Text= line2[1];
                rdr.Close();
                StreamWriter wrt = new StreamWriter(Resources.rememberMeString);
                wrt.Write(string.Empty);
                wrt.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (DatabaseHelper.CheckUser(textBox1.Text, textBox2.Text))
            {
                if(checkBox1.Checked)
                {
                    StreamWriter wrt = new StreamWriter(Resources.rememberMeString);
                    wrt.Write(textBox1.Text + ";" + textBox2.Text);
                    wrt.Close();
                }
                MessageBox.Show("Logged");
                FrmVacanta vacanta = new FrmVacanta();
                vacanta.ShowDialog();
            }
            else
            {
                MessageBox.Show("Eroare la autentificare!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmInregistrare inregistrare = new FrmInregistrare();
            inregistrare.ShowDialog();
            
        }
    }
}
