using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace OgrenciYonetimSistemi
{
    public partial class OgrenciYonetimForm : Form
    {
        public OgrenciYonetimForm()
        {
            InitializeComponent();
        }

        Ogrenci student = new Ogrenci();
        private void OgrenciYonetimForm_Load(object sender, EventArgs e)
        {
            // Öğrenci verileriyle birlikte datagridwiev in yoğunluğu
            fillGrid(new MySqlCommand("SELECT * FROM `student`"));
        }

        // datagridview de yoğuluğu gösteren fonksiyon
        public void fillGrid(MySqlCommand command)
        {
            dataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.DataSource = student.getStudents(command);
            // column 7 >> fotoğrafın büyüklüğünü ayarlıyor
            picCol = (DataGridViewImageColumn)dataGridView1.Columns[7];
            picCol.ImageLayout = DataGridViewImageCellLayout.Stretch;

            dataGridView1.AllowUserToAddRows = false;

            // dgv rows a göre değişen öğrencini sayısını göster
            labelTotalStudent.Text = "Toplam Öğrenci: " + dataGridView1.Rows.Count;
        }

        // dataGridView_Click ile öğrenci verilerini göster
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            textBoxID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBoxFname.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBoxLname.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();

            dateTimePickerBdate.Value = (DateTime)dataGridView1.CurrentRow.Cells[3].Value;

            if(dataGridView1.CurrentRow.Cells[4].Value.ToString() == "Kadın")
            {
                radioButtonFemale.Checked = true;
            }
            else
            {
                radioButtonMale.Checked = false;
            }
            textBoxPhone.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBoxAddress.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();

            byte[] pic;
            pic = (byte[])dataGridView1.CurrentRow.Cells[7].Value;
            MemoryStream picture = new MemoryStream(pic);
            pictureBoxStudentImage.Image = Image.FromStream(picture);

        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            textBoxID.Text = "";
            textBoxFname.Text = "";
            textBoxLname.Text = "";
            textBoxPhone.Text = "";
            textBoxAddress.Text = "";
            dateTimePickerBdate.Value = DateTime.Now;
            pictureBoxStudentImage.Image = null;
            radioButtonFemale.Checked = true;
        }

        // Arama ve Öğrencilerin verilerini göstermek için
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM `student` WHERE CONCAT(`first_name`,`last_name`,`address`) LIKE'%" + textBoxSearch.Text + "%'";
            MySqlCommand command = new MySqlCommand(query);
            fillGrid(command);
        }
        // Picturebox a bilgisayardan fotoğraf görüntüleme ve arama
        private void buttonUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Resim seç(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBoxStudentImage.Image = Image.FromFile(ofd.FileName);
            }

        }
        // Fotoğtafı bilgisayara kaydetme
        private void buttonDownload_Click(object sender, EventArgs e)
        {
            SaveFileDialog svf = new SaveFileDialog();
            // Dosya ismi için
            svf.FileName = "Öğrenci " + textBoxID.Text;

            //Picturebox ın boş olup olmadığını kontrol etmek için
            if(pictureBoxStudentImage.Image == null)
            {
                MessageBox.Show("PictureBox'ta resim yok");
            }
            else if(svf.ShowDialog() == DialogResult.OK)
            {
                pictureBoxStudentImage.Image.Save(svf.FileName + ("." + ImageFormat.Jpeg.ToString()));
            }
        }

        // Yeni öğrenci ekle
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
                MessageBox.Show("Öğrenci yaşı 10 ile 100 arasında olmalı!", "Geçersiz Yaş", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (verify())
            {
                pictureBoxStudentImage.Image.Save(pic, pictureBoxStudentImage.Image.RawFormat);

                if (student.insertStudent(fname, lname, bdate, phone, gender, address, pic))
                {
                    MessageBox.Show("Yeni Öğrenci Eklendi", "Yeni Öğrenci", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    fillGrid(new MySqlCommand("SELECT * FROM `student`"));
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

        // Seçilen öğrencinin verilerini düzenle
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
                        fillGrid(new MySqlCommand("SELECT * FROM `student`"));
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

        // Seçilen Öğrenciyi sil
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
                        fillGrid(new MySqlCommand("SELECT * FROM `student`"));
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
    }
}
