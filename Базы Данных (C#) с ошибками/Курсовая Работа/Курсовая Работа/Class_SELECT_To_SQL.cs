using System;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Курсовая_Работа
{
    internal class Class_SELECT_To_SQL
    {
        // Запросы на проверку роли подключенного пользователя к БД 
        internal int select_role_owner(SqlConnection cn)
        {
            using (SqlCommand thisCommand = cn.CreateCommand())
            {
                thisCommand.CommandText = "SELECT IS_MEMBER ('db_owner')";
                return Convert.ToInt32(thisCommand.ExecuteScalar());
            }
        }
        internal int select_role_def_role(SqlConnection cn)
        {
            using (SqlCommand thisCommand = cn.CreateCommand())
            {
                thisCommand.CommandText = "SELECT IS_MEMBER ('RoleDefaultUserDB')";
                return Convert.ToInt32(thisCommand.ExecuteScalar());
            }
        }
        // Запрос на вывод всей информации о сотруднике подключенном к сеансу
        internal void View_all_information(TextBox textBoxIDSotr, TextBox textBoxFIO, TextBox textBoxDateOfBirth, TextBox textBoxYearOld, 
            TextBox textBoxOtdel, TextBox textBoxPhoneNumber, TextBox textBoxDataWork, TextBox textBoxPassport, TextBox textBoxAllCountGiven,
            ListBox listBoxGivenKey, DataGridView dataGridViewActiveKey, SqlConnection cn)
        {
            List<string[]> data = new List<string[]>();
            List<string[]> data2 = new List<string[]>();
            try
            {
                SqlCommand thisCommand = cn.CreateCommand();
                    thisCommand.CommandText = $"SELECT [Сотрудники предприятия].[Код сотрудника], [Сотрудники предприятия].[Фамилия]+' '+[Сотрудники предприятия].[Имя]+' '+[Сотрудники предприятия].[Отчество] AS ФИО, [Сотрудники предприятия].[Дата рождения], Отделы.[Наименование отдела], [Сотрудники предприятия].[Номер телефона], [Сотрудники предприятия].[Дата приёма на работу], [Сотрудники предприятия].[Паспортные данные], DATEDIFF(year,[Сотрудники предприятия].[Дата рождения], GETDATE()) AS Возраст FROM Отделы INNER JOIN[Сотрудники предприятия] ON Отделы.[Номер отдела] = [Сотрудники предприятия].Отдел WHERE((([Сотрудники предприятия].[Код сотрудника]) = {Convert.ToInt32(textBoxIDSotr.Text)}) AND(([Сотрудники предприятия].[Фамилия] + ' ' +[Сотрудники предприятия].[Имя] + ' ' +[Сотрудники предприятия].[Отчество]) = N'{textBoxFIO.Text}')); ";
                using (SqlDataReader thisReader = thisCommand.ExecuteReader())
                {
                    while (thisReader.Read())
                    {
                        textBoxDateOfBirth.Text = Convert.ToDateTime(thisReader[2]).ToShortDateString();
                        textBoxYearOld.Text = thisReader[7].ToString();
                        textBoxOtdel.Text = thisReader[3].ToString();
                        textBoxPhoneNumber.Text = thisReader[4].ToString();
                        textBoxDataWork.Text = Convert.ToDateTime(thisReader[5]).ToShortDateString();
                        textBoxPassport.Text = thisReader[6].ToString();
                    }
                    
                    thisReader.Close();
                }
                int count = 0;
                listBoxGivenKey.Items.Clear();
                thisCommand.CommandText = $"SELECT [Сотрудники предприятия].[Код сотрудника], [Идентификационные номера ЭК].[Идентификационный номер], [Носители ЭК].Наименование, [Выдача ЭК].[Дата выдачи], DateAdd(MONTH,[Срок службы (мес)],[Дата изготовления]) AS Выражение1, DateDiff(day,GETDATE(),DateAdd(month,[Срок службы (мес)],[Дата изготовления])) AS [Кол-во дней до оконания] FROM([Носители ЭК] INNER JOIN[Поставка ключей] ON[Носители ЭК].[Код носителя] = [Поставка ключей].Ключ) INNER JOIN([Идентификационные номера ЭК] INNER JOIN (([Сотрудники предприятия] INNER JOIN [Заявление на регистрацию ЭК] ON [Сотрудники предприятия].[Код сотрудника] = [Заявление на регистрацию ЭК].Сотрудник) INNER JOIN[Выдача ЭК] ON[Заявление на регистрацию ЭК].[Код заявления] = [Выдача ЭК].[Регистрационное заявление]) ON[Идентификационные номера ЭК].[Идентификационный номер] = [Выдача ЭК].[Выданный ключ]) ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя] WHERE((([Сотрудники предприятия].[Код сотрудника]) = {Convert.ToInt32(textBoxIDSotr.Text)})); ";
                using (SqlDataReader thisReader2 = thisCommand.ExecuteReader())
                {
                    while (thisReader2.Read())
                    {
                        count++;
                        data.Add(new string[4]);
                        data[data.Count - 1][0] = thisReader2[1].ToString();
                        data[data.Count - 1][1] = thisReader2[2].ToString();
                        data[data.Count - 1][2] = Convert.ToDateTime(thisReader2[3]).ToShortDateString();
                        data[data.Count - 1][3] = thisReader2[5].ToString();

                        listBoxGivenKey.Items.Add(data[data.Count - 1][0] + " - " + data[data.Count - 1][1] + " - Дата выдачи: " + data[data.Count - 1][2]);
                    }
                    if (count == 0)
                    {
                        listBoxGivenKey.Items.Add("Выданных ключей нет.");
                    }
                    thisReader2.Close();
                }
                    textBoxAllCountGiven.Text = count.ToString();
                    thisCommand.CommandText = $"SELECT [Сотрудники предприятия].[Код сотрудника], [Идентификационные номера ЭК].[Идентификационный номер], [Списание ключей].[Дата списания] FROM([Носители ЭК] INNER JOIN[Поставка ключей] ON[Носители ЭК].[Код носителя] = [Поставка ключей].Ключ) INNER JOIN([Идентификационные номера ЭК] INNER JOIN (([Сотрудники предприятия] INNER JOIN [Заявление на регистрацию ЭК] ON [Сотрудники предприятия].[Код сотрудника] = [Заявление на регистрацию ЭК].Сотрудник) INNER JOIN([Выдача ЭК] INNER JOIN ([Заявление на списание ЭК] INNER JOIN [Списание ключей] ON[Заявление на списание ЭК].[Код заявления] = [Списание ключей].[Код заявления]) ON[Выдача ЭК].[Код выдачи] = [Заявление на списание ЭК].[Код выдачи]) ON[Заявление на регистрацию ЭК].[Код заявления] = [Выдача ЭК].[Регистрационное заявление]) ON[Идентификационные номера ЭК].[Идентификационный номер] = [Выдача ЭК].[Выданный ключ]) ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя] WHERE((([Сотрудники предприятия].[Код сотрудника]) = {Convert.ToInt32(textBoxIDSotr.Text)})); ";
                using (SqlDataReader thisReader3 = thisCommand.ExecuteReader())
                {
                    while (thisReader3.Read())
                    {
                        data2.Add(new string[1]);
                        data2[data2.Count - 1][0] = thisReader3[1].ToString();
                    }
                    thisReader3.Close();
                }
                dataGridViewActiveKey.Columns.Clear();
                dataGridViewActiveKey.ColumnCount = 4;
                dataGridViewActiveKey.Columns[0].HeaderText = "ID выданного ключа";
                dataGridViewActiveKey.Columns[1].HeaderText = "Наименование";
                dataGridViewActiveKey.Columns[2].HeaderText = "Дата выдачи";
                dataGridViewActiveKey.Columns[3].HeaderText = "Количество дней до окончания";
                bool flag = false;
                foreach (string[] s in data)
                {
                    for (int i = 0; i < data2.Count; i++)
                    {
                        if (Convert.ToInt32(s[0]) == Convert.ToInt32(data2[i][0]))
                        {
                            flag = true;
                        }
                    }
                    if (flag == false)
                    {
                        dataGridViewActiveKey.Rows.Add(s);
                    }
                    flag = false;
                }
                for (int i = 0; i < dataGridViewActiveKey.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dataGridViewActiveKey.Rows[i].Cells[3].Value) >= 100)
                    {
                        dataGridViewActiveKey.Rows[i].Cells[3].Style.BackColor = Color.GreenYellow;
                    }
                    else if (Convert.ToInt32(dataGridViewActiveKey.Rows[i].Cells[3].Value) < 100 & Convert.ToInt32(dataGridViewActiveKey.Rows[i].Cells[3].Value) > 5)
                    {
                        dataGridViewActiveKey.Rows[i].Cells[3].Style.BackColor = Color.Orange;
                    }
                    else
                    {
                        dataGridViewActiveKey.Rows[i].Cells[3].Style.BackColor = Color.Red;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
            finally
            {
                data.Clear();
                data2.Clear();
            }
        }
        // Запрос на вывод всех названий таблиц 
        internal void Completion_comboBoxListNameTable(ComboBox comboBoxListNameTable, SqlConnection cn)
        {
            try
            {
                comboBoxListNameTable.Items.Clear();
                SqlCommand thisCommand = cn.CreateCommand();
                thisCommand.CommandText = "SELECT Name FROM dbo.sysobjects WHERE (xtype = 'U')";
                using (SqlDataReader thisReader = thisCommand.ExecuteReader())
                {
                    while (thisReader.Read())
                    {
                        comboBoxListNameTable.Items.Add(thisReader["Name"]);
                    }
                    thisReader.Close();
                }
                comboBoxListNameTable.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
        }
        // Заполнение dataGridView по названию таблицы для просмотра БД
        internal void Completion_dataGrid(DataGridView dataGridViewSelectTableView, SqlConnection cn, string name_select_table)
        {
            try
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter($"select * from [dbo].[{name_select_table}]", cn))
                {
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    dataGridViewSelectTableView.DataSource = dataSet.Tables[0];
                }    
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // Запрос на заполнение comboBox "Причина списания"
        internal void Completion_ComboBox_Pr_SP(ComboBox comboBox4, SqlConnection cn)
        {
            try
            {
                comboBox4.Items.Clear();
                SqlCommand thisCommand = cn.CreateCommand();
                thisCommand.CommandText = "SELECT [Причины списания].Наименование FROM [Причины списания];";
                using (SqlDataReader thisReader = thisCommand.ExecuteReader())
                {
                    while (thisReader.Read())
                    {
                        comboBox4.Items.Add(thisReader["Наименование"]);
                    }
                    //thisReader.Close();
                    comboBox4.Enabled = true;
                } 
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }

        }
        // Запрос на заполнение 2-х таблиц для оформления заявления
        internal void DataGridView_Completion_ADMIN(DataGridView dataGridView1, DataGridView dataGridView2, string name_table, string name_table1, SqlConnection cn)
        {
            try
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter($"select * from [dbo].[{name_table}]", cn))
                {
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    dataGridView1.DataSource = dataSet.Tables[0];

                    SqlDataAdapter dataAdapter1 = new SqlDataAdapter($"select * from [dbo].[{name_table1}]", cn);
                    DataSet dataSet1 = new DataSet();
                    dataAdapter1.Fill(dataSet1);
                    dataGridView2.DataSource = dataSet1.Tables[0];

                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        for (int j = 0; j < dataGridView2.RowCount; j++)
                        {
                            if (Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value) == Convert.ToInt32(dataGridView2.Rows[j].Cells[1].Value))
                            {
                                dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.GreenYellow;
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        // Запрос на вывод ФИО пользователя сессии
        internal void Return_Login_User_FIO(string login,ToolStripMenuItem режимToolStripMenuItem, TextBox textBoxFIO, TextBox textBoxFIoSotr, TextBox textBoxIDSotr, SqlConnection cn)
        {
            try
            {
                SqlCommand thisCommand = cn.CreateCommand();
                //thisCommand.CommandText = $"SELECT [Сотрудники предприятия].[Код сотрудника], [Сотрудники предприятия].[Фамилия]+' '+[Сотрудники предприятия].[Имя]+' '+[Сотрудники предприятия].[Отчество] AS ФИО, [Сотрудники предприятия].[Логин для входа в систему] FROM[Сотрудники предприятия] WHERE((([Сотрудники предприятия].[Логин для входа в систему]) = '{login}')); ";
                thisCommand.CommandText = "SELECT [Сотрудники предприятия].[Код сотрудника], [Сотрудники предприятия].[Фамилия]+' '+[Сотрудники предприятия].[Имя]+' '+[Сотрудники предприятия].[Отчество] AS ФИО, [Сотрудники предприятия].[Логин для входа в систему] FROM[Сотрудники предприятия] WHERE((([Сотрудники предприятия].[Логин для входа в систему]) = '"+ login +"')); ";
                using (SqlDataReader thisReader = thisCommand.ExecuteReader())
                {
                    thisReader.Read();
                    textBoxFIO.Text = thisReader[1].ToString();
                    textBoxFIoSotr.Text = thisReader[1].ToString();
                    режимToolStripMenuItem.Text = режимToolStripMenuItem.Text + thisReader[1].ToString();
                    textBoxIDSotr.Text = thisReader[0].ToString();
                    //thisReader.Close();
                }
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }

        }
        // Запрос на возврат кода сотрудника по ФИО
        internal int search_fio(string fio,SqlConnection cn)
        {
            SqlCommand thisCommand = cn.CreateCommand();
            thisCommand.CommandText = $"SELECT [Сотрудники предприятия].[Код сотрудника], [Сотрудники предприятия].[Фамилия] + ' ' + [Сотрудники предприятия].[Имя] + ' ' +[Сотрудники предприятия].[Отчество] AS ФИО " +
                $"FROM[Сотрудники предприятия] WHERE((([Сотрудники предприятия].[Фамилия] + ' ' + [Сотрудники предприятия].[Имя] + ' ' + [Сотрудники предприятия].[Отчество]) = N'{fio}'));";
            using (SqlDataReader thisReader = thisCommand.ExecuteReader())
            {
                thisReader.Read();
                int number = Convert.ToInt32(thisReader.GetValue(0).ToString());
                thisReader.Close();
                return number;
            }
        }
        // Запрос на возврат кода причины списания
        internal int search_id_pr_spis(string combobox, SqlConnection cn)
        {
            int number = 0;
            try
            {
                SqlCommand thisCommand = cn.CreateCommand();
                //thisCommand.CommandText = $"SELECT [Причины списания].[Код причины], [Причины списания].Наименование FROM [Причины списания] WHERE ((([Причины списания].Наименование)=N'{combobox}'));";
                thisCommand.CommandText = $"SELECT [Причины списания].[Код причины], [Причины списания].Наименование FROM [Причины списания] WHERE ((([Причины списания].Наименование)='{combobox}'));";
                using (SqlDataReader thisReader = thisCommand.ExecuteReader())
                {
                    thisReader.Read();
                    number = Convert.ToInt32(thisReader.GetValue(0).ToString());
                    thisReader.Close();
                }               
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
            return number;
        }
        // Запрос на количество поставок всех ключей для диаграммы
        internal void Request_for_the_number_of_deliveries(Chart chartCountSupplyKey, TextBox textBox2, SqlConnection cn)
        {
            //List<string[]> data = new List<string[]>();
            List<string[]> data = new List<string[]>(5);
            try
            {
                int sum = 0;
                SqlCommand thisCommand = cn.CreateCommand();
                thisCommand.CommandText = "SELECT [Носители ЭК].Наименование, Sum([Поставка ключей].[Количество поставленных ключей]) AS Сумма " +
                    "FROM[Носители ЭК] INNER JOIN[Поставка ключей] ON[Носители ЭК].[Код носителя] = [Поставка ключей].Ключ " +
                    "GROUP BY[Носители ЭК].Наименование; ";
                using (SqlDataReader thisReader = thisCommand.ExecuteReader())
                {
                    thisReader.Read();
                    while (thisReader.Read())
                    {
                        data.Add(new string[2]);
                        data[data.Count - 1][0] = thisReader[0].ToString();
                        data[data.Count - 1][1] = thisReader[1].ToString();
                    }
                    thisReader.Close();
                }
                chartCountSupplyKey.Series[0].Points.Clear();
                for (int i = 0; i < data.Count; i++)
                {
                    chartCountSupplyKey.Series[0].Points.AddY(Convert.ToInt32(data[i][1].ToString()));
                    chartCountSupplyKey.Series[0].Points[i].LegendText = data[i][0].ToString();
                    sum += Convert.ToInt32(data[i][1].ToString());
                }
                chartCountSupplyKey.Series[0].IsValueShownAsLabel = true;
                textBox2.Text = sum.ToString();
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
            finally
            {
                data.Clear();
            }
        }
        // Запрос 3. Запрос с использованием логической функции IIf
        internal void zapros_3(DataGridView dataGridView8, SqlConnection cn)
        {
            List<string[]> data = new List<string[]>();
            try
            {
                SqlCommand thisCommand = cn.CreateCommand();
                thisCommand.CommandText = "SELECT [Сотрудники предприятия].[Код сотрудника], [Сотрудники предприятия].[Фамилия]+' '+[Сотрудники предприятия].[Имя]+' '+[Сотрудники предприятия].[Отчество] AS ФИО, IIf(Count([Выдача ЭК].[Код выдачи])>1,Count([Выдача ЭК].[Код выдачи]),0) AS Количество FROM([Носители ЭК] INNER JOIN([Поставка ключей] INNER JOIN [Идентификационные номера ЭК] ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя]) ON[Носители ЭК].[Код носителя] = [Поставка ключей].Ключ) INNER JOIN(([Сотрудники предприятия] INNER JOIN [Заявление на регистрацию ЭК] ON[Сотрудники предприятия].[Код сотрудника] = [Заявление на регистрацию ЭК].Сотрудник) INNER JOIN[Выдача ЭК] ON[Заявление на регистрацию ЭК].[Код заявления] = [Выдача ЭК].[Регистрационное заявление]) ON[Идентификационные номера ЭК].[Идентификационный номер] = [Выдача ЭК].[Выданный ключ] GROUP BY[Сотрудники предприятия].[Код сотрудника], [Сотрудники предприятия].[Фамилия] + ' ' +[Сотрудники предприятия].[Имя] + ' ' +[Сотрудники предприятия].[Отчество]; ";
                using (SqlDataReader thisReader = thisCommand.ExecuteReader())
                {
                    while (thisReader.Read())
                    {
                        data.Add(new string[3]);
                        data[data.Count - 1][0] = thisReader[0].ToString();
                        data[data.Count - 1][1] = thisReader[1].ToString();
                        data[data.Count - 1][2] = thisReader[2].ToString();
                    }
                    thisReader.Close();
                }
                dataGridView8.Columns.Clear();
                dataGridView8.ColumnCount = 3;
                dataGridView8.Columns[0].HeaderText = "Код сотрудника";
                dataGridView8.Columns[1].HeaderText = "ФИО";
                dataGridView8.Columns[2].HeaderText = "Количество ключей";
                foreach (string[] s in data)
                {
                    if (s[2] != "0")
                    {
                        dataGridView8.Rows.Add(s);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
            finally
            {
                data.Clear();
            }
        }
        // Запрос на вывод информации по сумме закупленных ключей
        internal void Request_for_the_sum_of_deliveries(Chart chartSumSupplyKey, TextBox textBox1, SqlConnection cn)
        {
            List<string[]> data = new List<string[]>();
            try
            {
                chartSumSupplyKey.Series.Clear();
                double sum = 0;
                SqlCommand thisCommand = cn.CreateCommand();
                thisCommand.CommandText = "SELECT [Носители ЭК].Наименование, Sum([Поставка ключей].[Стоимость за единицу]*[Поставка ключей].[Количество поставленных ключей]) AS Сумма " +
                    "FROM[Носители ЭК] INNER JOIN [Поставка ключей] ON[Носители ЭК].[Код носителя] = [Поставка ключей].Ключ " +
                    "GROUP BY[Носители ЭК].Наименование; ";
                using (SqlDataReader thisReader = thisCommand.ExecuteReader())
                {
                    thisReader.Read();
                    while (thisReader.Read())
                    {
                        data.Add(new string[2]);
                        data[data.Count - 1][0] = thisReader[0].ToString();
                        data[data.Count - 1][1] = thisReader[1].ToString();
                    }
                    thisReader.Close();
                }
                for (int i = 0; i < data.Count; i++)
                {
                    chartSumSupplyKey.Series.Add(data[i][0].ToString());
                    chartSumSupplyKey.Series[i].Points.AddXY(i + 1, Convert.ToDecimal(data[i][1].ToString()));
                    chartSumSupplyKey.Series[i].Label = Math.Round(Convert.ToDecimal(data[i][1].ToString())) + " р.".ToString();
                    sum += Math.Round(Convert.ToDouble(data[i][1].ToString()));
                }
                textBox1.Text = sum.ToString();
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
            finally
            {
                data.Clear();
            }
        }
        // Запрос на вывод информации по количеству выданных ключей
        internal void Request_for_the_count_of_given(Chart chartCountGivenKey, TextBox textBox3, SqlConnection cn)
        {
            List<string[]> data = new List<string[]>();
            try
            {
                chartCountGivenKey.Series[0].Points.Clear();
                double sum = 0;
                SqlCommand thisCommand = cn.CreateCommand();
                thisCommand.CommandText = "SELECT [Носители ЭК].Наименование, Count([Выдача ЭК].[Выданный ключ]) AS Количество " +
                    "FROM[Носители ЭК] INNER JOIN([Поставка ключей] INNER JOIN ([Идентификационные номера ЭК] INNER JOIN [Выдача ЭК] ON[Идентификационные номера ЭК].[Идентификационный номер] = [Выдача ЭК].[Выданный ключ]) ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя]) ON([Идентификационные номера ЭК].[Код носителя] = [Носители ЭК].[Код носителя]) AND([Носители ЭК].[Код носителя] = [Поставка ключей].Ключ) " +
                    "GROUP BY[Носители ЭК].Наименование; ";
                thisCommand.CommandText = "SELECT [Носители ЭК].Наименование, Count([Выдача ЭК].[Выданный ключ]) AS Количество " +
                  "FROM[Носители ЭК] INNER JOIN([Поставка ключей] INNER JOIN ([Идентификационные номера ЭК] INNER JOIN [Выдача ЭК] ON[Идентификационные номера ЭК].[Идентификационный номер] = [Выдача ЭК].[Выданный ключ]) ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя]) ON([Идентификационные номера ЭК].[Код носителя] = [Носители ЭК].[Код носителя]) AND([Носители ЭК].[Код носителя] = [Поставка ключей].Ключ) " +
                  "GROUP BY[Носители ЭК].Наименование; ";
                using (SqlDataReader thisReader = thisCommand.ExecuteReader())
                {
                    while (thisReader.Read())
                    {
                        data.Add(new string[2]);
                        data[data.Count - 1][0] = thisReader[0].ToString();
                        data[data.Count - 1][1] = thisReader[1].ToString();
                    }
                    thisReader.Close();
                }
                for (int i = 0; i < data.Count; i++)
                {
                    chartCountGivenKey.Series[0].Points.AddY(Convert.ToInt32(data[i][1].ToString()));
                    chartCountGivenKey.Series[0].Points[i].LegendText = data[i][0].ToString();
                    sum += Convert.ToInt32(data[i][1].ToString());
                }
                textBox3.Text = sum.ToString();
                chartCountGivenKey.Series[0].IsValueShownAsLabel = true;
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
            finally
            {
                data.Clear();
            }
        }
        internal void Request_for_the_count(DataGridView dataGridView3, TextBox textBox4, SqlConnection cn)
        {
            double sum = 0;
            List<string[]> data = new List<string[]>();
            try
            {
                SqlCommand thisCommand = cn.CreateCommand();
                thisCommand.CommandText = "SELECT [Носители ЭК].Наименование FROM [Носители ЭК]; ";
                using (SqlDataReader thisReader = thisCommand.ExecuteReader())
                {
                    while (thisReader.Read())
                    {
                        data.Add(new string[6]);
                        data[data.Count - 1][0] = thisReader[0].ToString();
                        data[data.Count - 1][1] = 0.ToString();
                        data[data.Count - 1][2] = 0.ToString();
                        data[data.Count - 1][3] = 0.ToString();
                        data[data.Count - 1][4] = 10.ToString();
                        data[data.Count - 1][5] = 0.ToString();
                    }
                    thisReader.Close();
                }
                // Кол-во поставленных
                thisCommand.CommandText = "SELECT [Носители ЭК].Наименование, Sum([Поставка ключей].[Количество поставленных ключей]) AS Сумма " +
                    "FROM[Носители ЭК] INNER JOIN[Поставка ключей] ON[Носители ЭК].[Код носителя] = [Поставка ключей].Ключ " +
                    "GROUP BY[Носители ЭК].Наименование; ";
                using (SqlDataReader thisReader2 = thisCommand.ExecuteReader())
                {
                    while (thisReader2.Read())
                    {
                        for (int i = 0; i < data.Count; i++)
                        {
                            if (thisReader2[0].ToString() == data[i][0])
                            {
                                data[i][1] = thisReader2[1].ToString();
                            }
                        }
                    }
                    thisReader2.Close();
                }
                //  Кол-во выданных
                thisCommand.CommandText = "SELECT [Носители ЭК].Наименование, Count([Выдача ЭК].[Выданный ключ]) AS Количество " +
                    "FROM[Носители ЭК] INNER JOIN([Поставка ключей] INNER JOIN ([Идентификационные номера ЭК] INNER JOIN [Выдача ЭК] ON[Идентификационные номера ЭК].[Идентификационный номер] = [Выдача ЭК].[Выданный ключ]) ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя]) ON([Идентификационные номера ЭК].[Код носителя] = [Носители ЭК].[Код носителя]) AND([Носители ЭК].[Код носителя] = [Поставка ключей].Ключ) " +
                    "GROUP BY[Носители ЭК].Наименование; ";
                using (SqlDataReader thisReader3 = thisCommand.ExecuteReader())
                {
                    while (thisReader3.Read())
                    {
                        for (int i = 0; i < data.Count; i++)
                        {
                            if (thisReader3[0].ToString() == data[i][0])
                            {
                                data[i][2] = thisReader3[1].ToString();
                            }
                        }
                    }
                    thisReader3.Close();
                }
                for (int i = 0; i < data.Count; i++)
                {
                    data[i][3] = (Convert.ToInt32(data[i][1]) - Convert.ToInt32(data[i][2])).ToString();
                    int diff = Convert.ToInt32(data[i][4]) - Convert.ToInt32(data[i][3]);
                    if (diff <= 0)
                    {
                        data[i][5] = "0";
                    }
                    else
                    {
                        data[i][5] = diff.ToString();
                        sum += diff;
                    }
                }
                dataGridView3.Columns.Clear();
                dataGridView3.ColumnCount = 6;
                dataGridView3.Columns[0].HeaderText = "Наименование ЭК";
                dataGridView3.Columns[1].HeaderText = "Кол-во поставленных";
                dataGridView3.Columns[2].HeaderText = "Кол-во выданных";
                dataGridView3.Columns[3].HeaderText = "Остатки на складе";
                dataGridView3.Columns[4].HeaderText = "Минимальный порог кол-ва в наличии";
                dataGridView3.Columns[5].HeaderText = "Кол-во для заказа";
                foreach (string[] s in data)
                {
                    dataGridView3.Rows.Add(s);
                }
                textBox4.Text = sum.ToString();
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
            finally
            {
                data.Clear();
            }
        }
        // Запрос на вывод информации о выданных ключей сотрудникам  // заполнение comboBox5
        internal void Completion_ComboBox_Cotr_key(ComboBox comboBox5, string fio, SqlConnection cn)
        {
            List<string[]> data = new List<string[]>();
            List<string[]> data2 = new List<string[]>();
            try
            {
                comboBox5.Items.Clear();
                SqlCommand thisCommand = cn.CreateCommand();
                thisCommand.CommandText = $"SELECT [Сотрудники предприятия].[Фамилия]+' '+[Сотрудники предприятия].[Имя]+' '+[Сотрудники предприятия].[Отчество] AS ФИО, [Идентификационные номера ЭК].[Идентификационный номер], [Носители ЭК].Наименование FROM[Носители ЭК] INNER JOIN([Поставка ключей] INNER JOIN ([Идентификационные номера ЭК] INNER JOIN ([Сотрудники предприятия] INNER JOIN ([Заявление на регистрацию ЭК] INNER JOIN [Выдача ЭК] ON[Заявление на регистрацию ЭК].[Код заявления] = [Выдача ЭК].[Регистрационное заявление]) ON[Сотрудники предприятия].[Код сотрудника] = [Заявление на регистрацию ЭК].Сотрудник) ON[Идентификационные номера ЭК].[Идентификационный номер] = [Выдача ЭК].[Выданный ключ]) ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя]) ON[Носители ЭК].[Код носителя] = [Поставка ключей].Ключ WHERE((([Сотрудники предприятия].[Фамилия] + ' ' +[Сотрудники предприятия].[Имя] + ' ' +[Сотрудники предприятия].[Отчество]) = N'{fio}')); ";
                using (SqlDataReader thisReader = thisCommand.ExecuteReader())
                {
                    while (thisReader.Read())
                    {
                        data.Add(new string[2]);
                        data[data.Count - 1][0] = thisReader[1].ToString();
                        data[data.Count - 1][1] = thisReader[2].ToString();
                    }
                    thisReader.Close();
                }
                thisCommand.CommandText = $"SELECT [Сотрудники предприятия].[Код сотрудника], [Идентификационные номера ЭК].[Идентификационный номер], [Списание ключей].[Дата списания] FROM([Носители ЭК] INNER JOIN[Поставка ключей] ON[Носители ЭК].[Код носителя] = [Поставка ключей].Ключ) INNER JOIN([Идентификационные номера ЭК] INNER JOIN (([Сотрудники предприятия] INNER JOIN [Заявление на регистрацию ЭК] ON [Сотрудники предприятия].[Код сотрудника] = [Заявление на регистрацию ЭК].Сотрудник) INNER JOIN([Выдача ЭК] INNER JOIN ([Заявление на списание ЭК] INNER JOIN [Списание ключей] ON[Заявление на списание ЭК].[Код заявления] = [Списание ключей].[Код заявления]) ON[Выдача ЭК].[Код выдачи] = [Заявление на списание ЭК].[Код выдачи]) ON[Заявление на регистрацию ЭК].[Код заявления] = [Выдача ЭК].[Регистрационное заявление]) ON[Идентификационные номера ЭК].[Идентификационный номер] = [Выдача ЭК].[Выданный ключ]) ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя] WHERE((([Сотрудники предприятия].[Фамилия] + ' ' +[Сотрудники предприятия].[Имя] + ' ' +[Сотрудники предприятия].[Отчество]) = N'{fio}'));";
                using (SqlDataReader thisReader2 = thisCommand.ExecuteReader())
                {
                    while (thisReader2.Read())
                    {
                        data2.Add(new string[1]);
                        data2[data2.Count - 1][0] = thisReader2[1].ToString();
                    }
                    thisReader2.Close();
                }
                bool flag = false;
                for (int i = 0; i<data.Count;i++)
                {
                    for(int j = 0; j<data2.Count;j++)
                    {
                        if (data[i][0] == data2[j][0])
                        {
                            flag = true;
                        }
                    }
                    if (flag == false)
                    {
                        comboBox5.Items.Add($"{data[i][0]} - {data[i][1]}");
                    }
                    flag = false;
                }
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
            finally
            {
                data.Clear();
                data2.Clear();
            }
        }
        // Запрос на вывод номера выдачи ключа
        internal int search_number_given(string fio, int number_key, SqlConnection cn)
        {
            SqlCommand thisCommand = cn.CreateCommand();
            thisCommand.CommandText = $"SELECT [Выдача ЭК].[Код выдачи], [Сотрудники предприятия].[Фамилия]+' '+[Сотрудники предприятия].[Имя]+' '+[Сотрудники предприятия].[Отчество] AS ФИО, [Идентификационные номера ЭК].[Идентификационный номер] FROM[Носители ЭК] INNER JOIN([Поставка ключей] INNER JOIN ([Идентификационные номера ЭК] INNER JOIN ([Сотрудники предприятия] INNER JOIN ([Заявление на регистрацию ЭК] INNER JOIN [Выдача ЭК] ON[Заявление на регистрацию ЭК].[Код заявления] = [Выдача ЭК].[Регистрационное заявление]) ON[Сотрудники предприятия].[Код сотрудника] = [Заявление на регистрацию ЭК].Сотрудник) ON[Идентификационные номера ЭК].[Идентификационный номер] = [Выдача ЭК].[Выданный ключ]) ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя]) ON[Носители ЭК].[Код носителя] = [Поставка ключей].Ключ WHERE((([Сотрудники предприятия].[Фамилия] + ' ' +[Сотрудники предприятия].[Имя] + ' ' +[Сотрудники предприятия].[Отчество]) = N'{fio}')) AND((([Идентификационные номера ЭК].[Идентификационный номер]) = N'{number_key}')); ";
            using (SqlDataReader thisReader = thisCommand.ExecuteReader())
            {
                thisReader.Read();
                int number = Convert.ToInt32(thisReader[0].ToString());
                thisReader.Close();
                return number;
            }
        }
        // Запрос на вывод информации об выданных ключах в dataGridView для последующего списания
        internal void debiting_keys_dataGridView(DataGridView dataGridView4, SqlConnection cn)
        {
            bool flag = false;
            List<string[]> data = new List<string[]>();
            List<string[]> data2 = new List<string[]>();
            try
            {
                dataGridView4.Columns.Clear();
                dataGridView4.ColumnCount = 10;
                dataGridView4.Columns[0].HeaderText = "Код выдачи";
                dataGridView4.Columns[1].HeaderText = "Иден. номер ЭК";
                dataGridView4.Columns[2].HeaderText = "Название";
                dataGridView4.Columns[3].HeaderText = "Дата изготовления";
                dataGridView4.Columns[4].HeaderText = "Срок службы (мес.)";
                dataGridView4.Columns[5].HeaderText = "ФИО";
                dataGridView4.Columns[6].HeaderText = "Отдел";
                dataGridView4.Columns[7].HeaderText = "Метод аутентификации";
                dataGridView4.Columns[8].HeaderText = "Дата окончания срока годности";
                dataGridView4.Columns[9].HeaderText = "Кол-во дней до окончания";
                SqlCommand thisCommand = cn.CreateCommand();
                thisCommand.CommandText = "SELECT [Выдача ЭК].[Код выдачи], [Идентификационные номера ЭК].[Идентификационный номер], [Носители ЭК].Наименование, [Поставка ключей].[Дата изготовления], [Носители ЭК].[Срок службы (мес)], [Сотрудники предприятия].[Фамилия]+' '+[Сотрудники предприятия].[Имя]+' '+[Сотрудники предприятия].[Отчество] AS ФИО, Отделы.[Наименование отдела], Отделы.[Методы аутентификации], DateAdd(month,[Срок службы (мес)],[Дата изготовления]) AS [Окончание срока действия ЭК] FROM Отделы INNER JOIN([Сотрудники предприятия] INNER JOIN ([Заявление на регистрацию ЭК] INNER JOIN (([Носители ЭК] INNER JOIN [Поставка ключей] ON [Носители ЭК].[Код носителя] = [Поставка ключей].Ключ) INNER JOIN([Идентификационные номера ЭК] INNER JOIN [Выдача ЭК] ON[Идентификационные номера ЭК].[Идентификационный номер] = [Выдача ЭК].[Выданный ключ]) ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя]) ON[Заявление на регистрацию ЭК].[Код заявления] = [Выдача ЭК].[Регистрационное заявление]) ON[Сотрудники предприятия].[Код сотрудника] = [Заявление на регистрацию ЭК].Сотрудник) ON Отделы.[Номер отдела] = [Сотрудники предприятия].Отдел;";
                using (SqlDataReader thisReader = thisCommand.ExecuteReader())
                {
                    while (thisReader.Read())
                    {
                        data.Add(new string[10]);
                        data[data.Count - 1][0] = thisReader[0].ToString();
                        data[data.Count - 1][1] = thisReader[1].ToString();
                        data[data.Count - 1][2] = thisReader[2].ToString();
                        data[data.Count - 1][3] = Convert.ToDateTime(thisReader[3]).ToShortDateString();
                        data[data.Count - 1][4] = thisReader[4].ToString();
                        data[data.Count - 1][5] = thisReader[5].ToString();
                        data[data.Count - 1][6] = thisReader[6].ToString();
                        data[data.Count - 1][7] = thisReader[7].ToString();
                        data[data.Count - 1][8] = Convert.ToDateTime(thisReader[8]).ToShortDateString();
                        data[data.Count - 1][9] = ((Convert.ToDateTime(data[data.Count - 1][8]) - DateTime.Now).Days).ToString();
                    }
                    thisReader.Close();
                }
                thisCommand.CommandText = "SELECT [Идентификационные номера ЭК].[Идентификационный номер], [Списание ключей].[Дата списания] FROM([Идентификационные номера ЭК] INNER JOIN([Выдача ЭК] INNER JOIN [Заявление на списание ЭК] ON[Выдача ЭК].[Код выдачи] = [Заявление на списание ЭК].[Код выдачи]) ON[Идентификационные номера ЭК].[Идентификационный номер] = [Выдача ЭК].[Выданный ключ]) INNER JOIN[Списание ключей] ON[Заявление на списание ЭК].[Код заявления] = [Списание ключей].[Код заявления];";
                using (SqlDataReader thisReader2 = thisCommand.ExecuteReader())
                {
                    while (thisReader2.Read())
                    {
                        data2.Add(new string[1]);
                        data2[data2.Count - 1][0] = thisReader2[0].ToString();
                    }
                    thisReader2.Close();
                }
                int j = 0;
                foreach (string[] s in data)
                {
                    for (int i = 0; i < data2.Count; i++)
                    {
                        if (Convert.ToInt32(s[1]) == Convert.ToInt32(data2[i][0]))
                        {
                            flag = true;
                        }
                    }
                    if (flag == false)
                    {
                        dataGridView4.Rows.Add(s);
                    }
                    j++;
                    flag = false;
                }
                for (int i = 0; i < dataGridView4.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dataGridView4.Rows[i].Cells[9].Value) >= 100)
                    {
                        dataGridView4.Rows[i].Cells[9].Style.BackColor = Color.GreenYellow;
                    }
                    else if (Convert.ToInt32(dataGridView4.Rows[i].Cells[9].Value) < 100 & Convert.ToInt32(dataGridView4.Rows[i].Cells[9].Value) > 5)
                    {
                        dataGridView4.Rows[i].Cells[9].Style.BackColor = Color.Orange;
                    }
                    else
                    {
                        dataGridView4.Rows[i].Cells[9].Style.BackColor = Color.Red;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
            finally
            {
                data.Clear();
                data2.Clear();
            }
        }
        // Запрос на вывод доступных ключей для выдачи для заполнения в comboBox
        internal void debiting_keys_ComboBox(ComboBox comboBox3, SqlConnection cn)
        {
            List<string[]> data = new List<string[]>();
            List<string[]> data2 = new List<string[]>();
            try
            {
                comboBox3.Items.Clear();
                comboBox3.Text = string.Empty;
                SqlCommand thisCommand = cn.CreateCommand();
                thisCommand.CommandText = "SELECT [Идентификационные номера ЭК].[Идентификационный номер], [Поставка ключей].[Дата изготовления], [Носители ЭК].[Вид носителя], [Носители ЭК].Наименование FROM[Носители ЭК] INNER JOIN([Поставка ключей] INNER JOIN [Идентификационные номера ЭК] ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя]) ON[Носители ЭК].[Код носителя] = [Поставка ключей].Ключ;";
                using (SqlDataReader thisReader = thisCommand.ExecuteReader())
                {
                    while (thisReader.Read())
                    {
                        data.Add(new string[4]);
                        data[data.Count - 1][0] = thisReader[0].ToString();
                        data[data.Count - 1][1] = Convert.ToDateTime(thisReader[1]).ToShortDateString();
                        data[data.Count - 1][2] = thisReader[2].ToString();
                        data[data.Count - 1][3] = thisReader[3].ToString();
                    }
                    thisReader.Close();
                }
                thisCommand.CommandText = "SELECT [Идентификационные номера ЭК].[Идентификационный номер], [Выдача ЭК].[Дата выдачи] FROM([Носители ЭК] INNER JOIN([Поставка ключей] INNER JOIN [Идентификационные номера ЭК] ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя]) ON[Носители ЭК].[Код носителя] = [Поставка ключей].Ключ) INNER JOIN[Выдача ЭК] ON[Идентификационные номера ЭК].[Идентификационный номер] = [Выдача ЭК].[Выданный ключ];";
                using (SqlDataReader thisReader2 = thisCommand.ExecuteReader())
                {
                    while (thisReader2.Read())
                    {
                        data2.Add(new string[2]);
                        data2[data2.Count - 1][0] = thisReader2[0].ToString();
                        data2[data2.Count - 1][1] = thisReader2[1].ToString();
                    }
                    thisReader2.Close();
                }
                for (int i = 0; i < data.Count; i++)
                {
                    bool flag = false;
                    for (int j = 0; j < data2.Count; j++)
                    {
                        if (data[i][0] == data2[j][0])
                        {
                            flag = true;
                        }
                    }
                    if (flag == false)
                    {
                        comboBox3.Items.Add($"{data[i][0]} - {data[i][1]} - {data[i][2]} - {data[i][3]}");
                    }
                } 
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
            finally
            {
                data.Clear();
                data2.Clear();
            }
        }

        // Запрос на вывод доступных ключей для выдачи для заполнения в comboBox
        internal void debiting_keys_ComboBox2(ComboBox comboBox2, SqlConnection cn)
        { 
            List<string[]> data = new List<string[]>();
            List<string[]> data2 = new List<string[]>();
            //try
            //{
                comboBox2.Items.Clear();
                comboBox2.Text = string.Empty;
                SqlCommand thisCommand = cn.CreateCommand();
                thisCommand.CommandText = "SELECT [Идентификационные номера ЭК].[Идентификационный номер], [Поставка ключей].[Дата изготовления], [Носители ЭК].[Вид носителя], [Носители ЭК].Наименование FROM[Носители ЭК] INNER JOIN([Поставка ключей] INNER JOIN [Идентификационные номера ЭК] ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя]) ON[Носители ЭК].[Код носителя] = [Поставка ключей].Ключ;";
                using (SqlDataReader thisReader = thisCommand.ExecuteReader())
                {
                    while (thisReader.Read())
                    {
                        data.Add(new string[4]);
                        data[data.Count - 1][0] = thisReader[0].ToString();
                        data[data.Count - 1][1] = Convert.ToDateTime(thisReader[1]).ToShortDateString();
                        data[data.Count - 1][2] = thisReader[2].ToString();
                        data[data.Count - 1][3] = thisReader[3].ToString();
                    }
                    thisReader.Close();
                }
                thisCommand.CommandText = "SELECT [Идентификационные номера ЭК].[Идентификационный номер], [Выдача ЭК].[Дата выдачи] FROM([Носители ЭК] INNER JOIN([Поставка ключей] INNER JOIN [Идентификационные номера ЭК] ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя]) ON[Носители ЭК].[Код носителя] = [Поставка ключей].Ключ) INNER JOIN[Выдача ЭК] ON[Идентификационные номера ЭК].[Идентификационный номер] = [Выдача ЭК].[Выданный ключ];";
                using (SqlDataReader thisReader2 = thisCommand.ExecuteReader())
                {
                    while (thisReader2.Read())
                    {
                        data2.Add(new string[2]);
                        data2[data2.Count - 1][0] = thisReader2[0].ToString();
                        data2[data2.Count - 1][1] = thisReader2[1].ToString();
                    }
                    thisReader2.Close();
                }
                for (int i = 0; i < data.Count; i++)
                {
                    bool flag = false;
                    for (int j = 0; j < data2.Count; j++)
                    {
                        if (data[i][0] == data2[j][0])
                        {
                            flag = true;
                        }
                    }
                    if (flag == false)
                    {
                        //comboBox2.Items.Add($"{data[i][0]} - {data[i][1]} - {data[i][2]} - {data[i][3]}");
                        comboBox2.Items.Add($"{data[i][0]} - {data[i][1]} - {data[i-1][2]} - {data[i+7][3]}");
                    }
                }
            //}
            //catch
            //{
            //    MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            //}
            //finally
            //{

            //    data.Clear();
            //    data2.Clear();
            //}
        }
        // Запрос на возврат номера заявления на списания  
        internal int number_zayv_na_spisanie(int num_given_key, SqlConnection cn)
        {
            SqlCommand thisCommand = cn.CreateCommand();
            thisCommand.CommandText = $"SELECT [Выдача ЭК].[Код выдачи], [Заявление на списание ЭК].[Код заявления] FROM[Выдача ЭК] INNER JOIN[Заявление на списание ЭК] ON[Выдача ЭК].[Код выдачи] = [Заявление на списание ЭК].[Код выдачи] WHERE((([Выдача ЭК].[Код выдачи]) = {num_given_key})); ";
            using (SqlDataReader thisReader = thisCommand.ExecuteReader())
            {
                thisReader.Read();
                int number = Convert.ToInt32(thisReader[1].ToString());
                thisReader.Close();
                return number;
            }
        }
        // Запрос 2. Запрос на выборку в определнном промежутке 
        internal void zapros_2(DataGridView dataGridView7,int left_gr, int right_gr,int month_number, SqlConnection cn)
        {
            //try
            //{
            //    string CommandText = $"SELECT [Идентификационные номера ЭК].[Идентификационный номер], [Носители ЭК].[Вид носителя], [Носители ЭК].Наименование, [Поставка ключей].[Стоимость за единицу], [Поставка ключей].[Дата изготовления] FROM([Носители ЭК] INNER JOIN[Поставка ключей] ON[Носители ЭК].[Код носителя] = [Поставка ключей].Ключ) INNER JOIN[Идентификационные номера ЭК] ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя] WHERE((([Поставка ключей].[Стоимость за единицу]) between {left_gr} and {right_gr})) and ((Month([Поставка ключей].[Дата изготовления]) = {month_number})); ";
            //    using (SqlDataAdapter dataAdapter1 = new SqlDataAdapter(CommandText, cn))
            //    {
            //        DataSet dataSet1 = new DataSet();
            //        dataAdapter1.Fill(dataSet1);
            //        dataGridView7.DataSource = dataSet1.Tables[0];
            //    }
            //}
            //catch
            //{
            //    MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            //}    
            string CommandText = $"SELECT [Идентификационные номера ЭК].[Идентификационный номер], [Носители ЭК].[Вид носителя], [Носители ЭК].Наименование, [Поставка ключей].[Стоимость за единицу], [Поставка ключей].[Дата изготовления] FROM([Носители ЭК] INNER JOIN[Поставка ключей] ON[Носители ЭК].[Код носителя] = [Поставка ключей].Ключ) INNER JOIN[Идентификационные номера ЭК] ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя] WHERE((([Поставка ключей].[Стоимость за единицу]) between {left_gr} and {right_gr})) and ((Month([Поставка ключей].[Дата изготовления]) = {month_number})); ";
            using (SqlDataAdapter dataAdapter1 = new SqlDataAdapter(CommandText, cn))
            {
                DataSet dataSet1 = new DataSet();
                dataAdapter1.Fill(dataSet1);
                dataGridView7.DataSource = dataSet1.Tables[0];
            }
        }
        // Запрос на возврат кода сотрудника по коду выдачи 
        internal int number_sotr(int num_given_key, SqlConnection cn)
        {
            SqlCommand thisCommand = cn.CreateCommand();
            thisCommand.CommandText = $"SELECT [Выдача ЭК].[Код выдачи], [Сотрудники предприятия].[Код сотрудника], [Сотрудники предприятия].[Фамилия]+' '+[Сотрудники предприятия].[Имя]+' '+[Сотрудники предприятия].[Отчество] AS ФИО FROM[Сотрудники предприятия] INNER JOIN([Заявление на регистрацию ЭК] INNER JOIN [Выдача ЭК] ON[Заявление на регистрацию ЭК].[Код заявления] = [Выдача ЭК].[Регистрационное заявление]) ON[Сотрудники предприятия].[Код сотрудника] = [Заявление на регистрацию ЭК].Сотрудник WHERE((([Выдача ЭК].[Код выдачи]) = {num_given_key})); ";
            using (SqlDataReader thisReader = thisCommand.ExecuteReader())
            {
                thisReader.Read();
                int number = Convert.ToInt32(thisReader[1].ToString());
                thisReader.Close();
                return number;
            } 
        }
        // Перекрестный запрос 
        internal void cross_request(DataGridView dataGridView5, SqlConnection cn)
        {
            List<string[]> data = new List<string[]>();
            List<string[]> data2 = new List<string[]>();
            List<string[]> data3 = new List<string[]>();
            try
            {
                SqlCommand thisCommand = cn.CreateCommand();
                thisCommand.CommandText = "SELECT [Носители ЭК].Наименование FROM[Носители ЭК]; ";
                using (SqlDataReader thisReader = thisCommand.ExecuteReader())
                {
                    while (thisReader.Read())
                    {
                        data.Add(new string[1]);
                        data[data.Count - 1][0] = thisReader[0].ToString();
                    }
                    thisReader.Close();
                }
                dataGridView5.Columns.Clear();
                dataGridView5.ColumnCount = data.Count + 3;

                thisCommand.CommandText = "SELECT [Сотрудники предприятия].[Код сотрудника], [Сотрудники предприятия].[Фамилия]+' '+[Сотрудники предприятия].[Имя]+' '+[Сотрудники предприятия].[Отчество] AS ФИО FROM[Сотрудники предприятия]; ";
                using (SqlDataReader thisReader2 = thisCommand.ExecuteReader())
                {
                    while (thisReader2.Read())
                    {
                        data2.Add(new string[2]);
                        data2[data2.Count - 1][0] = thisReader2[0].ToString();
                        data2[data2.Count - 1][1] = thisReader2[1].ToString();
                    }
                    thisReader2.Close();
                }
                dataGridView5.RowCount = data2.Count + 2;

                //for (int i = 0; i <= data.Count; i++)
                //{
                //    dataGridView5.Rows[0].Cells[i + 3].Value = data[i][0];
                //}
                for (int i = 0; i <= data.Count; i++)
                {
                    dataGridView5.Rows[0].Cells[i + 3].Value = data[i][0];
                }
                dataGridView5.Rows[0].Cells[2].Value = "Итоги:";
                dataGridView5.Columns[2].DefaultCellStyle.Font = new Font(dataGridView5.DefaultCellStyle.Font, FontStyle.Bold);
                for (int i = 0; i < data2.Count; i++)
                {
                    dataGridView5.Rows[i + 1].Cells[0].Value = data2[i][0];
                    dataGridView5.Rows[i + 1].Cells[1].Value = data2[i][1];
                }
                dataGridView5.Rows[dataGridView5.RowCount - 1].Cells[1].Value = "Итоги:";
                dataGridView5.Rows[dataGridView5.RowCount - 1].DefaultCellStyle.Font = new Font(dataGridView5.DefaultCellStyle.Font, FontStyle.Bold);

                thisCommand.CommandText = "SELECT [Сотрудники предприятия].[Код сотрудника], [Носители ЭК].Наименование, Count([Носители ЭК].Наименование) AS [Count-Наименование] FROM([Носители ЭК] INNER JOIN[Поставка ключей] ON[Носители ЭК].[Код носителя] = [Поставка ключей].Ключ) INNER JOIN([Сотрудники предприятия] INNER JOIN ([Заявление на регистрацию ЭК] INNER JOIN ([Идентификационные номера ЭК] INNER JOIN [Выдача ЭК] ON[Идентификационные номера ЭК].[Идентификационный номер] = [Выдача ЭК].[Выданный ключ]) ON[Заявление на регистрацию ЭК].[Код заявления] = [Выдача ЭК].[Регистрационное заявление]) ON[Сотрудники предприятия].[Код сотрудника] = [Заявление на регистрацию ЭК].Сотрудник) ON[Поставка ключей].[Код поставки] = [Идентификационные номера ЭК].[Код носителя] GROUP BY[Сотрудники предприятия].[Код сотрудника], [Носители ЭК].Наименование; ";
                using (SqlDataReader thisReader3 = thisCommand.ExecuteReader())
                {
                    while (thisReader3.Read())
                    {
                        data3.Add(new string[3]);
                        data3[data3.Count - 1][0] = thisReader3[0].ToString();
                        data3[data3.Count - 1][1] = thisReader3[1].ToString();
                        data3[data3.Count - 1][2] = thisReader3[2].ToString();
                    }
                    thisReader3.Close();
                }

                for (int i = 1; i < dataGridView5.RowCount - 1; i++)
                {
                    for (int j = 3; j < dataGridView5.ColumnCount; j++)
                    {
                        foreach (string[] s in data3)
                        {
                            if (dataGridView5.Rows[0].Cells[j].Value.ToString() == s[1] & dataGridView5.Rows[i].Cells[0].Value.ToString() == s[0])
                            {
                                dataGridView5.Rows[i].Cells[j].Value = s[2];
                            }
                        }
                    }
                }
                int sum_column = 0;
                int sum_row = 0;
                for (int i = 1; i < dataGridView5.RowCount - 1; i++)
                {
                    for (int j = 3; j < dataGridView5.ColumnCount; j++)
                    {
                        if (dataGridView5.Rows[i].Cells[j].Value != null)
                        {
                            sum_row += Convert.ToInt32(dataGridView5.Rows[i].Cells[j].Value);
                        }
                    }
                    dataGridView5.Rows[i].Cells[2].Value = sum_row.ToString();
                    sum_row = 0;
                }
                for (int j = 3; j < dataGridView5.ColumnCount; j++)
                {
                    for (int i = 1; i < dataGridView5.RowCount - 1; i++)
                    {
                        if (dataGridView5.Rows[i].Cells[j].Value != null)
                        {
                            sum_column += Convert.ToInt32(dataGridView5.Rows[i].Cells[j].Value);
                        }
                    }
                    dataGridView5.Rows[dataGridView5.RowCount - 1].Cells[j].Value = sum_column.ToString();
                    sum_column = 0;
                }
                
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
            finally
            {
                data.Clear();
                data2.Clear();
                data3.Clear();
            }
        }
        // Запрос на возврат автоматически сформированного кода записи в таблице заявление на регистрацию 
        internal int number_zayv(int id_sotr, SqlConnection cn)
        {
            bool flag = false;
            int number = 0;
            List<int[]> data = new List<int[]>();
            List<int[]> data2 = new List<int[]>();
            SqlCommand thisCommand = cn.CreateCommand();
            thisCommand.CommandText = $"SELECT [Заявление на регистрацию ЭК].[Код заявления], [Сотрудники предприятия].[Код сотрудника] FROM[Сотрудники предприятия] INNER JOIN[Заявление на регистрацию ЭК] ON[Сотрудники предприятия].[Код сотрудника] = [Заявление на регистрацию ЭК].Сотрудник WHERE((([Сотрудники предприятия].[Код сотрудника]) = {id_sotr}));";
            using (SqlDataReader thisReader = thisCommand.ExecuteReader())
            {
                while (thisReader.Read())
                {
                    data.Add(new int[1]);
                    data[data.Count - 1][0] = Convert.ToInt32(thisReader[0]);
                }
                thisReader.Close();
            }
            thisCommand.CommandText = $"SELECT [Выдача ЭК].[Регистрационное заявление], [Заявление на регистрацию ЭК].[Код заявления], [Сотрудники предприятия].[Код сотрудника] FROM([Сотрудники предприятия] INNER JOIN[Заявление на регистрацию ЭК] ON[Сотрудники предприятия].[Код сотрудника] = [Заявление на регистрацию ЭК].Сотрудник) INNER JOIN[Выдача ЭК] ON[Заявление на регистрацию ЭК].[Код заявления] = [Выдача ЭК].[Регистрационное заявление] WHERE((([Сотрудники предприятия].[Код сотрудника]) = {id_sotr})); ";
            using (SqlDataReader thisReader2 = thisCommand.ExecuteReader())
            {
                while (thisReader2.Read())
                {
                    data2.Add(new int[1]);
                    data2[data2.Count - 1][0] = Convert.ToInt32(thisReader2[1]);
                }
                thisReader2.Close();
            }
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data2.Count; j++)
                {
                    if (data[i][0] == data2[j][0])
                    {
                        flag = true;
                    }
                }
                if (flag == false)
                {
                    number = Convert.ToInt32(data[i][0]);
                }
                flag = false;
            }
            return number;
        }
    }
}
