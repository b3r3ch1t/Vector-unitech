using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using vector_unitech_application.AppServices;

namespace Vector_unitech_api.Controllers
{
    [ApiController]
    [Route( "v1/vector/[controller]" )]
    public class HomeController : Controller
    {
        private readonly IAppService _appService;

        public HomeController( IAppService appService )
        {
            _appService = appService;
        }


        [HttpGet]
        [Route( "GetAllEmails" )]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllEmails()
        {
            var response = await _appService.GetAllEmailsAsync();

            if ( response.Error ) return BadRequest( response.Message );


            if ( response.Result == null )
            {
                return NoContent();
            }

            return Ok( response.Result );

        }



        [HttpGet]
        [Route( "GetNamesGroupedByHourAsync" )]
        [Authorize( "Admin" )]
        public async Task<IActionResult> GetNamesGroupedByHourAsync()
        {
            var response = await _appService.GetNamesGroupedByHourAsync();

            if ( response.Error ) return BadRequest( response.Message );


            if ( response.Result == null )
            {
                return NoContent();
            }

            return Ok( response.Result );

        }
    }
}
