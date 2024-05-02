using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ONTI2017_V2
{
    public partial class ConvertUsers : Form
    {
        public ConvertUsers()
        {
            InitializeComponent();
        }

        private void ConvertUsers_Load(object sender, EventArgs e)
        {
            DatabaseHelper.GetUsers();
            foreach(var x in DatabaseHelper.userModels)
            {
                comboBox1.Items.Add(x.Email);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string email = comboBox1.SelectedItem.ToString();
            foreach(var x in DatabaseHelper.userModels)
            {
                if(x.Email == email)
                {
                    DatabaseHelper.UpdateUser(x);
                    break;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmInregistrare inregistrare = new FrmInregistrare();
            inregistrare.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
