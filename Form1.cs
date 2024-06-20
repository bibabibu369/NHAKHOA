using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Exam_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string ketnoi = @"Data Source=.\sqlexpress;Initial Catalog=NHAKHOA;Integrated Security=True";
        SqlDataAdapter daNK;
        SqlCommand cmd;
        SqlConnection conn;

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(ketnoi);
            conn.Open();

            string sql = "SELECT * FROM NHAKHOA";
            daNK = new SqlDataAdapter(sql, conn);
            DataTable dtNK = new DataTable();
            daNK.Fill(dtNK);
            dgv.DataSource = dtNK;
        }

        public void rf()
        {
            conn = new SqlConnection(ketnoi);
            conn.Open();

            string sql = "SELECT * FROM NHAKHOA";
            daNK = new SqlDataAdapter(sql, conn);
            DataTable dtNK = new DataTable();
            daNK.Fill(dtNK);
            dgv.DataSource = dtNK;
        }

        private void btnRs_Click(object sender, EventArgs e)
        {
            tbID.Clear();
            tbDc.Clear();
            tbTen.Clear();
            tbTim.Clear();
            rCV.Checked = false;
            rTay.Checked = false;
            rTram.Checked = false;
            tbID.Focus();
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                tbID.Text = row.Cells["Mã KH"].Value.ToString().Trim();
                tbTen.Text = row.Cells["Họ tên"].Value.ToString().Trim();
                dtp.Text = row.Cells["Ngày sinh"].Value.ToString().Trim();
                tbDc.Text = row.Cells["Địa chỉ"].Value.ToString().Trim();
                string dv = row.Cells["Dịch vụ sử dụng"].ToString().Trim();

                if (dv == "Cạo vôi") rCV.Checked = true;
                else if (dv == "Tẩy trắng") rTay.Checked = true;
                else if (dv == "Trám răng") rTram.Checked = true;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string id = tbID.Text.Trim();
            string ten = tbTen.Text.Trim();
            DateTime ns = dtp.Value;
            string dc = tbDc.Text.Trim();
            string dv = rCV.Checked ? "Cạo vôi" : (rTay.Checked ? "Tẩy trắng" : (rTram.Checked ? "Trám răng" : null));

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(ten) || string.IsNullOrEmpty(dc) || dv == null)
            {
                MessageBox.Show("Vui long nhap day du thong tin", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using(conn = new SqlConnection(ketnoi))
            {
                conn.Open();
                string add = "INSERT INTO NHAKHOA ([Mã KH], [Họ tên], [Ngày sinh], [Địa chỉ], [Dịch vụ sử dụng]) VALUES(@id, @ten, @ns, @dc, @dv)";

                using (cmd = new SqlCommand(add, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@ten", ten);
                    cmd.Parameters.AddWithValue("@ns", ns);
                    cmd.Parameters.AddWithValue("@dc", dc);
                    cmd.Parameters.AddWithValue("@dv", dv);

                    if(cmd.ExecuteNonQuery() > 0)
                    {
                        rf();
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string id = tbID.Text.Trim();
            string ten = tbTen.Text.Trim();
            DateTime ns = dtp.Value;
            string dc = tbDc.Text.Trim();
            string dv = rCV.Checked ? "Cạo vôi" : (rTay.Checked ? "Tẩy trắng" : (rTram.Checked ? "Trám răng" : null));

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(ten) || string.IsNullOrEmpty(dc) || dv == null)
            {
                MessageBox.Show("Vui long nhap day du thong tin", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (conn = new SqlConnection(ketnoi))
            {
                conn.Open();
                string add = "UPDATE NHAKHOA SET [Mã KH] = @id, [Họ tên] = @ten, [Ngày sinh] = @ns, [Địa chỉ] = @dc, [Dịch vụ sử dụng] = @dv WHERE [Mã KH] = @id";

                using (cmd = new SqlCommand(add, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@ten", ten);
                    cmd.Parameters.AddWithValue("@ns", ns);
                    cmd.Parameters.AddWithValue("@dc", dc);
                    cmd.Parameters.AddWithValue("@dv", dv);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        rf();
                    }
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string id = tbID.Text.Trim();

            if (string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Vui long nhap day du thong tin", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (conn = new SqlConnection(ketnoi))
            {
                conn.Open();
                string add = "DELETE FROM NHAKHOA WHERE [Mã KH] = @id";

                using (cmd = new SqlCommand(add, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        rf();
                        tbID.Clear();
                        tbDc.Clear();
                        tbTen.Clear();
                        tbTim.Clear();
                        rCV.Checked = false;
                        rTay.Checked = false;
                        rTram.Checked = false;
                        tbID.Focus();
                    }
                }
            }
        }

        private void tbTim_TextChanged(object sender, EventArgs e)
        {
            string search = tbTim.Text.Trim();
            string sql = "SELECT * FROM NHAKHOA WHERE [Mã KH] LIKE @search OR [Họ tên] LIKE @search";

            using (conn = new SqlConnection(ketnoi))
            {
                conn.Open();
                
                using (cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                    DataTable dtNK = new DataTable();
                    daNK = new SqlDataAdapter(cmd); 
                    daNK.Fill(dtNK);
                    dgv.DataSource = dtNK;
                }
            }
        }
    }
}
