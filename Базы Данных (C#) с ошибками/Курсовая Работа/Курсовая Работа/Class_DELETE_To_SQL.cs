using System.Data.SqlClient;
using System.Windows.Forms;

namespace Курсовая_Работа
{
    class Class_DELETE_To_SQL
    {
        // Запрос на удаление записи по её ID
        internal void drop_value(SqlConnection cn, string name_table, int id_value, string name_column)
        {
            try
            {
                //using (SqlCommand thisCommand = cn.CreateCommand())
                SqlCommand thisCommand = cn.CreateCommand();
                thisCommand.CommandText = $"DELETE FROM [{name_table}] WHERE [{name_column}] = {id_value};";
                thisCommand.ExecuteNonQuery();
                MessageBox.Show("Успешно", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Во время работы произошла ошибка", "Внимание");
            }

        }

        internal void func1()
        {

        }







    }
}
