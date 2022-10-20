using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Maliyet_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-5FU8Q6M;Initial Catalog=Test_Maliyet;Integrated Security=True");
        
        void MalzemeListe()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * From tblmalzemeler", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        void UrunListe()
        {
            SqlDataAdapter da2 = new SqlDataAdapter("Select * From tblurunler", baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView1.DataSource = dt2;

        }

        void Kasa()
        {
            SqlDataAdapter da3 = new SqlDataAdapter("Select * From tblkasa", baglanti);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            dataGridView1.DataSource = dt3;
        }
        void Urunler()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select * From tblurunler", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbUrun.ValueMember = "URUNID";
            CmbUrun.DisplayMember = "AD";
            CmbUrun.DataSource = dt;
            baglanti.Close();
        }

        void Malzemeler()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select * From tblmalzemeler", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbMalzeme.ValueMember = "MALZEMEID";
            CmbMalzeme.DisplayMember = "AD";
            CmbMalzeme.DataSource = dt;
            baglanti.Close();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            MalzemeListe();
            Urunler();
            Malzemeler();
        }

        private void BtnMalzemeListesi_Click(object sender, EventArgs e)
        {
            MalzemeListe();
        }

        private void BtnUrunListesi_Click(object sender, EventArgs e)
        {
            UrunListe();
        }

        private void BtnKasa_Click(object sender, EventArgs e)
        {
            Kasa();
        }

        private void BtnCıkıs_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnMalzemeEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into tblmalzemeler (ad,stok,fiyat,notlar) values (@p1,@p2,@p3,@p4)", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtMalzemeAd.Text);
            komut.Parameters.AddWithValue("@p2", decimal.Parse(TxtMalzemeStok.Text));
            komut.Parameters.AddWithValue("@p3", decimal.Parse(TxtMalzemeFiyat.Text));
            komut.Parameters.AddWithValue("@p4", TxtMalzemeNotlar.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Malzeme Sisteme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            MalzemeListe();
        }

        private void BtnUrunEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into tblurunler (ad) values (@p1)", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtUrunAd.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürün Sisteme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UrunListe();
        }

        private void BtnUrunOlustur_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into tblfırın(urunıd,malzemeıd,mıktar,malıyet) values (@p1,@p2,@p3,@p4)", baglanti);
            komut.Parameters.AddWithValue("@p1", CmbUrun.SelectedValue);
            komut.Parameters.AddWithValue("@p2", CmbMalzeme.SelectedValue);
            komut.Parameters.AddWithValue("@p3", decimal.Parse(TxtMiktar.Text));
            komut.Parameters.AddWithValue("@p4", decimal.Parse(TxtMaaliyet.Text));
            komut.ExecuteNonQuery();
            MessageBox.Show("Malzeme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            listBox1.Items.Add(CmbMalzeme.Text + " - " + TxtMaaliyet.Text);
        }

        private void TxtMiktar_TextChanged(object sender, EventArgs e)
        {
            double maliyet;
            if(TxtMiktar.Text == "")
            {
                TxtMiktar.Text = "0";
            }

            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select * From tblmalzemeler where Malzemeıd=@p1", baglanti);
            komut.Parameters.AddWithValue("@p1", CmbMalzeme.SelectedValue);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                TxtMaaliyet.Text = dr[3].ToString();

            }
            baglanti.Close();

            maliyet = Convert.ToDouble(TxtMaaliyet.Text) / 1000 * Convert.ToDouble(TxtMiktar.Text);

            TxtMaaliyet.Text = maliyet.ToString();


        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            TxtUrunId.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TxtUrunAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();

            baglanti.Open();


            SqlCommand komut = new SqlCommand("Select sum (Malıyet) from tblfırın where urunıd=@p1", baglanti);

            komut.Parameters.AddWithValue("@p1", TxtUrunId.Text);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                TxtMaaliyetFiyat.Text = dr[0].ToString();
            }
            baglanti.Close();
        }
    }
}
