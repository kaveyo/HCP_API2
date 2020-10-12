using System.Linq;
using System.Security.Claims;
using System.Web.Http;

using System.Data.SqlClient;

namespace HCP_API.Controllers
{
    public class ResourceController : ApiController
    {
        ClassDatabase obj = new ClassDatabase();
        
      
        [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/test/help")]
        public IHttpActionResult GetHelp(int id)
        {
            obj.conn.ConnectionString = obj.locate1;
            obj.conn.Open();
            SqlDataReader sdr;
            string be = "";
            SqlCommand cmd = new SqlCommand("SELECT explanation FROM help  where id = '" + id + "' ", obj.conn);

            SqlDataAdapter dataAdp = new SqlDataAdapter(cmd);

            using (sdr = cmd.ExecuteReader())
            {
                if (sdr.Read())
                {

                    be = (sdr["explanation"].ToString());

                }

            }

            return Ok("Result: " + be);
        }

        [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/test/dependent")]
        public IHttpActionResult GetDependent()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok("Hello: " + identity.Name);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        [Route("api/test/registered")]
        public IHttpActionResult GetRegistered(string phone_no)
        {
            obj.conn.ConnectionString = obj.locate1;
            obj.conn.Open();
            SqlDataReader sdr;
            string be = "";
            SqlCommand cmd = new SqlCommand("SELECT phone_number FROM registered  where phone_number = '" + phone_no + "' ", obj.conn);

            SqlDataAdapter dataAdp = new SqlDataAdapter(cmd);

            using (sdr = cmd.ExecuteReader())
            {
                if (sdr.Read())
                {

                    be = (sdr["phone_number"].ToString());

                }

            }
            if (be == "")
            {
                return Ok("0");
            }
            else { return Ok("1"); }
            //return Ok("Result: " + be);
        }

        [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/test/amount")]
        public IHttpActionResult GetAmount()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok("Hello: " + identity.Name);
        }
        //This resource is only For Admin and SuperAdmin role
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet]
        [Route("api/test/resource2")]
        public IHttpActionResult GetResource2()
        {
            var identity = (ClaimsIdentity)User.Identity;
           
            var UserName = identity.Name;

            return Ok("Hello " + UserName );
        }
        //This resource is only For SuperAdmin role
        [Authorize(Roles = " SuperAdmin")]
        [HttpGet]
        [Route("api/test/resource3")]
        public IHttpActionResult GetResource3()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            return Ok("Hello " + identity.Name + "Your Role(s) are: " + string.Join(",", roles.ToList()));
        }
    }
}
