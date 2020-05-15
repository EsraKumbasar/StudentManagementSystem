using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace OgrenciYonetimSistemi
{
    public partial class OgrenciYazdirForm : Form
    {
        public OgrenciYazdirForm()
        {
            InitializeComponent();
        }
        Ogrenci student = new Ogrenci();

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void OgrenciYazdirForm_Load(object sender, EventArgs e)
        {
            fillGrid(new MySqlCommand("SELECT * FROM `student`"));

            if (radioButtonHayir.Checked)
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
            }
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

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void radioButtonHayir_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
        }

        private void radioButtonEvet_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = true;
            dateTimePicker2.Enabled = true;
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            // veriyi kullanıcın seçimine bağlı olarak gosterme
            MySqlCommand command;
            string sorgu;

            // radiobutton evet olup olmadigini kontrol et
            // yani kullanicinin istegine bagli zaman araligi
            if (radioButtonEvet.Checked)
            {
                //zaman degerini al
                string date1 = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                string date2 = dateTimePicker2.Value.ToString("yyyy-MM-dd");

                if (radioButtonErkek.Checked)
                {
                    sorgu = "SELECT * FROM `student` WHERE `birthdate` BETWEEN '" + date1 + "' AND '" + date2 + "' AND gender = 'Erkek'";
                }
                else if (radioButtonKadin.Checked)
                {
                    sorgu = "SELECT * FROM `student` WHERE `birthdate` BETWEEN '" + date1 + "' AND '" + date2 + "' AND gender = 'Kadin'";
                }
                else
                {
                    sorgu = "SELECT * FROM `student` WHERE `birthdate` BETWEEN '" + date1 + "' AND '" + date2 + "'";

                }

                command =new MySqlCommand(sorgu);
                fillGrid(command);
            }
            else // dogum gunu araligi secilmemis hali ile verileri goster
            {
                if (radioButtonErkek.Checked)
                {
                    sorgu = "SELECT * FROM `student` WHERE gender = 'Erkek'";
                }
                else if (radioButtonKadin.Checked)
                {
                    sorgu = "SELECT * FROM `student` WHERE gender = 'Kadin'";
                }
                else
                {
                    sorgu = "SELECT * FROM `student`";

                }

                command = new MySqlCommand(sorgu);
                fillGrid(command);
            }
        }

        //datagridview deki verileri text halinde yazdir
        private void buttonPrint_Click(object sender, EventArgs e)
        {
            //dosya dizini
            //dosya ismi = ogrenciler_liste.txt
            //yer = masaustu
            string yol = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\ogrenciler_liste.txt";

            using(var yazici = new StreamWriter(yol))
            {
                //dosyanin olup olmadigini kontrol et
                if (!File.Exists(yol)){

                    File.Create(yol);
                }

                DateTime bdate;

                //satirlar
                for(int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    //satirlar
                    for(int j = 0; j < dataGridView1.Columns.Count - 1; j++)
                    {
                        //dogum gunu satiri
                        if(j == 3)
                        {
                            bdate = Convert.ToDateTime(dataGridView1.Rows[i].Cells[j].Value.ToString());
                            yazici.Write("\t" + bdate.ToString("yyyy-MM-dd") + "\t" + "|");
                        }

                        //son satir
                        else if(j == dataGridView1.Columns.Count - 2)
                        {
                            yazici.Write("\t" + dataGridView1.Rows[i].Cells[j].Value.ToString());
                        }
                        else
                        {
                            yazici.Write("\t" + dataGridView1.Rows[i].Cells[j].Value.ToString() + "\t" + "|");
                        }

                      

                    }
                    //yeni satir yap
                    yazici.WriteLine("");
                    //ayirma yap
                    yazici.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
                }
             


                yazici.Close();
                MessageBox.Show("Veri Aktarildi!");
            }
        }
    }
}
