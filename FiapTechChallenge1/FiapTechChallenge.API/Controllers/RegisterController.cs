using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FiapTechChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {

        }

    }
}
