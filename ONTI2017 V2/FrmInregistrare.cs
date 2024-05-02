using ONTI2017_V2.Models;
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
    public partial class FrmInregistrare : Form
    {
        public List<string> locatiiCaptcha = new List<string>();
        public string RaspunsCaptcha=string.Empty;
        public FrmInregistrare()
        {
            InitializeComponent();
        }

        private void FrmInregistrare_Load(object sender, EventArgs e)
        {
            foreach(var item in Directory.GetFiles(Resources.logareString))
            {
                locatiiCaptcha.Add(item);
            }
            Random rand = new Random();
            int ix = rand.Next(locatiiCaptcha.Count);
            pictureBox1.ImageLocation = locatiiCaptcha[ix];
            RaspunsCaptcha = locatiiCaptcha[ix];


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (RaspunsCaptcha.Contains(textBox6.Text + ".png")&&(textBox4.Text==textBox5.Text))
            {
                UserModel model = new UserModel
                {
                    Name = textBox1.Text,
                    LastName = textBox2.Text,
                    Email = textBox3.Text,
                    Password = textBox4.Text,
                    TipCont = 0
                };
                DatabaseHelper.InsertUser(model);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
