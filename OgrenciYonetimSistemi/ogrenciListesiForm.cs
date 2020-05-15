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
    public partial class ogrenciListesiForm : Form
    {
        public ogrenciListesiForm()
        {
            InitializeComponent();
        }

        Ogrenci ogrenci = new Ogrenci();
        private void ogrenciListesiForm_Load(object sender, EventArgs e)
        {
            // datagridview ile öğrenci verilerinin yoğunluğunu göstericez.
            MySqlCommand command = new MySqlCommand("SELECT * FROM `student`");
            dataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.DataSource = ogrenci.getStudents(command);
            // column 7 >> fotoğrafın büyüklüğünü ayarlıyor
            picCol = (DataGridViewImageColumn)dataGridView1.Columns[7];
            picCol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            // düzenlen/siline öğrencileri tabloda görüntüle
            ogrenciEkleSilForm updateDeleteStdf = new ogrenciEkleSilForm();
            updateDeleteStdf.textBoxID.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            updateDeleteStdf.textBoxFname.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            updateDeleteStdf.textBoxLname.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            updateDeleteStdf.dateTimePickerBdate.Value = (DateTime)dataGridView1.CurrentRow.Cells[3].Value;

            //cinsiyet
            if(dataGridView1.CurrentRow.Cells[4].Value.ToString() == "Female")
            {
                updateDeleteStdf.radioButtonFemale.Checked = true;

            }

            updateDeleteStdf.textBoxPhone.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            updateDeleteStdf.textBoxAddress.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();

            //fotoğraf
            byte[] pic;
            pic = (byte[])dataGridView1.CurrentRow.Cells[7].Value;
            MemoryStream picture = new MemoryStream(pic);
            updateDeleteStdf.pictureBoxStudentImage.Image = Image.FromStream(picture);
            updateDeleteStdf.Show();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            // datagridview verisini yenile
            MySqlCommand command = new MySqlCommand("SELECT * FROM `student`");
            dataGridView1.ReadOnly = true;
            DataGridViewImageColumn picCol = new DataGridViewImageColumn();
            dataGridView1.RowTemplate.Height = 80;
            dataGridView1.DataSource = ogrenci.getStudents(command);
            // column 7 >> fotoğrafın büyüklüğünü ayarlıyor
            picCol = (DataGridViewImageColumn)dataGridView1.Columns[7];
            picCol.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.AllowUserToAddRows = false;
        }
    }
}
