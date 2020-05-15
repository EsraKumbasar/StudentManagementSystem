using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace OgrenciYonetimSistemi
{
    class MY_DB
    {
        // baglanti
       private MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=;database=csharp_student_db");
    
       // baglanmak icin fonksiyon
       public MySqlConnection getConncection
        {
            get
            {
                return con;
            }
        }

        // baglantiyi acmak icin fonksiyon
        public void openConncetion()
        {
            if(con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }

        // baglantiyi kapatmak icin fonksiyon
        public void closeConncetion()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }



    }
}
