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

namespace StockManagement
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

        public void Load_Data()
        {
            try
            {
                SqlConnection con = new SqlConnection("Data Source=DESKTOP-2B984GN;Initial Catalog=Stock;Integrated Security=True");
                SqlDataAdapter sda = new SqlDataAdapter("Select * From [Stock].[dbo].[Products]", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgProduct.Rows.Clear();
                foreach (DataRow items in dt.Rows)
                {
                    int n = dgProduct.Rows.Add();
                    dgProduct.Rows[n].Cells[0].Value = items["ProductCode"].ToString();
                    dgProduct.Rows[n].Cells[1].Value = items["ProductName"].ToString();

                    if ((bool)items["ProductStatus"])
                    {
                        dgProduct.Rows[n].Cells[2].Value = "Deactive";
                    }
                    else
                    {
                        dgProduct.Rows[n].Cells[2].Value = "Active";
                    }


                }
            }
            catch
            {

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult d_result = MessageBox.Show("Are you sure ?", " Commit", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (d_result == DialogResult.OK)
                {
                    SqlConnection con = new SqlConnection("Data Source=DESKTOP-2B984GN;Initial Catalog=Stock;Integrated Security=True");

                    con.Open();

                    //bool status = true;
                    //if (comboxStatus.SelectedIndex == 0)
                    //{
                    //    status = true;
                    //}
                    //else
                    //{
                    //    status = false;
                    //}

                    string sql_add = @"INSERT INTO [Stock].[dbo].[Products] 
			            ([ProductCode] 
			            ,[ProductName]
			            ,[ProductStatus]) 
	                VALUES
			        ('" + txtpCode.Text + "', '" + txtpName.Text + "', '" + comboxStatus.SelectedIndex.ToString() + "')";


                    SqlCommand cmd = new SqlCommand(sql_add, con);
                    cmd.ExecuteNonQuery();

                    con.Close();
                    Load_Data();
                    Clear();

                    labelStatus.Text = "1 row inserted.";
                }
                else
                {
                    labelStatus.Text = "No row insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                labelStatus.Text = "No row insert";
                
            }
        }

        private void Products_Load(object sender, EventArgs e)
        {
            comboxStatus.SelectedIndex = 0;
            labelStatus.Text = "";
            Load_Data();
        }

        private void dgProduct_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            txtpCode.Text = dgProduct.SelectedRows[0].Cells[0].Value.ToString();
            txtpName.Text = dgProduct.SelectedRows[0].Cells[1].Value.ToString();

            if (dgProduct.SelectedRows[0].Cells[2].Value.ToString() == "Active")
            {
                comboxStatus.SelectedIndex = 0;
            }
            else
            {
                comboxStatus.SelectedIndex = 1;
            }

            labelStatus.Text = "Row selected";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult d_result = MessageBox.Show("Are you sure ?", "Delete selected row", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (d_result == DialogResult.OK)
                {
                    SqlConnection con = new SqlConnection("Data Source=DESKTOP-2B984GN;Initial Catalog=Stock;Integrated Security=True");

                    con.Open();

                    string sql_delete = @"Delete From [Stock].[dbo].[Products] 
                            Where ProductCode='" + txtpCode.Text + "'";

                    SqlCommand cmd = new SqlCommand(sql_delete, con);
                    cmd.ExecuteNonQuery();

                    con.Close();
                    Load_Data();
                    labelStatus.Text = "Deleted 1 row.";
                    Clear();
                }
                else
                {
                    labelStatus.Text = "No row delete";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                labelStatus.Text = "No row delete.";
            }
        }

        private void Clear()
        {
            txtpCode.Clear();
            txtpName.Clear();
            comboxStatus.SelectedIndex = 0;
           
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult d_result = MessageBox.Show("Are you sure ?", "Update selected row", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (d_result == DialogResult.OK)
                {
                    SqlConnection con = new SqlConnection("Data Source=DESKTOP-2B984GN;Initial Catalog=Stock;Integrated Security=True");

                    con.Open();

                    string sql_update = @"update [Stock].[dbo].[Products] set
                            ProductName = '" + txtpName.Text + "'," +
                            "ProductStatus = '" + comboxStatus.SelectedIndex.ToString() + "'" +
                            "WHERE ProductCode = '" + txtpCode.Text + "'";


                    SqlCommand cmd = new SqlCommand(sql_update, con);
                    cmd.ExecuteNonQuery();

                    con.Close();
                    Load_Data();
                    labelStatus.Text = "Updated 1 row.";
                    Clear();
                }
                else
                {
                    labelStatus.Text = "No row update";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                labelStatus.Text = "No row Update.";
            }
        }
    }
}
