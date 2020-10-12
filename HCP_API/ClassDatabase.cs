using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace HCP_API
{
    public class ClassDatabase
    {
        public SqlConnection conn = new SqlConnection();

        public SqlCommand cmd = new SqlCommand();
        public string locate1 = @"Data Source=10.180.5.14;Initial Catalog=hcp_api;User ID =sa; Password=Password123;";
    }
}