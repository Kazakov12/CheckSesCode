using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Курсовая_Работа
{
    public partial class MainForm : Form
    {
        SqlConnection cn;
        Class_SELECT_To_SQL csts = new Class_SELECT_To_SQL();
        Class_INSERT_To_SQL cist = new Class_INSERT_To_SQL();
        Class_DELETE_To_SQL cdst = new Class_DELETE_To_SQL();
        public MainForm()
        {
            InitializeComponent();
            режимToolStripMenuItem.Text = "Внимание! Для продолжения работы требуется подключение к Базе данных";
            start_app();
        }
        private void авторизоватьсяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Authorization avto = new Authorization();
                avto.ShowDialog(this);
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
        }
        internal void connection_to_SQL(string login, string password)
        {

        }
        internal void connection_to_SQL()
        {
            if (режимToolStripMenuItem.Text != "Внимание! Для продолжения работы требуется подключение к Базе данных")
            {
                //if (cn.State == ConnectionState.Open)
                //{
                    //cn.Close();
                  //  form_clear();
                //}
                cn.Close();
                form_clear();
            }

            string login = "admin";
            string password = "qwerty!123";
            string connectionString = $"Server=tcp:server-azure-db.database.windows.net,1433;Initial Catalog=БД Безопасность Предприятия;Persist Security Info=False;User ID={login};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            cn = new SqlConnection(connectionString);
            cn.Open();
            Status_Check();
            csts.Return_Login_User_FIO(login, режимToolStripMenuItem, textBoxFIO, textBoxFIoSotr,textBoxIDSotr, cn);
            csts.View_all_information(textBoxIDSotr, textBoxFIoSotr,
                textBoxDateOfBirth, textBoxYearOld, textBoxOtdel, textBoxPhoneNumber,
                textBoxDataWork, textBoxPassport, textBoxAllCountGiven, listBoxGivenKey,
                dataGridViewActiveKey, cn);
            button6.Enabled = true;

        }
        internal void start_app()
        {
            tabControlMenu.TabPages.Remove(tabPageEditAdmin);
            tabControlMenu.TabPages.Remove(tabPageRedistKeyAdmin);
            tabControlMenu.TabPages.Remove(tabPageEditAdmin);
            tabControlMenu.TabPages.Remove(tabPageRegistKey);
            tabControlMenu.TabPages.Remove(tabPageInfoKey);
            tabControlMenu.TabPages.Remove(tabPageSpisanieSrok);
            tabControlMenu.TabPages.Remove(tabPageZapros);
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
        }

        internal void form_clear()
        {
            comboBoxKeySotr.Text = string.Empty;
            comboBoxKeySotr.Items.Clear();
            comboBoxZSotr.Text = string.Empty;
            comboBoxKeySotr.Visible = false;
            label19.Visible = false;
            button2.Enabled = false;
            button7.Enabled = false;
            comboBox6.Text = string.Empty;
        }
        internal void clear_datagrid_select_table()
        {
        }
        internal void report_2_3()
        {
            csts.Request_for_the_number_of_deliveries(chartCountSupplyKey,textBox2, cn);
            csts.Request_for_the_sum_of_deliveries(chartSumSupplyKey, textBox1, cn);
            csts.Request_for_the_count_of_given(chartCountGivenKey, textBox3, cn);
            csts.Request_for_the_count(dataGridView3, textBox4, cn);
        }
        internal void Status_Check()
        {
            bool role_flag = false;
            if (csts.select_role_owner(cn) == 1)
            {
                режимToolStripMenuItem.Text = "Подключен: Администратор - ";
                if (tabControlMenu.TabPages.Count == 2)
                {
                    tabControlMenu.TabPages.Add(tabPageEditAdmin);
                    tabControlMenu.TabPages.Add(tabPageRedistKeyAdmin);
                    tabControlMenu.TabPages.Add(tabPageInfoKey);
                    tabControlMenu.TabPages.Add(tabPageSpisanieSrok);
                    tabControlMenu.TabPages.Add(tabPageZapros);
                }
                if (tabControlMenu.TabPages.Count == 1)
                {
                    tabControlMenu.TabPages.Add(tabPageRegistKey);
                    tabControlMenu.TabPages.Add(tabPageEditAdmin);
                    tabControlMenu.TabPages.Add(tabPageRedistKeyAdmin);
                    tabControlMenu.TabPages.Add(tabPageInfoKey);
                    tabControlMenu.TabPages.Add(tabPageSpisanieSrok);
                    tabControlMenu.TabPages.Add(tabPageZapros);

                }
                report_2_3();
                update_info();
                csts.debiting_keys_dataGridView(dataGridView4, cn);
                csts.debiting_keys_ComboBox(comboBox3, cn);
                csts.Completion_comboBoxListNameTable(comboBox6, cn);
                dataGridView6.Columns.Clear();
                role_flag = true;
            }
            if (!role_flag)
            {
                if (csts.select_role_def_role(cn) == 1)
                {
                    режимToolStripMenuItem.Text = "Подключен: Пользователь - ";
                    if (tabControlMenu.TabPages.Count == 7)
                    {
                        tabControlMenu.TabPages.Remove(tabPageEditAdmin);
                        tabControlMenu.TabPages.Remove(tabPageRedistKeyAdmin);
                        tabControlMenu.TabPages.Remove(tabPageInfoKey);
                        tabControlMenu.TabPages.Remove(tabPageSpisanieSrok);
                        tabControlMenu.TabPages.Remove(tabPageZapros);
                    }
                    if (tabControlMenu.TabPages.Count == 1)
                    {
                        tabControlMenu.TabPages.Add(tabPageRegistKey);
                    }
                }
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {              
            bool flag = false;
            if (режимToolStripMenuItem.Text != "Внимание! Для продолжения работы требуется подключение к Базе данных")
            {
                //if (cn.State == ConnectionState.Open)
                //{
                //    cn.Close();
                //}
                if (cn.State == ConnectionState.Open)
                {
                    flag = true;
                }
                if (!flag)
                {
                    cn.Close();
                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            processing_of_applications();
        }
        internal void processing_of_applications()
        {
            if (comboBox1.Text == "Списание")
            {
                csts.Completion_ComboBox_Pr_SP(comboBox4, cn);
                label5.Visible = true;
                comboBox4.Visible = true;
                csts.DataGridView_Completion_ADMIN(dataGridView1, dataGridView2, "Заявление на списание ЭК", "Списание ключей", cn);
                label3.Visible = false;
                comboBox2.Visible = false;
                comboBox4.Text = string.Empty;
                comboBox2.Text = string.Empty;
                button1.Enabled = false;
            }
            if (comboBox1.Text == "Выдача")
            {
                label3.Visible = true;
                comboBox2.Visible = true;
                label5.Visible = false;
                comboBox4.Visible = false;
                comboBox2.Text = string.Empty;
                csts.debiting_keys_ComboBox2(comboBox2, cn);
                csts.DataGridView_Completion_ADMIN(dataGridView1, dataGridView2, "Заявление на регистрацию ЭК", "Выдача ЭК", cn);
                button1.Enabled = false;
                
            }
            if (comboBox1.Text == "Ремонт")
            {
                label5.Visible = false;
                comboBox4.Visible = false;
                csts.DataGridView_Completion_ADMIN(dataGridView1, dataGridView2, "Заявление на ремонт ЭК", "Ремонт ЭК", cn);
                label3.Visible = false;
                comboBox2.Visible = false;
                button1.Enabled = true;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string[] num = comboBoxKeySotr.Text.Split('-');
            if (comboBoxZSotr.Text == "Заявление на выдачу ЭК")
            {
                cist.insert_table_regist(cn, csts.search_fio(textBoxFIoSotr.Text, cn),date());
                form_clear();
            }
            else if(comboBoxZSotr.Text == "Заявление на списание ЭК")
            {
                cist.insert_table_cancellation(cn, csts.search_number_given(textBoxFIoSotr.Text, Convert.ToInt32(num[0]), cn), date());
                form_clear();
            }
            else if(comboBoxZSotr.Text == "Заявление на ремонт ЭК")
            {
                cist.insert_table_repair(cn, csts.search_number_given(textBoxFIoSotr.Text, Convert.ToInt32(num[0]), cn), date());
                form_clear();
            }
        }

        internal string date()
        {
            string[] s = DateTime.Now.ToShortDateString().Split('.');
            //string st = ($"{s[2]}.{s[1]}.{s[0]}");
            string st = ($"{s[3]}.{s[2]}.{s[1]}.{s[0]}");
            return st;
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxZSotr.Text == "Заявление на ремонт ЭК" || comboBoxZSotr.Text == "Заявление на списание ЭК" )
            {
                csts.Completion_ComboBox_Cotr_key(comboBoxKeySotr, textBoxFIoSotr.Text, cn);
                label19.Visible = true;
                comboBoxKeySotr.Visible = true;
                comboBoxKeySotr.Text = string.Empty;
                button2.Enabled = false;
            }
            else  if (comboBoxZSotr.Text == "Заявление на выдачу ЭК")
            {
                label19.Visible = false;
                comboBoxKeySotr.Visible = false;
                button2.Enabled = true;
                comboBoxKeySotr.Items.Clear();
                comboBoxKeySotr.Text = string.Empty;
            }
        }
        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxZSotr.Text = string.Empty;
            comboBoxKeySotr.Items.Clear();
            comboBoxKeySotr.Text = string.Empty;
            label4.Visible = true;
            comboBoxZSotr.Visible = true;
        }
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox4.Text == "Высвобождение сотрудника с предприятия ")
            {
                comboBox2.Text = string.Empty;
                comboBox2.Items.Clear();
                comboBox2.Visible = false;
                label3.Visible = false;
                button1.Enabled = true;
            }
            else
            {
                comboBox2.Visible = true;
                comboBox2.Text = string.Empty;
                label3.Visible = true;
                csts.debiting_keys_ComboBox2(comboBox2, cn);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows[dataGridView1.CurrentRow.Index].DefaultCellStyle.BackColor != Color.GreenYellow)
            {
                string[] num = comboBox2.Text.Split('-');
                if (comboBox1.Text == "Списание")
                {
                    if (comboBox2.Visible == true)
                    {
                        int pr_spis = csts.search_id_pr_spis(comboBox4.Text,cn);
                        cist.insert_table_cancellation2(cn, Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value), date(), pr_spis);
                        int id_sotr = csts.number_sotr(Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value), cn);
                        cist.insert_table_regist(cn, id_sotr, date());
                        int id_z = csts.number_zayv(id_sotr, cn);
                        cist.insert_table_regist2(cn, id_z, Convert.ToInt32(num[0]), date());
                    }
                    else
                    {
                        int pr_spis = csts.search_id_pr_spis(comboBox4.Text, cn); 
                        cist.insert_table_cancellation2(cn, Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value), date(),pr_spis);
                    }
                }
                if (comboBox1.Text == "Выдача")
                {
                    cist.insert_table_regist2(cn, Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value), Convert.ToInt32(num[0]), date());
                }
                if (comboBox1.Text == "Ремонт")
                {
                    cist.insert_table_repair2(cn, Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value), date());
                }
                comboBox2.Text = string.Empty;
                comboBox4.Text = string.Empty;
                comboBox2.Items.Clear();
                comboBox2.Visible = false;
                comboBox2.Text = string.Empty;
                //comboBox2.Text = string.Empty;
                label3.Visible = false;
                processing_of_applications();
            }
            else
            {
                MessageBox.Show("Ошибка! Данное заявление уже обработано", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            update_info(); 
            csts.debiting_keys_dataGridView(dataGridView4, cn);
            csts.debiting_keys_ComboBox(comboBox3, cn);
        }
        internal void update_info()
        {
            label21.Text = "Информация обновлена на: " + DateTime.Now;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if(dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[9].Style.BackColor == Color.Red)
            {
                if (radioButton1.Checked == true)
                {
                    string[] data = comboBox3.Text.Split('-');
                    cist.insert_table_cancellation(cn, Convert.ToInt32(dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[0].Value), date()); 
                    int number = csts.number_zayv_na_spisanie(Convert.ToInt32(dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[0].Value), cn);
                    cist.insert_table_cancellation2(cn, number, date(),2);
                    int id_sotr = csts.number_sotr(Convert.ToInt32(dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[0].Value), cn);
                    cist.insert_table_regist(cn, id_sotr, date());
                    int id_z = csts.number_zayv(id_sotr,cn);
                    cist.insert_table_regist2(cn, id_z,Convert.ToInt32(data[0]), date());
                }
                if (radioButton2.Checked == true)
                {
                    cist.insert_table_cancellation(cn, Convert.ToInt32(dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[0].Value), date());
                    int number = csts.number_zayv_na_spisanie(Convert.ToInt32(dataGridView4.Rows[dataGridView4.CurrentRow.Index].Cells[0].Value), cn);
                    cist.insert_table_cancellation2(cn, number, date(),4);
                }
                update_info();
                csts.debiting_keys_dataGridView(dataGridView4, cn);
                csts.debiting_keys_ComboBox(comboBox3, cn);
                button3.Visible = false;
                label22.Visible = false;
                comboBox3.Visible = false;
                radioButton1.Checked = false;
                radioButton2.Checked = false;
            }
            else
            {
                MessageBox.Show("Ошибка! Данный ключ еще рано списывать!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void comboBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            button3.Visible = true;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button3.Visible = true;
            label22.Visible = false;
            comboBox3.Visible = false;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button3.Visible = false;
            label22.Visible = true;
            comboBox3.Visible = true;
            comboBox3.Text = string.Empty;
        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            report_2_3();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            processing_of_applications();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
        private void Blocking_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            csts.View_all_information(textBoxIDSotr, textBoxFIoSotr,
                    textBoxDateOfBirth, textBoxYearOld, textBoxOtdel, textBoxPhoneNumber,
                    textBoxDataWork, textBoxPassport, textBoxAllCountGiven, listBoxGivenKey,
                    dataGridViewActiveKey, cn);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            cdst.drop_value(cn, comboBox6.Text, Convert.ToInt32(dataGridView6.Rows[dataGridView6.CurrentRow.Index].Cells[0].Value),dataGridView6.Columns[0].HeaderText);
            csts.Completion_dataGrid(dataGridView6, cn, comboBox6.Text);
        }

        private void comboBox6_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            csts.Completion_dataGrid(dataGridView6, cn, comboBox6.Text);
            button7.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            csts.Completion_dataGrid(dataGridView6, cn, comboBox6.Text);
        }

        private void comboBox5_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBox5.Text == "Запрос 1. Перекрестный запрос")
            {
                csts.cross_request(dataGridView5, cn);
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = false;
                panel1.Size = new Size(1183, 508);
                panel1.Location = new Point(8, 72);
            }
            else if (comboBox5.Text == "Запрос 2. Запрос на выборку в определнном промежутке")
            {
                panel1.Visible = false;
                panel2.Visible = true;
                panel3.Visible = false;
                panel2.Size = new Size(1183, 508);
                panel2.Location = new Point(8, 72);

            }
            else if (comboBox5.Text == "Запрос 3. Запрос с использованием логической функции IIf")
            {
                panel1.Visible = false;
                panel2.Visible = false;
                panel3.Visible = true;
                panel3.Location = new Point(8, 72);
                csts.zapros_3(dataGridView8, cn);
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            csts.zapros_2(dataGridView7, Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown2.Value), Convert.ToInt32(numericUpDown3.Value), cn);
        }
    }
}
