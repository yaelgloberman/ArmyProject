using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using ArmyExe2.Models;
using Microsoft.Data.SqlClient;

namespace ArmyExe2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsernameController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UsernameController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        //function that returns a list of the users
        [HttpGet]
        [Route("GetAllUsers")]
        public Response GetAllUsers()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("userConnection").ToString());
            Response response = new Response();
            DAL dal = new DAL();
            response = dal.GetAllUsers(connection);
            return response;
        }

        //function that returns a list of the days and the number of pepole that sick in that day
        [HttpGet]
        [Route("GetSickByDays")]
        public Response GetSickByDays()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("userConnection").ToString());
            Response response = new Response();
            DAL dal = new DAL();
            response = dal.GetSickByDays(connection);
            return response;
        }

        //function returns the user by a given id
        [HttpGet]
        [Route("GetUserById/{id}")]
        public Response GetUserById(string id)
        {
            Response response = new Response();
            DAL dal = new DAL();
            bool b = dal.IsValidID(id);
            if (!b)
            {
                response.StatusCode = 100;
                response.StatusMessage = "No valid id";
                response.user = null;
            }
            else
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("userConnection").ToString());
                response = dal.GetUserById(connection, id);
            }
          
            return response;
        }

        //add function that adds user to the system
        [HttpPost]
        [Route("addUser")]
        public Response addUser(Username user)
        { 
            Response response = new Response();
            DAL dal = new DAL();
            var errors=dal.ValidateUsername(user);
            if (errors.Count==0)
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("userConnection").ToString());
                response = dal.addUser(connection, user);
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = string.Join(", ", errors);
                response.user = null;
            }
            return response;  

        }

        //add function that add Positive date
        [HttpPost]
        [Route("addPositive")]
        public Response addPositive(string id, DateTime date)
        {
            Response response = new Response();
            DAL dal = new DAL();
            if (date <= DateTime.Today)
            {
                bool b = dal.IsValidID(id);
                if (!b)
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No valid id";
                    response.user = null;
                }
                else
                {
                    SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("userConnection").ToString());
                    response = dal.addPositive(connection, id, date);
                }
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "corona positive test cant be in the future";
                response.user = null;
            }
            return response;

        }

        //add function that add recover date
        [HttpPost]
        [Route("addRecover")]
        public Response addRecover(string id, DateTime date)
        {
            Response response = new Response();
            DAL dal = new DAL();
            if (date <= DateTime.Today)
            {
                bool b = dal.IsValidID(id);
                if (!b)
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No valid id";
                    response.user = null;
                }
                else
                {
                    SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("userConnection").ToString());
                    response = dal.addRecover(connection, id, date);
                }

            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "corona recover cant be a date in the future";
                response.user = null;
            }
            return response;

        }
    }
}