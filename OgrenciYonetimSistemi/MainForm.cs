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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void yeniÖğrenciEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OgrenciEkle ogrenciEkle = new OgrenciEkle();
            ogrenciEkle.Show(this);
        }

        private void istatistiklerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IstatikForm iF = new IstatikForm();
            iF.Show(this);
        }

        private void öğrenciListesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ogrenciListesiForm slf = new ogrenciListesiForm();
            slf.Show(this);
        }

        private void düzenleSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ogrenciEkleSilForm ogrenciEkle = new ogrenciEkleSilForm();
            ogrenciEkle.Show(this);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void öğrenciYönetimFormuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OgrenciYonetimForm ogrenciYonetim = new OgrenciYonetimForm();
            ogrenciYonetim.Show(this);
        }

        private void yazdırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OgrenciYazdirForm oyf = new OgrenciYazdirForm();
            oyf.Show(this);
        }

    }
}
