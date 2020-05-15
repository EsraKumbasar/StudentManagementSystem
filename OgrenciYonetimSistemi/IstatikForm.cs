using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OgrenciYonetimSistemi
{
    public partial class IstatikForm : Form
    {
        public IstatikForm()
        {
            InitializeComponent();
        }

        // renk değişkenleri
        Color panTotalColor;
        Color panFemaleColor;
        Color panMaleColor;

        private void IstatikForm_Load(object sender, EventArgs e)
        {
            // Arka fondaki renkler için
            panTotalColor = panelTotal.BackColor;
            panMaleColor = panelMale.BackColor;
            panFemaleColor = panelFemale.BackColor;

            // Değerleri elde etmek için
            Ogrenci student = new Ogrenci();
            double totalStudents = Convert.ToDouble(student.totalStudent());
            double totalMaleStudents = Convert.ToDouble(student.totalMaleStudent());
            double totalFemaleStudents = Convert.ToDouble(student.totalFemaleStudent());

            // % olarak sayım
            double malePercentage = totalMaleStudents * 100 / totalStudents;
            double femalePercentage = totalFemaleStudents * 100 / totalStudents;

            labelTotal.Text = "Toplam Öğrenci: " + totalStudents.ToString();
            labelMale.Text = "Erkek: % " + malePercentage.ToString("0.00");
            labelFemale.Text = "Kadın: % " + femalePercentage.ToString("0.00");
        }

        private void labelTotal_Click(object sender, EventArgs e)
        {

        }

        private void labelTotal_MouseEnter(object sender, EventArgs e)
        {
            panelTotal.BackColor = Color.White;
            labelTotal.ForeColor = panTotalColor;
        }
        //YANLIŞ
        private void panelTotal_MouseLeave(object sender, EventArgs e)
        {

        }

        private void labelMale_MouseEnter(object sender, EventArgs e)
        {
            panelMale.BackColor = Color.White;
            labelMale.ForeColor = panMaleColor;
        }
        //YANLIŞ
        private void panelMale_MouseLeave(object sender, EventArgs e)
        {

        }

        private void labelTotal_MouseLeave(object sender, EventArgs e)
        {
            panelTotal.BackColor = panTotalColor;
            labelTotal.ForeColor = Color.White;
        }

        private void labelMale_MouseLeave(object sender, EventArgs e)
        {
            panelMale.BackColor = panMaleColor;
            labelMale.ForeColor = Color.White;
        }

        private void labelFemale_MouseEnter(object sender, EventArgs e)
        {
            panelFemale.BackColor = Color.White;
            labelFemale.ForeColor = panFemaleColor;
        }

        private void labelFemale_MouseLeave(object sender, EventArgs e)
        {
            panelFemale.BackColor = panFemaleColor;
            labelFemale.ForeColor = Color.White;
        }

        private void labelFemale_Click(object sender, EventArgs e)
        {

        }
    }
}
