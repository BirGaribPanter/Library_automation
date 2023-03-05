using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace kutuphane_otomasyonu_1
{
    public partial class Form1 : Form
    {
        int seri = 0;
        string durum = "0";
        string tarih;
        IDataReader dr;
        OleDbConnection veri;
        OleDbDataAdapter data;
        OleDbCommand cmd;
        DataSet ds;
        void vericek()//verileri çekme işlemi
        {

            veri = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database9.accdb");
            data = new OleDbDataAdapter("Select serino,kitap_adi,verildi_verilmedi,alan_adi,tarih,yazar from kutuphane", veri);
            ds = new DataSet();
            veri.Open();
            data.Fill(ds, "kutuphane");
            dataGridView1.DataSource = ds.Tables["kutuphane"];
            veri.Close();
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Aqua;

            dataGridView1.Columns[0].HeaderText = "serino";
            dataGridView1.Columns[1].HeaderText = "kitap_adi";
            dataGridView1.Columns[2].HeaderText = "verilme durumu";
            dataGridView1.Columns[3].HeaderText = "alan kişinin adı";
            dataGridView1.Columns[4].HeaderText = "tarih";
            dataGridView1.Columns[5].HeaderText = "yazar";

        }
        void kontrol()
        {
            veri = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database9.accdb");
            cmd = new OleDbCommand();
            veri.Open();
            cmd.Connection = veri;
            cmd.CommandText = "SELECT * FROM kutuphane WHERE serino='" + textBox5.Text + "'";
            dr = cmd.ExecuteReader();
        }// Kayıt Kontrolü

        void temizle()
        {
            dateTimePicker1.Value = Convert.ToDateTime(tarih);
            radioButton2.Checked = true;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

        }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            veri = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database9.accdb");
            cmd = new OleDbCommand();
            veri.Open();
            cmd.Connection = veri;
            cmd.CommandText = "SELECT COUNT(*) FROM kutuphane";
            seri = Convert.ToInt32(cmd.ExecuteScalar())+1;
            veri.Close();
            if (textBox1.Text == "")
            {
                temizle();
                MessageBox.Show("KİTAP BİLGİSİ GİRİN!", "HATA");
            }
            else if (textBox2.Text == "")
            {
                temizle();
                MessageBox.Show(" KİTAP BİLGİSİ GİRİN!", "HATA");
            }           

            else
            {
                cmd = new OleDbCommand();
                cmd.Connection = veri;
                veri.Open();
                cmd.CommandText = "insert into kutuphane (serino,kitap_adi,tarih,yazar) values ('" + seri.ToString() + "','" + textBox1.Text + "','" + dateTimePicker1.Value + "','"  + textBox2.Text + "')";
                cmd.ExecuteNonQuery();
                veri.Close();
                temizle();
                vericek();
                MessageBox.Show("KİTAP EKLEME İŞLEMİ BAŞARILI!", "Ekleme İŞlemi Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
        }
       

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                MessageBox.Show("KİTAP BİLGİSİ Boş Bırakılamaz!!", "Boş Bırakma Hatası");
            }
           
            else if (textBox4.Text == "")
            {
                MessageBox.Show("KİŞİ ADI Boş Bırakılamaz!!", "Boş Bırakma Hatası");
            }
          


            else
            {
                kontrol();

                if (dr.Read())
                {

                    cmd = new OleDbCommand();

                    cmd.Connection = veri;
                    cmd.CommandText = "update kutuphane set verildi_verilmedi='" + durum.ToString() + "',alan_adi='" + textBox4.Text +  "' where serino='" + textBox5.Text + "'";
                    cmd.ExecuteNonQuery();
                    veri.Close();
                    vericek();
                    temizle();
                    MessageBox.Show("VERİ Güncelleme İşlemi Başarılı!", "Güncelleme İŞlemi Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("VERİ Bulunamadı", "VERİ Bulunamama Hatası");
                }

            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            tarih = dateTimePicker1.Value.ToString();
            vericek();
            radioButton2.Checked = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox5.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            durum = "1";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            durum = "0";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Length <= 0)
            {
                MessageBox.Show("SERİNO Bırakılamaz!!", "Boş Bırakma Hatası");
            }
            else
            {

                kontrol();

                if (dr.Read())
                {

                    cmd = new OleDbCommand();

                    cmd.Connection = veri;
                    cmd.CommandText = "delete from kutuphane where serino='" + textBox3.Text + "'";
                    cmd.ExecuteNonQuery();
                    veri.Close();
                    vericek();
                    textBox5.Clear();
                    MessageBox.Show("Seçilen VERİ Siilindi", "Silme İşlemi Başarılı");

                }
                else
                {
                    MessageBox.Show("Girilen VERİ Bulunamadı", "VERİ Bulunamama Hatası");
                }


                temizle();

            }
        }
    }
}//Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database9.accdb
