using ONTI2017_V2.Models;
using ONTI2017_V2.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ONTI2017_V2
{
    public partial class FrmVacanta : Form
    {
        public int indexShowcase = 0;
        public List<int> ints = new List<int>();
        public List<Color> colors = new List<Color>();
        public FrmVacanta()
        {
            InitializeComponent();
        }

        private void FrmVacanta_Load(object sender, EventArgs e)
        {
            if (DatabaseHelper.LoggedUser.TipCont == 1)
            {
                tabControl1.TabPages.RemoveAt(0);
            }
            DatabaseHelper.vacanteModels.Clear();
            DatabaseHelper.GetVacante();
            foreach (VacanteModel model in DatabaseHelper.vacanteModels) 
            {
                    DatabaseHelper.GetAllRezervariData(model);
                    foreach(RezervareModel model1 in DatabaseHelper.vacantelemele)
                    {
                        if (model1.IdVacanta == model.Id)
                         {
                        ints.Add(model.Id);
                        dataGridView1.Rows.Add(model.NumeVacanta, model1.datastart, model1.dataend, model1.NrPersoane, model1.PretTotal);
                         }
                    }
             }
            
            
        }
        private void LoadVacante(int index)
        {
            label1.Text = DatabaseHelper.vacanteModels[index].NumeVacanta;
            label2.Text = DatabaseHelper.vacanteModels[index].Pret.ToString();
            label3.Text = DatabaseHelper.vacanteModels[index].NrLocuri.ToString();
            label4.Text = DatabaseHelper.vacanteModels[index].Descriere;
            pictureBox1.ImageLocation = DatabaseHelper.vacanteModels[index].CaleFisier;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (indexShowcase < DatabaseHelper.vacanteModels.Count-1)
            {
                indexShowcase++;
                LoadVacante(indexShowcase);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (indexShowcase > 0)
            {
                indexShowcase--;
                LoadVacante(indexShowcase);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(indexShowcase< DatabaseHelper.vacanteModels.Count-1)
            {
                indexShowcase++;
            }
            else
            {
                indexShowcase = 0;
            }
            LoadVacante(indexShowcase);
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DatabaseHelper.rezervareModels.Clear();
            DatabaseHelper.GetAllRezervariData(DatabaseHelper.vacanteModels[indexShowcase]);
            RezervaAcum rezervaAcum = new RezervaAcum(DatabaseHelper.vacanteModels[indexShowcase]);
            rezervaAcum.ShowDialog();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ConvertUsers convertUsers = new ConvertUsers();
            convertUsers.ShowDialog(); 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Resources.vacanteString);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            if(e.RowIndex>=0 && dataGridView.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                DatabaseHelper.DeleteRezervari("Rezervari", ints[e.RowIndex]);
                dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DatabaseHelper.LoggedUser = new UserModel();

            this.Close();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            pictureBox1.DrawToBitmap(bmp,new Rectangle(0,0,bmp.Width,bmp.Height));
          for(int i =0;i< bmp.Height;i++)
            {
                for(int j=0;j<bmp.Width;j++)
                {
                    Color color = bmp.GetPixel(j,i);
                    int Lightness = (int)Math.Sqrt(0.241 * Math.Pow(Double.Parse(color.R.ToString()), 2) + 0.691 * Math.Pow(Double.Parse(color.G.ToString()), 2) + 0.068 * Math.Pow(Double.Parse(color.B.ToString()), 2));
                    if (Lightness > 200||Lightness<50)
                    {
                        colors.Add(color);
                    }
                }
            }
          int startj= bmp.Height/2-colors.Count/bmp.Width/2;
            int starti = 0;
          for(int i =0;i< colors.Count;i++)
            {
                if (starti < bmp.Width)
                {
                    bmp.SetPixel(starti, startj, colors[i]);
                    starti++;
                }
                else
                {
                    starti = 0;
                    startj++;
                }
            }
            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = bmp.Size;
            pictureBox.SizeMode=PictureBoxSizeMode.StretchImage;
            pictureBox.Image = bmp;
            Graphics g = pictureBox.CreateGraphics();
            SolidBrush brush = new SolidBrush(Color.White);
            g.DrawString(DatabaseHelper.vacanteModels[indexShowcase].NumeVacanta, this.Font,
            new SolidBrush(Color.Black), new PointF(bmp.Width / 2, bmp.Height / 2));
            g.Dispose();
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
            pictureBox.Image.Save(sfd.FileName);
        }
    }
}
