using Entities;
using Microsoft.Data.SqlClient;
using repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class RatingRepository : IRatingRepository
    {
        MyShopContext _myShopContext;
        public RatingRepository(MyShopContext myShopContext)
        {
            _myShopContext = myShopContext;
        }
        int rowAffected = 0;
        public Rating Post(Rating rating)
        {
            string connaction = "data source=DESKTOP-E0FAPSB\\SQLEXPRESS;Database=LeahShopDB;Trusted_Connection=True;TrustServerCertificate=True;";
            string query = "INSERT INTO Rating (HOST,METHOD,PATH,REFERER,USER_AGENT,Record_Date)" +
                "VALUES (@Host,@Method,@Path,@REFERER,@USER_AGENT,@Record_Date)";
            using (SqlConnection cn = new SqlConnection(connaction))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.Add("@Host", SqlDbType.NVarChar,50).Value = rating.Host;
                cmd.Parameters.Add("@Method", SqlDbType.NChar, 10).Value = rating.Method;
                cmd.Parameters.Add("@Path", SqlDbType.NVarChar, 50).Value = rating.Path;
                cmd.Parameters.Add("@REFERER", SqlDbType.NVarChar, 100).Value = rating.Referer ?? (object)DBNull.Value;
                cmd.Parameters.Add("@USER_AGENT", SqlDbType.NVarChar).Value = rating.UserAgent;
                cmd.Parameters.Add("@Record_Date", SqlDbType.DateTime).Value = rating.RecordDate;
                //cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = null;
                cn.Open();
                rowAffected = cmd.ExecuteNonQuery();
                cn.Close();

                //await _myShopContext.Ratings.AddAsync(rating);
                //await _myShopContext.SaveChangesAsync();

                return rating;
            }


        }
    }

}