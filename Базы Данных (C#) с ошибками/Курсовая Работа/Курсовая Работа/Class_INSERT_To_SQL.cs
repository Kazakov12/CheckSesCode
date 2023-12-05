using System.Windows.Forms;
using System.Data.SqlClient;

namespace Курсовая_Работа
{
    class Class_INSERT_To_SQL
    {
        // Запрос на добавление записи заявление на регистрацию ЭК
        internal void insert_table_regist(SqlConnection cn, int number_sotr, string date)
        {
            try
            {
                using (SqlCommand thisCommand = cn.CreateCommand())
                {
                    thisCommand.CommandText = $"INSERT INTO [Заявление на регистрацию ЭК] ([Сотрудник],[Дата подачи заявления]) VALUES ('{number_sotr}','{date}')";
                    thisCommand.ExecuteNonQuery();
                    MessageBox.Show("Успешно", "Заявление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show($"Во время работы произошла ошибка", "Внимание");
            }
        }
        // Запрос на добавление записи заявление на ремонт ЭК
        internal void insert_table_repair(SqlConnection cn, int number_z, string date)
        {
            try
            {
                using (SqlCommand thisCommand = cn.CreateCommand())
                {
                    thisCommand.CommandText = $"INSERT INTO [Заявление на ремонт ЭК] ([Код выданного ключа],[Дата подачи заявления]) VALUES ('{number_z}','{date}')";
                    thisCommand.ExecuteNonQuery();
                    MessageBox.Show("Успешно", "Заявление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
        }
        // Запрос на добавление записи заявление на списание ЭК
        internal void insert_table_cancellation(SqlConnection cn, int number_z, string date)
        {
            try
            {
                using (SqlCommand thisCommand = cn.CreateCommand())
                {
                    thisCommand.CommandText = $"INSERT INTO [Заявление на списание ЭК] ([Код выдачи],[Дата подачи заявления]) VALUES ('{number_z}','{date}')";
                    thisCommand.ExecuteNonQuery();
                    MessageBox.Show("Успешно", "Заявление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
        }
        // Запрос на добавление записи на ремонт ЭК
        internal void insert_table_repair2(SqlConnection cn, int number_z, string date)
        {
            try
            {
                using (SqlCommand thisCommand = cn.CreateCommand())
                {
                    thisCommand.CommandText = $"INSERT INTO [Ремонт ЭК] ([Код заявления на ремонт],[Дата выполнения ремонта]) VALUES ('{number_z}','{date}')";
                    thisCommand.ExecuteNonQuery();
                    MessageBox.Show("Успешно", "Заявление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch 
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
        }
        // Запрос на добавление записи на списание ЭК
        internal void insert_table_cancellation2(SqlConnection cn, int number_z, string date, int pr_spis)
        {
            try
            {
                using (SqlCommand thisCommand = cn.CreateCommand())
                {
                    thisCommand.CommandText = $"INSERT INTO [Списание ключей] ([Код заявления],[Причина списания],[Дата списания]) VALUES ('{number_z}','{pr_spis}','{date}')";
                    thisCommand.ExecuteNonQuery();
                    MessageBox.Show("Успешно", "Заявление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show($"Во время работы произошла ошибка", "Внимание");
            }
        }
        // Запрос на добавление записи на выдачу ЭК
        internal void insert_table_regist2(SqlConnection cn, int number_z, int number_key, string date)
        {
            try
            {
                using (SqlCommand thisCommand = cn.CreateCommand())
                {

                    thisCommand.CommandText = $"INSERT INTO [Выдача ЭК] ([Регистрационное заявление],[Выданный ключ],[Дата выдачи]) VALUES ('{number_z}','{number_key}','{date}')";
                    thisCommand.ExecuteNonQuery();
                    MessageBox.Show("Успешно", "Заявление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }
        }
    }
}
