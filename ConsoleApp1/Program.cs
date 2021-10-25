using System;
using System.Data.SqlClient;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("XIn Chao lop IM91 - LTCSDL !!!");
            string cnStr = "Server = localhost; Database = Northwind; User id = sa; password = Password123;";
            SqlConnection cnn = new SqlConnection(cnStr);
            try
            {
                
                cnn.Open();
                Console.WriteLine("Ket noi thanh cong toi DB !!!");
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                // Cau 1 - du lieu dang bảng
                string sqlStr = "select CategoryID, CategoryName from Categories";
                cmd.CommandText = sqlStr;
                cmd.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();
                string str = "";
                while(reader.Read())
                {
                    str +="\t"+ reader["CategoryID"] + " - " + reader["CategoryName"] + "\n";
                }
                reader.Close();
                Console.Write(str);

                // Cau 2 - Du lieu dang đơn - scalar
                sqlStr = "select count(*) as Total from Categories;";
                cmd.CommandText = sqlStr;
                int total = (int)cmd.ExecuteScalar();
                Console.WriteLine(" --> Total of Categories: " + total);

                // Cau 3 - Khong co tra ve du lieu
                sqlStr = "insert into Categories(CategoryName, [Description]) values('CNTT',N'Công Nghệ TT')";
                cmd.CommandText = sqlStr;
                cmd.ExecuteNonQuery();
                Console.WriteLine(" Thêm mới thành công !!!");
            }
            catch(Exception e)
            {
                Console.WriteLine("Ket noi khong thanh cong !!!");
                Console.WriteLine("Loi:" + e.Message);
            }
            if (cnn.State == System.Data.ConnectionState.Open)
            {
                cnn.Close();
                Console.WriteLine("Ket noi da ngat !!!");
            }
        }
    }
}
