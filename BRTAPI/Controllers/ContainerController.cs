using BRTAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Data.SqlClient;

namespace BRTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerController : ControllerBase
    {
        private readonly ApiDbContext _context;

        private readonly IConfiguration _configuration;


        public ContainerController(ApiDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("GetContainerNos")]
        public async Task<ActionResult<List<string>>> GetContainerNos()
        {
            List<string> containerNos = new List<string>();
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT distinct   [ContainerNo] FROM [ContainerIndex] ci where  ci.IsEmptyGateOut = 0 and ci.IsDestuffed = 0 and ci.IsGateIn = 1 and ci.IsDeleted = 0     union    select distinct  cy.ContainerNo [ContainerIndexId] from CYContainer cy where cy.IsGateIn = 1 and cy.IsDelivered = 0 and cy.IsGateOut = 0 and cy.IsDeleted = 0";
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {

                            containerNos.Add(reader["ContainerNo"].ToString());
                        }
                    }
                }
            }
            return containerNos;
        }


        [HttpGet("GetContainerSize")]
        public async Task<ActionResult <int>> GetContainerSize(string containerNo)
        {
            int Size = 0;
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
            DECLARE @ConatinerNo varchar(100)
            SET @ConatinerNo = @ContainerNo
            SELECT TOP 1  Size 
            FROM [ContainerIndex] ci 
            WHERE ci.ContainerNo = @ConatinerNo 
            AND ci.IsEmptyGateOut = 0 
            AND ci.IsDestuffed = 0 
            AND ci.IsGateIn = 1 
            AND ci.IsDeleted = 0 
            UNION 
            SELECT TOP 1 Size 
            FROM CYContainer cy 
            WHERE cy.ContainerNo = @ConatinerNo 
            AND cy.IsGateIn = 1 
            AND cy.IsDelivered = 0 
            AND cy.IsGateOut = 0 
            AND cy.IsDeleted = 0";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ContainerNo", containerNo);
                    await connection.OpenAsync();
                    Size = (int)command.ExecuteScalar();
                }
            }
            return Size;
        }


    }
}
