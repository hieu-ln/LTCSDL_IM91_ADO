using System;
using System.Data.SqlClient;
using System.Data;

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
                /*
                sqlStr = "insert into Categories(CategoryName, [Description]) values('CNTT',N'Công Nghệ TT')";
                cmd.CommandText = sqlStr;
                cmd.ExecuteNonQuery();
                Console.WriteLine(" Thêm mới thành công !!!");
                
                sqlStr = "UPDATE Categories SET [CategoryName] = N'CNTT' WHERE CategoryID = 10";
                cmd.CommandText = sqlStr;
                cmd.ExecuteNonQuery();
                Console.WriteLine(" Cập nhật thành công !!!");
                */

                // Chạy STORED PROCEDURE CustOrderHist
                cmd.CommandText = "CustOrderHist";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;                
                cmd.Parameters.Add("@CustomerID", System.Data.SqlDbType.NChar).Value = "ALFKI";
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                Console.WriteLine(" Khach Hang voi ma la ALFKI, mua hang tai Northwind:::");
                if(ds.Tables.Count > 0)
                {
                    int i = 1;
                    foreach(DataRow row in ds.Tables[0].Rows)
                    {
                        string rStr = i.ToString()+"-  "+ row["ProductName"].ToString();
                        rStr = rStr + "\t" + row["Total"].ToString();
                        i = i + 1;
                        Console.WriteLine(rStr);
                    }
                }

                // Chạy STORED PROCEDURE c
                cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = "CustOrdersDetail";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@OrderID", System.Data.SqlDbType.Int).Value = 10254;
                da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                Console.WriteLine("\n\nChi tiet don hang 10254 :");
                Console.WriteLine("(Su dung STORE PROCEDURE)");
                if (ds.Tables.Count > 0)
                {
                    int i = 1;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string rStr = i.ToString() + "-  " + row["ProductName"].ToString();
                        rStr = rStr + "\t" + row["UnitPrice"].ToString();
                        rStr = rStr + "\t" + row["Quantity"].ToString();
                        rStr = rStr + "\t" + row["Discount"].ToString()+"%";
                        rStr = rStr + "\t" + row["ExtendedPrice"].ToString();
                        i = i + 1;
                        Console.WriteLine(rStr);
                    }
                }

                // Su dung cau lenh SQL Select
                sqlStr = "select * from [Order Details] where OrderID =10254";
                cmd.CommandText = sqlStr;
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                str = "";
                int j = 1;
                while (dr.Read())
                {
                    double price = double.Parse(dr["UnitPrice"].ToString());
                    double qty = double.Parse(dr["Quantity"].ToString());
                    double dis = double.Parse(dr["Discount"].ToString());
                    double amt = price * qty * (1 - dis);

                    str +=j.ToString()+"- "+ dr["ProductID"].ToString();
                    str +="\t"+ dr["UnitPrice"].ToString();
                    str += "\t" + dr["Quantity"].ToString();
                    str += "\t" + dr["Discount"].ToString();
                    str += "\t" + amt.ToString("0.0");
                    str += "\n";
                    j++;
                }
                Console.WriteLine("\n\nSU DUNG DataReader : ");
                Console.Write(str);
                dr.Close();

                /// SQLCommandBuilder
                /*
                da = new SqlDataAdapter("Select * from Categories", cnn);
                ds = new DataSet();
                da.Fill(ds);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["CategoryName"].ToString() == "CNTT")
                    {
                        row.Delete();
                        break;
                    }
                }
                SqlCommandBuilder cb = new SqlCommandBuilder(da);
                da.Update(ds);
                Console.WriteLine("\n\nXoa thanh cong CNTT !!");
                */

                string s = "w";

                /// DataTable
                da = new SqlDataAdapter("Select * from Categories", cnn);
                ds = new DataSet();
                da.Fill(ds);
                if(ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow[] rows = dt.Select("[Description] like '%"+s+"%'", "CategoryID desc");
                    str = "";
                    int stt = 1;
                    foreach(DataRow row in rows)
                    {
                        str +=stt.ToString() + "- " + row["CategoryID"].ToString();
                        str += "\t" + row["CategoryName"].ToString();
                        str += "\t" + row["Description"].ToString() + "\n";
                        stt++;
                    }
                    Console.WriteLine("\n\nDescription co chu " + s + " la:");
                    Console.WriteLine(str);
                }

                // SQLParameter
                SqlParameter paraDesc = new SqlParameter();
                paraDesc.SqlDbType = SqlDbType.NVarChar;
                paraDesc.ParameterName = "@des";
                paraDesc.Value = s;

                sqlStr = "Select * from Categories WHERE [Description] like '%' + @des +'%'";
                cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = sqlStr;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(paraDesc);
                reader = cmd.ExecuteReader();
                str = "";
                j = 1;
                while (reader.Read())
                {
                    str += j.ToString() + " - " + reader["CategoryID"].ToString();
                    str += "\t" + reader["CategoryName"].ToString();
                    str += "\t" + reader["Description"].ToString() + "\n";
                    j++;
                }
                reader.Close();
                Console.WriteLine("\n\nDescription co chu " + s + " la:");
                Console.WriteLine("(Xai SQLParameter)");
                Console.WriteLine(str);



                // Chạy STORED PROCEDURE với param output
                cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandText = "PhepCong";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@a", System.Data.SqlDbType.Int).Value = 10;
                cmd.Parameters.Add("@b", System.Data.SqlDbType.Int).Value = 20;
                cmd.Parameters.Add("@c", SqlDbType.Int);
                cmd.Parameters["@c"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int kq = Convert.ToInt16(cmd.Parameters["@c"].Value);
                Console.WriteLine("\n\n Su dung PARAM OUTPUT:");
                Console.WriteLine("\t10 + 20 = " + kq.ToString());
            }
            catch (Exception e)
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
