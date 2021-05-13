using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data; //Збірка потрібна для підключення до БД
using System.Data.SqlClient; //Потрібна для підключення до БД

namespace magazin
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;
        private bool newRowAdding = false;

        public Form1()
        {
            InitializeComponent();
        }



        private void LoadData() 
        {
            try 
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM Products", sqlConnection);

                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Products");

                dataGridView1.DataSource = dataSet.Tables["Products"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[8, i] = linkCell;
                }

                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable; //забороняємо сортування по колонці
                }
            }
            catch(Exception ex) 
            { 
              MessageBox.Show(ex.Message, "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private void ReloadData()
        {
            try
            {
                dataSet.Tables["Products"].Clear();

                sqlDataAdapter.Fill(dataSet, "Products");

                dataGridView1.DataSource = dataSet.Tables["Products"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[8, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dima\Desktop\magazin\magazin\magazin\Database.mdf;Integrated Security=True");
            // @ - Переводить в Юнікод те що в лапках
            // підключаемось до бази даних
            sqlConnection.Open();

            LoadData(); //визиваємо метод заповнення таблиці
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 8)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Видалити обрану строку?", "Видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;

                            dataGridView1.Rows.RemoveAt(rowIndex);//видаляємо дані с таблиці в DataGrid

                            dataSet.Tables["Products"].Rows[rowIndex].Delete();//видаляємо дані с таблиці в БД

                            sqlDataAdapter.Update(dataSet, "Products");//оновлюємо сторінку
                        }
                    }

                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;

                        DataRow row = dataSet.Tables["Products"].NewRow();

                        row["Destiny"] = dataGridView1.Rows[rowIndex].Cells["Destiny"].Value;
                        row["Sub_destiny"] = dataGridView1.Rows[rowIndex].Cells["Sub_destiny"].Value;
                        row["Name"] = dataGridView1.Rows[rowIndex].Cells["Name"].Value;
                        row["Producer"] = dataGridView1.Rows[rowIndex].Cells["Producer"].Value;
                        row["Qantity"] = dataGridView1.Rows[rowIndex].Cells["Qantity"].Value;
                        row["Price"] = dataGridView1.Rows[rowIndex].Cells["Price"].Value;
                        row["Supplier"] = dataGridView1.Rows[rowIndex].Cells["Supplier"].Value;

                        dataSet.Tables["Products"].Rows.Add(row);

                        dataSet.Tables["Products"].Rows.RemoveAt(dataSet.Tables["Products"].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        dataGridView1.Rows[e.RowIndex].Cells[8].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, "Products");

                        newRowAdding = false;
                    }

                    else if (task == "Update")
                    {
                        int r = e.RowIndex;

                        dataSet.Tables["Products"].Rows[r]["Destiny"] = dataGridView1.Rows[r].Cells["Destiny"].Value;
                        dataSet.Tables["Products"].Rows[r]["Sub_destiny"] = dataGridView1.Rows[r].Cells["Sub_destiny"].Value;
                        dataSet.Tables["Products"].Rows[r]["Name"] = dataGridView1.Rows[r].Cells["Name"].Value;
                        dataSet.Tables["Products"].Rows[r]["Producer"] = dataGridView1.Rows[r].Cells["Producer"].Value;
                        dataSet.Tables["Products"].Rows[r]["Qantity"] = dataGridView1.Rows[r].Cells["Qantity"].Value;
                        dataSet.Tables["Products"].Rows[r]["Price"] = dataGridView1.Rows[r].Cells["Price"].Value;
                        dataSet.Tables["Products"].Rows[r]["Supplier"] = dataGridView1.Rows[r].Cells["Supplier"].Value;

                        sqlDataAdapter.Update(dataSet, "Products");

                        dataGridView1.Rows[e.RowIndex].Cells[8].Value = "Delete";//після того як новий товар буде доданий, ячейка "Апдейт" зміниться на "Видалити"
                    }

                    ReloadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;

                    int lastRow = dataGridView1.Rows.Count - 2;

                    DataGridViewRow row = dataGridView1.Rows[lastRow];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[8, lastRow] = linkCell;

                    row.Cells["Command"].Value = "Insert"; //замінює видалення на встановлення
                }
            }
            catch(Exception ex) 
            { MessageBox.Show(ex.Message, "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try 
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;

                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[8, rowIndex] = linkCell;

                    editingRow.Cells["Command"].Value = "Update"; //замінює видалення на редагування
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Помилка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);
            //валідуємо ячейки 5 та 6
            if (dataGridView1.CurrentCell.ColumnIndex == 5)
            {
                TextBox textBox = e.Control as TextBox;

                if (textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }
            }

            if (dataGridView1.CurrentCell.ColumnIndex == 6)
            {
                TextBox textBox = e.Control as TextBox;

                if (textBox != null)
                {
                    textBox.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }
            }
        }

        private void Column_KeyPress(object sender, KeyPressEventArgs e)//обробка для того щоб можна було вводити лише числа
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            { e.Handled = true; }
        }

        private void вийтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
