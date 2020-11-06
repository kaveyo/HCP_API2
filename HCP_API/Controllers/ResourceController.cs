using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Data.Entity;
using System.Data.SqlClient;
using HCP_API.Models;
using System.Net.Http;
using System.Net;
using System;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Web.Http.Results;
using Newtonsoft.Json;

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
        [Route("api/test/help_fcp")]
        public IHttpActionResult GetHelp_fcp(int id)
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
        public async Task<string> GetValidateAsync( string id , DateTime dob , string surname)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://10.170.3.40:8084/");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //GET Method
                HttpResponseMessage response = await client.GetAsync("v2/rg/?name='"+id+"'");
                if (response.IsSuccessStatusCode)
                {
                    Rg_model department = await response.Content.ReadAsAsync<Rg_model>();
                    if (department.PersonNo.ToLower() == id.ToLower() && Convert.ToDateTime(department.DateOfBirth) == dob && department.Surname.ToLower() == surname.ToLower() ) {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                   // return JsonConvert.SerializeObject(department);
                }
                else
                {
                   // Console.WriteLine("Internal server Error");
                    return "Internal server Error";
                }


            }
        }

        public async Task<string> GetValidateAsync_fcp(string id, DateTime dob, string surname)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://10.170.3.40:8084/");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //GET Method
                HttpResponseMessage response = await client.GetAsync("v2/rg/?name='" + id + "'");
                if (response.IsSuccessStatusCode)
                {
                    Rg_model department = await response.Content.ReadAsAsync<Rg_model>();
                    if (department.PersonNo.ToLower() == id.ToLower() && Convert.ToDateTime(department.DateOfBirth) == dob && department.Surname.ToLower() == surname.ToLower())
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                    // return JsonConvert.SerializeObject(department);
                }
                else
                {
                    // Console.WriteLine("Internal server Error");
                    return "Internal server Error";
                }


            }
        }
        [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/test/principals")]
        public registered Getprincipals(string national_id)
        {
            try
            {
                using (hcp_apiEntities6 entities = new hcp_apiEntities6())
                {
                    return entities.registereds.FirstOrDefault(e => e.national_id == national_id);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("api/test/dependent")]
        public dependant GetDependent(string national_id)
        {
            try
            {
                using (hcp_apiEntities6 entities = new hcp_apiEntities6())
                {
                    return entities.dependants.FirstOrDefault(x => x.national_id == national_id);
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        [Route("api/test/check_registered")]
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
            else { return Ok("1");
            }
           
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [Route("api/test/register")]
        public async Task<HttpResponseMessage> PostRegisterAsync([FromBody] registered register_obj)
        {
            try
            {
                using(hcp_apiEntities6 entities = new hcp_apiEntities6())
                {
                    entities.registereds.Add(register_obj);

                    if (await GetValidateAsync(register_obj.national_id, Convert.ToDateTime(register_obj.dob), register_obj.surname) == "1")
                    {

                        entities.SaveChanges();

                        var message = Request.CreateResponse(HttpStatusCode.Created, register_obj);
                        message.Headers.Location = new Uri(Request.RequestUri + register_obj.national_id.ToString());
                        return message;
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Wrong user");
                    }
                }
            }
            catch (System.Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
               // throw;
            }
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [Route("api/test/register_fcp")]
        public async Task<HttpResponseMessage> PostRegisterAsync_fcp([FromBody] registered_fcp register_obj_fcp)
        {
            try
            {
                using (hcp_apiEntities6 entities = new hcp_apiEntities6())
                {
                    entities.registered_fcp.Add(register_obj_fcp);

                    if (await GetValidateAsync(register_obj_fcp.national_id, Convert.ToDateTime(register_obj_fcp.dob), register_obj_fcp.surname) == "1")
                    {

                        entities.SaveChanges();

                        var message = Request.CreateResponse(HttpStatusCode.Created, register_obj_fcp);
                        message.Headers.Location = new Uri(Request.RequestUri + register_obj_fcp.national_id.ToString());
                        return message;
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Wrong user");
                    }
                }
            }
            catch (System.Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                // throw;
            }
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [Route("api/test/register_dependent")]
        public async Task<HttpResponseMessage> PostRegister_dependentAsync([FromBody] dependant dependant_obj)
        {
          
            try
            {
                using (hcp_apiEntities6 entities = new hcp_apiEntities6())
                {
                    entities.dependants.Add(dependant_obj);


                    if (await GetValidateAsync(dependant_obj.national_id, Convert.ToDateTime(dependant_obj.dob), dependant_obj.surname) == "1")
                    {
                       
                        entities.SaveChanges();

                        var message = Request.CreateResponse(HttpStatusCode.Created, dependant_obj);
                        message.Headers.Location = new Uri(Request.RequestUri + dependant_obj.national_id.ToString());
                        return message;
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Wrong user");
                    }
                }
            }
            catch (System.Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                // throw;
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [Route("api/test/register_dependent_fcp")]
        public async Task<HttpResponseMessage> PostRegister_dependentAsync_fcp([FromBody] dependant_fcp dependant_obj_fcp)
        {

            try
            {
                using (hcp_apiEntities6 entities = new hcp_apiEntities6())
                {
                    entities.dependant_fcp.Add(dependant_obj_fcp);


                    if (await GetValidateAsync(dependant_obj_fcp.national_id, Convert.ToDateTime(dependant_obj_fcp.dob), dependant_obj_fcp.surname) == "1")
                    {

                        entities.SaveChanges();

                        var message = Request.CreateResponse(HttpStatusCode.Created, dependant_obj_fcp);
                        message.Headers.Location = new Uri(Request.RequestUri + dependant_obj_fcp.national_id.ToString());
                        return message;
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Wrong user");
                    }
                }
            }
            catch (System.Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                // throw;
            }
        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        [Route("api/test/payment")]
        public IHttpActionResult GetPayment(string phone_no)
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

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [Route("api/test/register_church")]
        public IHttpActionResult PostRegister_ChurchAsync([FromBody] registered regi_obj)
        {
            try
            {
                using (hcp_apiEntities6 entities = new hcp_apiEntities6())
                {
                    using (SqlConnection sqlCon = new SqlConnection(obj.locate1))
                    {
                        sqlCon.Open();
                        string query = "UPDATE registered set church = '"+regi_obj.church+ "' where phone_number = '"+regi_obj.phone_number.Trim()+"'";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.ExecuteNonQuery();
                       
                    
                    }
                    var message = Request.CreateResponse(HttpStatusCode.Created, regi_obj);
                    message.Headers.Location = new Uri(Request.RequestUri + regi_obj.church.ToString());
                    return Ok("Registered");
                }
            }
            catch (System.Exception ex)
            {
                return Ok(ex.ToString());
                // throw;
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [Route("api/test/register_church_fcp")]
        public IHttpActionResult PostRegister_ChurchAsync_fcp([FromBody] registered_fcp regi_obj_fcp)
        {
            try
            {
                using (hcp_apiEntities6 entities = new hcp_apiEntities6())
                {
                    using (SqlConnection sqlCon = new SqlConnection(obj.locate1))
                    {
                        sqlCon.Open();
                        string query = "UPDATE registered set church = '" + regi_obj_fcp.church + "' where phone_number = '" + regi_obj_fcp.phone_number.Trim() + "'";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.ExecuteNonQuery();


                    }
                    var message = Request.CreateResponse(HttpStatusCode.Created, regi_obj_fcp);
                    message.Headers.Location = new Uri(Request.RequestUri + regi_obj_fcp.church.ToString());
                    return Ok("Registered");
                }
            }
            catch (System.Exception ex)
            {
                return Ok(ex.ToString());
                // throw;
            }
        }
    }
}
