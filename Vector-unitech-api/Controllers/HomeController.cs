using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vector_unitech_api.Security;
using vector_unitech_application.AppServices;

namespace Vector_unitech_api.Controllers
{
    [ApiController]
    [Route( "v1/vector/[controller]" )]
    public class HomeController : Controller
    {
        private readonly IAppService _appService;
        private readonly TokenService _tokenService;
        public HomeController( IAppService appService, TokenService tokenService )
        {
            _appService = appService;
            _tokenService = tokenService;
        }


        [HttpGet]
        [Route( "GetAllEmails" )]
        [Authorize( "Admin" )]
        public async Task<IActionResult> GetAllEmails()
        {
            var response = await _appService.GetAllEmailsAsync();

            if ( response.Error ) return BadRequest( response.Message );


            if ( response.Result == null )
            {
                return NoContent();
            }

            return Ok( new { emails = response.Result } );

        }



        [HttpGet]
        [Route( "GetNamesGroupedByHour" )]
        [AllowAnonymous]
        public async Task<IActionResult> GetNamesGroupedByHourAsync()
        {
            var response = await _appService.GetNamesGroupedByHourAsync();

            if ( response.Error ) return BadRequest( response.Message );


            if ( response.Result == null )
            {
                return NoContent();
            }

            return Ok( new { resultado = response.Result } );

        }


        [HttpPost]
        [Route( "Authenticate" )]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate( [FromBody] UserLogin model )
        {
            var user = UserRepository.Get( model.Username, model.Password );

            if ( user == null )
                return BadRequest( new { message = "Usuário ou senha inválidos" } );

            var token = await _tokenService.GenerateTokenAsync( user );

            user.Password = "";

            return Ok( new
            {
                user = user,
                token = token.Result
            } );
        }

    }
}
