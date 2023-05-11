using ArmyExe2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Linq;

namespace ArmyExe2.Controllers
        {
            [Route("api/[controller]")]
            [ApiController]
            public class CoronaDetailController : ControllerBase
            {
                private readonly IConfiguration _configuration;
                public CoronaDetailController(IConfiguration configuration)
                {
                    _configuration = configuration;

                }

              //function that returns a list of all the pepole that did a corona virus Vaccin
            [HttpGet]
            [Route("GetAllCoronaDetails")]
            public Response GetAllCoronaDetails()
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("userConnection").ToString());
                Response response = new Response();
                DAL dal = new DAL();
                response = dal.GetAllCoronaDetails(connection);
                return response;
            }

            //function that the number of pepole that did not do a corona virus Vaccin
            [HttpGet]
            [Route("GetNotVaccinatedCount")]
            public Response GetNotVaccinatedCount()
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("userConnection").ToString());
                Response response = new Response();
                DAL dal = new DAL();
                response =  dal.GetNotVaccinatedCount(connection);
                return response;
            }

            //function that you can send her id and i finds the corona vaccin details of that person
            [HttpGet]
            [Route("GetCoronaDetailsById/{id}")]
            public Response GetCoronaDetailsById(string id)
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("userConnection").ToString());
                Response response = new Response();
                DAL dal = new DAL();
                bool b = dal.IsValidID(id);
                if (!b)
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No valid id";
                    response.user = null;
                }
            response = dal.GetCoronaDetailsById(connection,id);
                return response;
            }

            //add function that adds to a user in the system a vaccin date and a vaccin manufactured
            [HttpPost]
            [Route("addCoronaDetails")]
            public Response addCoronaDetails(CoronaDetails coronaDetails)
            {
                Response response = new Response();
                DAL dal = new DAL();
                var errors = dal.ValidateCoronaDetails(coronaDetails);
                if (errors.Count == 0)
                {
                    SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("userConnection").ToString());
                    response = dal.addCoronaDetails(connection, coronaDetails);
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = string.Join(", ", errors);
                    response.user = null;
                }
                return response;
            }

       
    }
    }

