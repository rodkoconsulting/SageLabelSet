using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SageLabelSet
{
    class Program
    {
        private const string SqlConnection = "Server=POLMAS01\\SAGE17;Database=POL;Integrated Security=True";
        private const string EndOfItemCodeChar = "_";

        public static void LabelSet(string itemCode, string fileName)
        {
            var connection = new SqlConnection(SqlConnection);
            connection.Open();
            var command = new SqlCommand("SageLabelSet", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.Add("@ItemCode", SqlDbType.VarChar, 30);
            command.Parameters.Add("@FileName", SqlDbType.VarChar, 40);
            command.Parameters["@ItemCode"].Value = itemCode;
            command.Parameters["@FileName"].Value = fileName;
            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
        }

        public static string GetItemCodeFromFileName(string fileName)
        {
            var index = fileName.IndexOf(EndOfItemCodeChar, StringComparison.Ordinal);
            return fileName.Substring(0, index);
        }

        static void Main(string[] args)
        {
            var fileName = args[0];
            if (string.IsNullOrEmpty(fileName)) return;
            if (!fileName.Contains("_")) return;
            var itemCode = GetItemCodeFromFileName(fileName);
            if (itemCode.Length == 0) return;
            LabelSet(itemCode,fileName);
        }
    }
}
