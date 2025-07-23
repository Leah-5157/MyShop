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
using Microsoft.Extensions.Configuration;

namespace Repositories
{
    public class RatingRepository : IRatingRepository
    {
        MyShopContext _myShopContext;
        private readonly string _connectionString;
        public RatingRepository(MyShopContext myShopContext, IConfiguration configuration)
        {
            _myShopContext = myShopContext;
            _connectionString = configuration.GetConnectionString("home");
        }
        int rowAffected = 0;
        public Rating Post(Rating rating)
        {
            string query = "INSERT INTO Rating (HOST,METHOD,PATH,REFERER,USER_AGENT,Record_Date)" +
                "VALUES (@Host,@Method,@Path,@REFERER,@USER_AGENT,@Record_Date)";
            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.Add("@Host", SqlDbType.NVarChar,50).Value = rating.Host;
                cmd.Parameters.Add("@Method", SqlDbType.NChar, 10).Value = rating.Method;
                cmd.Parameters.Add("@Path", SqlDbType.NVarChar, 50).Value = rating.Path;
                cmd.Parameters.Add("@REFERER", SqlDbType.NVarChar, 100).Value = string.IsNullOrEmpty(rating.Referer) ? string.Empty : rating.Referer;
                cmd.Parameters.Add("@USER_AGENT", SqlDbType.NVarChar).Value = rating.UserAgent;
                cmd.Parameters.Add("@Record_Date", SqlDbType.DateTime).Value = rating.RecordDate;
                cn.Open();
                rowAffected = cmd.ExecuteNonQuery();
                cn.Close();
                return rating;
            }
        }
    }
}