using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataViewOrnek3
{
    public partial class Form1 : Form
    {
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;
        DataView dataView;
        string ConnectionString = "Provider=Microsoft.ACE.Oledb.12.0;Data Source=kullanici.accdb";
        public Form1()
        {
            InitializeComponent();
            Veri_Cek();
        }
        void Veri_Cek()
        {
            using (OleDbConnection baglanti = new OleDbConnection(ConnectionString))
            {

                dataView = new DataView();
                ds = new DataSet();
                cmd = new OleDbCommand("SELECT * FROM Kullanici", baglanti);
                da = new OleDbDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds, "Add New");
                dataView = new DataView(ds.Tables[0]);
                dataView.Sort = "Id";
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataView.Count; i++)
            {
                if (kadi.Text == dataView[i][1].ToString() && sifre.Text == dataView[i][2].ToString())
                {
                    MessageBox.Show("Merhaba, " + dataView[i][1].ToString() + ". Başarıyla giriş yaptınız.");
                    return;
                }
            }
            MessageBox.Show("Kullanici adınız veya şifreniz yanlış. Lütfen tekrar deneyiniz.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Kullanici_Ekle(kadi.Text, sifre.Text);
        }

        void Kullanici_Ekle(string kadi, string sifre)
        {
            try
            {
                DataRowView newRow = dataView.AddNew();
                newRow["Kullanici_Adi"] = kadi;
                newRow["Sifre"] = sifre;
                newRow.EndEdit();
                dataView.Sort = "Id";

                using (OleDbConnection baglanti = new OleDbConnection(ConnectionString))
                {
                    OleDbCommand com = new OleDbCommand("INSERT INTO Kullanici (Kullanici_Adi,Sifre) VALUES (@Kullanici_Adi,@Sifre)");
                    com.Connection = baglanti;
                    baglanti.Open();
                    if (baglanti.State == ConnectionState.Open)
                    {
                        com.Parameters.Add("@Kullanici_Adi", OleDbType.VarChar).Value = dataView[0][1];
                        com.Parameters.Add("@Sifre", OleDbType.VarChar).Value = dataView[0][2];
                        try
                        {            
                            com.ExecuteNonQuery();
                            MessageBox.Show("Başarıyla kayıt oldunuz.");
                            Veri_Cek();
                        }
                        catch (OleDbException ex)
                        {
                            MessageBox.Show(ex.Source);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bağlantı Hatası");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
