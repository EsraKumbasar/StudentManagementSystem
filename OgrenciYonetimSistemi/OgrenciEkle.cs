using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;


namespace OgrenciYonetimSistemi
{
    public partial class OgrenciEkle : Form
    {
        public OgrenciEkle()
        {
            InitializeComponent();
        }

        private void buttonAddStudent_Click(object sender, EventArgs e)
        {
            // yeni ogrenci ekle
            Ogrenci student = new Ogrenci();
            string fname = textBoxFname.Text;
            string lname = textBoxLname.Text;
            DateTime bdate = dateTimePickerBdate.Value;
            string phone = textBoxPhone.Text;
            string address = textBoxAddress.Text;
            string gender = "Erkek";

            if (radioButtonFemale.Checked)
            {
                gender = "Kadın";
            }

            MemoryStream pic = new MemoryStream();

            // Ogrencilerin yaslarini kontrol etmeliyiz.
            // Yaslari 10-100 arasinda olmali

            int born_Year = dateTimePickerBdate.Value.Year;
            int this_Year = DateTime.Now.Year;
            if (((this_Year - born_Year) < 10) || ((this_Year - born_Year) > 100))
            {
                MessageBox.Show("Öğrenci yaşı 10 ile 100 arasında olmalı!","Geçersiz Yaş",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else if (verify())
            {
                pictureBoxStudentImage.Image.Save(pic, pictureBoxStudentImage.Image.RawFormat);

                if (student.insertStudent(fname, lname, bdate, phone, gender, address, pic))
                {
                    MessageBox.Show("Yeni Öğrenci Eklendi", "Yeni Öğrenci", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Hatalı", "Yeni Öğrenci", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Boş Dosya", "Yeni Öğrenci", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }

        private void buttonUploadImage_Click(object sender, EventArgs e)
        {
            //bilgisayarindan resim ara
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Resim seç(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBoxStudentImage.Image = Image.FromFile(ofd.FileName);
            }

        }
        //veriyi dogrulamak icin fonksiyon
        public bool verify()
        {
            if((textBoxFname.Text.Trim() == "") ||
                (textBoxLname.Text.Trim() == "") ||
                (textBoxPhone.Text.Trim() == "") ||
                (textBoxAddress.Text.Trim() == "") ||
                (pictureBoxStudentImage.Image == null))
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        //buton kapat
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBoxStudentImage_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBoxAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBoxPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePickerBdate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBoxLname_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBoxFname_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
