using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MuniTrack_API.Contollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        //Se utiliza la inyección de dependencias con IConfiguration (para acceder al
        //archivo appsettings.json) y con IUserService (para validar a los usuarios).
        private readonly IConfiguration _config;
        private readonly IOperatorService _operatorService;

        public AuthenticationController(IConfiguration config, IOperatorService operatorService)
        {
            _config = config;
            this._operatorService = operatorService;

        }
        //Este método maneja la ruta POST api/authentication/authenticate, que es donde se
        //envían las credenciales para autenticarse.
        [HttpPost("authenticate")]
        public IActionResult Autenticar(AuthenticationDTO authenticationDto)
        {
            //Paso 1: Validamos las credenciales
            var oper = _operatorService.ValidateOper(authenticationDto); //Lo primero que hacemos es llamar a una función que valide los parámetros que enviamos.

            if (oper is null) //Si el la función de arriba no devuelve nada es porque los datos son incorrectos, por lo que devolvemos un Unauthorized (un status code 401).
                return Unauthorized();

            //Paso 2: Crear el token
            var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Authentication:SecretForKey"])); //Traemos la SecretKey del Json. agregar antes: using Microsoft.IdentityModel.Tokens;

            var credentials = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

            //Los claims son datos en clave->valor que nos permite guardar data del usuario.
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", oper.NLegajo.ToString())); //"sub" es una key estándar que significa unique user identifier, es decir, si mandamos el id del usuario por convención lo hacemos con la key "sub".
            claimsForToken.Add(new Claim("given_name", oper.Name)); //Lo mismo para given_name y family_name, son las convenciones para nombre y apellido. Ustedes pueden usar lo que quieran, pero si alguien que no conoce la app
            claimsForToken.Add(new Claim("family_name", oper.LastName));
            claimsForToken.Add(new Claim("role", oper.Position.ToString())); //quiere usar la API por lo general lo que espera es que se estén usando estas keys.
            
            var jwtSecurityToken = new JwtSecurityToken( //agregar using System.IdentityModel.Tokens.Jwt; Acá es donde se crea el token con toda la data que le pasamos antes.
              _config["Authentication:Issuer"],
              _config["Authentication:Audience"],
              claimsForToken,
              DateTime.UtcNow,
              DateTime.UtcNow.AddHours(1),
              credentials);

            var tokenToReturn = new JwtSecurityTokenHandler() //Pasamos el token a string
                .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }
    }
}
