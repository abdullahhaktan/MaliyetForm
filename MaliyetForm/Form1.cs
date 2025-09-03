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

namespace MaliyetForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=ABDULLAH;Initial Catalog=DbMaliyet;Integrated Security=True;Encrypt=False;");

        bool durum;

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        void urunleriGetir()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM TBL_URUNLER", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cmbUrun.DisplayMember = "AD";
            cmbUrun.ValueMember = "URUN_ID";

            cmbUrun.DataSource = dt;
            conn.Close();
        }

        void malzemeleriGetir()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM TBL_MALZEMELER", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cmbMalzeme.DisplayMember = "AD";
            cmbMalzeme.ValueMember = "ID";

            cmbMalzeme.DataSource = dt;
            conn.Close();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            urunleriGetir();
            malzemeleriGetir();
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void btnMalzemeEkle_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Insert into TBL_MALZEMELER(AD,STOK,FIYAT,NOTLAR)" +
                " VALUES (@P1,@P2,@P3,@P4)", conn);

            cmd.Parameters.AddWithValue("@P1", txtMalzemeAd.Text);
            cmd.Parameters.AddWithValue("@P2", txtMalzemeStok.Text);
            cmd.Parameters.AddWithValue("@P3", txtMalzemeFiyat.Text);
            cmd.Parameters.AddWithValue("@P4", txtNotlar.Text);

            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Malzeme Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnMalzemeGuncelle_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE TBL_MALZEMELER SET AD=@P1,STOK=@P2,FIYAT=@P3,NOTLAR=@P4 " +
                "WHERE ID=@P5", conn);

            cmd.Parameters.AddWithValue("@P1", txtMalzemeAd.Text);
            cmd.Parameters.AddWithValue("@P2", decimal.Parse(txtMalzemeStok.Text));
            cmd.Parameters.AddWithValue("@P3", decimal.Parse(txtMalzemeFiyat.Text));
            cmd.Parameters.AddWithValue("@P4", txtNotlar.Text);
            cmd.Parameters.AddWithValue("@P5", txtMalzemeId.Text);
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Malzeme Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO TBL_URUNLER(AD,MFIYAT,SFIYAT,STOK) " +
                "VALUES(@P1,@P2,@P3,@P4)", conn);

            cmd.Parameters.AddWithValue("@P1", txtUrunAd.Text);
            cmd.Parameters.AddWithValue("@P2", txtUrunMaliyet.Text);
            cmd.Parameters.AddWithValue("@P3", txtUrunSatis.Text);
            cmd.Parameters.AddWithValue("@P4", txtUrunStok.Text);
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Ürün Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO TBL_FIRIN(URUN_ID,MALZEME_ID,MIKTAR,MALIYET) " +
                "VALUES(@P1,@P2,@P3,@P4)", conn);

            cmd.Parameters.AddWithValue("@P1", cmbUrun.SelectedValue);
            cmd.Parameters.AddWithValue("@P2", cmbMalzeme.SelectedValue);
            cmd.Parameters.AddWithValue("@P3", decimal.Parse(txtMiktar.Text));
            cmd.Parameters.AddWithValue("@P4", decimal.Parse(txtSatisFiyat.Text));
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Fırın Maliyeti Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            richTextBox1.Text += cmbMalzeme.Text + " : " + txtMiktar.Text + " KG " + txtSatisFiyat.Text + "Lira \n";

        }

        private void txtMiktar_TextChanged(object sender, EventArgs e)
        {
            int urunId = int.Parse(cmbUrun.SelectedValue.ToString());
            decimal fiyat = 0;
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select SFIYAT from TBL_URUNLER where URUN_ID=@P1", conn);
            cmd.Parameters.AddWithValue("@P1", urunId);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                fiyat = decimal.Parse(dr[0].ToString());
            }
            conn.Close();

            txtSatisFiyat.Text = (Convert.ToDecimal(txtMiktar.Text) * fiyat).ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            if (!durum)
            {
                txtMalzemeId.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
                txtMalzemeAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
                txtMalzemeStok.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
                txtMalzemeFiyat.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
                txtNotlar.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            }
            else
            {
                txtUrunId.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
                txtUrunAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
                txtUrunMaliyet.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
                txtUrunSatis.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
                txtUrunStok.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            }
        }


        private void btnUrunList_Click(object sender, EventArgs e)
        {
            durum = true;
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM TBL_URUNLER", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void btnMalzemeList_Click(object sender, EventArgs e)
        {
            durum = false;
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM TBL_MALZEMELER", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void btnUrunGuncelle_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE TBL_URUNLER SET AD=@P1,MFIYAT=@P2,SFIYAT=@P3,STOK=@P4 " +
                "WHERE URUN_ID=@P5", conn);
            cmd.Parameters.AddWithValue("@P1", txtUrunAd.Text);
            cmd.Parameters.AddWithValue("@P2", txtUrunMaliyet.Text);
            cmd.Parameters.AddWithValue("@P3", txtUrunSatis.Text);
            cmd.Parameters.AddWithValue("@P4", txtUrunStok.Text);
            cmd.Parameters.AddWithValue("@P5", txtUrunId.Text);
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Ürün Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
