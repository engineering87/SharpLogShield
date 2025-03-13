using Microsoft.AspNetCore.Mvc;

namespace SharpLogShield.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        // Iniezione delle dipendenze tramite costruttore
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        // GET api/test
        [HttpGet]
        public IActionResult Get()
        {
            // Simuliamo delle informazioni sensibili
            string sensitiveData = "User email: john.doe@example.com and credit card: 1234-5678-9876-5432";

            // Logghiamo un messaggio che contiene dati sensibili
            _logger.LogInformation("Test GET request. Sensitive data: {SensitiveData}", sensitiveData);

            return Ok("GET request received and sensitive data logged!");
        }

        // POST api/test
        [HttpPost]
        public IActionResult Post([FromBody] string sensitiveData)
        {
            // Simuliamo il log di una richiesta POST con dati sensibili
            _logger.LogInformation("Test POST request. Sensitive data: {SensitiveData}", sensitiveData);

            return Ok($"POST request received. Data: {sensitiveData}");
        }

        // PUT api/test/{id}
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string sensitiveData)
        {
            // Simuliamo un aggiornamento con dati sensibili
            _logger.LogInformation("Test PUT request. Updating data for ID {Id}. Sensitive data: {SensitiveData}", id, sensitiveData);

            return Ok($"PUT request received. Data for ID {id} updated.");
        }

        // DELETE api/test/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Simuliamo un'operazione di cancellazione con log
            _logger.LogInformation("Test DELETE request. Deleting data for ID {Id}", id);

            return Ok($"DELETE request received. Data for ID {id} deleted.");
        }
    }
}
