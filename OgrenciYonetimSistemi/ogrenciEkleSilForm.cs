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
using MySql.Data.MySqlClient;

namespace OgrenciYonetimSistemi
{
    public partial class ogrenciEkleSilForm : Form
    {
        public ogrenciEkleSilForm()
        {
            InitializeComponent();
        }

        Ogrenci student = new Ogrenci();

        private void ogrenciEkleSilForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonUploadImage_Click(object sender, EventArgs e)
        {

            //bilgisayarindan resim ara
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Resim seç(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBoxStudentImage.Image = Image.FromFile(ofd.FileName);
            }

        }
        //veriyi dogrulamak icin fonksiyon
        public bool verify()
        {
            if ((textBoxFname.Text.Trim() == "") ||
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


        private void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                // Seçilen öğrencileri güncelle
                // yeni ogrenci ekle

                int id = Convert.ToInt32(textBoxID.Text);
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
                    MessageBox.Show("Öğrenci yaşı 10 ile 100 arasında olmalı!", "Geçersiz Yaş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (verify())
                {
                    pictureBoxStudentImage.Image.Save(pic, pictureBoxStudentImage.Image.RawFormat);

                    if (student.updateStudent(id, fname, lname, bdate, phone, gender, address, pic))
                    {
                        MessageBox.Show("Öğrenci Bilgileri Güncellendi", "Öğrenci Düzenleme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Hatalı", "Öğrenci Düzenleme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Boş Dosya", "Öğrenci Düzenleme", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch
            {
                MessageBox.Show("Lütfen geçerli öğrenci id'si giriniz!", "Öğrenci Düzenleme", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // Öğrenci verilerini silme
                int id = Convert.ToInt32(textBoxID.Text);
                // Öğrenci verilerini silmeden önce bilgilendirme mesajı
                if (MessageBox.Show("Bu öğrenciyi silmek istediğinden emin misin", "Öğrenci Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (student.deleteStudent(id))
                    {
                        MessageBox.Show("Öğrenci Silindi", "Öğrenci Silme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // dosyaları temizleme
                        textBoxID.Text = "";
                        textBoxFname.Text = "";
                        textBoxLname.Text = "";
                        textBoxPhone.Text = "";
                        textBoxAddress.Text = "";
                        dateTimePickerBdate.Value = DateTime.Now;
                        pictureBoxStudentImage.Image = null;

                    }
                    else
                    {
                        MessageBox.Show("Öğrenci Silinemedi", "Öğrenci Silme", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch 
            {
                MessageBox.Show("Lütfen geçerli öğrenci id'si giriniz!", "Öğrenci Silme", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            try
            {
                // id ile öğrenciyi arama
                int id = Convert.ToInt32(textBoxID.Text);
                MySqlCommand command = new MySqlCommand("SELECT `id`, `first_name`, `last_name`, `birthdate`, `gender`, `phone`, `address`, `picture` FROM `student` WHERE `id`=" + id);

                DataTable table = student.getStudents(command);

                if (table.Rows.Count > 0)
                {
                    textBoxFname.Text = table.Rows[0]["first_name"].ToString();
                    textBoxLname.Text = table.Rows[0]["last_name"].ToString();
                    textBoxPhone.Text = table.Rows[0]["phone"].ToString();
                    textBoxAddress.Text = table.Rows[0]["address"].ToString();

                    dateTimePickerBdate.Value = (DateTime)table.Rows[0]["birthdate"];

                    //cinsiyet
                    if (table.Rows[0]["gender"].ToString() == "Female")
                    {
                        radioButtonFemale.Checked = true;
                    }
                    else
                    {
                        radioButtonMale.Checked = true;
                    }

                    //resim
                    byte[] pic = (byte[])table.Rows[0]["picture"];
                    MemoryStream picture = new MemoryStream(pic);
                    pictureBoxStudentImage.Image = Image.FromStream(picture);

                }
            }
            catch
            {
                MessageBox.Show("Lütfen geçerli öğrenci id'si giriniz!", "Geçersiz ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        
        private void buttonFind_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        // Sadece sayılı tuşların basılmasına izin verir
        private void textBoxID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
