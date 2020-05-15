using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;


namespace OgrenciYonetimSistemi
{
    class Ogrenci
    {
        MY_DB db = new MY_DB();

       

        // Yeni ogrenci eklemek icin fonksiyon
        public bool insertStudent(string fname,string lname,DateTime bdate,string phone,string gender,string address,MemoryStream picture)
        {

            MySqlCommand command = new MySqlCommand("INSERT INTO `student`(`first_name`, `last_name`, `birthdate`, `gender`, `phone`, `address`, `picture`) VALUES (@fn,@ln,@bdt,@gdr,@phn,@adrs,@pic)", db.getConncection);

            //@fn,@ln,@bdt,@gdr,@phn,@adrs,@pic
            command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = fname;
            command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = lname;
            command.Parameters.Add("@bdt", MySqlDbType.Date).Value = bdate;
            command.Parameters.Add("@gdr", MySqlDbType.VarChar).Value = gender;
            command.Parameters.Add("@phn", MySqlDbType.VarChar).Value = phone;
            command.Parameters.Add("@adrs", MySqlDbType.Text).Value = address;
            command.Parameters.Add("@pic", MySqlDbType.LongBlob).Value = picture.ToArray();

            db.openConncetion();

            if(command.ExecuteNonQuery() == 1)
            {
                db.closeConncetion();
                return true;
            }
            else
            {
                db.closeConncetion();
                return false;
            }
           
        }

        // ogrenci verilerinin tablosunu oluşturmak için fonksiyon oluşturacağız.
        public DataTable getStudents(MySqlCommand command)
        {
            command.Connection = db.getConncection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }

        // ogrenci verilerini güncellemek için fonksiyon
        public bool updateStudent(int id, string fname, string lname, DateTime bdate, string phone, string gender, string address, MemoryStream picture)
        {
            MySqlCommand command = new MySqlCommand("UPDATE `student` SET `first_name`=@fn,`last_name`=@ln,`birthdate`=@bdt,`gender`=@gdr,`phone`=@phn,`address`=@adrs,`picture`=@pic WHERE `id`=@ID", db.getConncection);

            //@ID,@fn,@ln,@bdt,@gdr,@phn,@adrs,@pic
            command.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;
            command.Parameters.Add("@fn", MySqlDbType.VarChar).Value = fname;
            command.Parameters.Add("@ln", MySqlDbType.VarChar).Value = lname;
            command.Parameters.Add("@bdt", MySqlDbType.Date).Value = bdate;
            command.Parameters.Add("@gdr", MySqlDbType.VarChar).Value = gender;
            command.Parameters.Add("@phn", MySqlDbType.VarChar).Value = phone;
            command.Parameters.Add("@adrs", MySqlDbType.Text).Value = address;
            command.Parameters.Add("@pic", MySqlDbType.LongBlob).Value = picture.ToArray();

            db.openConncetion();

            if (command.ExecuteNonQuery() == 1)
            {
                db.closeConncetion();
                return true;
            }
            else
            {
                db.closeConncetion();
                return false;
            }
        }
         
        // ogrenci verilerini silmek için fonksiyon
        public bool deleteStudent(int id)
        {
            MySqlCommand command = new MySqlCommand("DELETE FROM `student` WHERE `id`=@studentID", db.getConncection);

            //@studentID
            command.Parameters.Add("@studentID", MySqlDbType.Int32).Value = id;
           

            db.openConncetion();

            if (command.ExecuteNonQuery() == 1)
            {
                db.closeConncetion();
                return true;
            }
            else
            {
                db.closeConncetion();
                return false;
            }
        }

        // Sayımı sorgulatmak için fonksiyon
        public string execCount(string query)
        {
            MySqlCommand command = new MySqlCommand(query,db.getConncection);

            db.openConncetion();
            string count = command.ExecuteScalar().ToString();
            db.closeConncetion();

            return count;
        }

        // Toplam öğrenci sayısını elde etmek için 
        public string totalStudent()
        {
            return execCount("SELECT COUNT(*) FROM `student`");
        }

        // Erkek öğrenci sayısını elde etmek için 
        public string totalMaleStudent()
        {
            return execCount("SELECT COUNT(*) FROM `student` WHERE `gender`='Erkek'");
        }

        // Kız öğrenci sayısını elde etmek için 
        public string totalFemaleStudent()
        {
            return execCount("SELECT COUNT(*) FROM `student` WHERE `gender`='Kadın'");
        }
    }
}
