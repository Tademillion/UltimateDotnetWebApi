

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("/api/authentication")]
[ApiController]
[ServiceFilter(typeof(ValidationFilterAttribute))]
public class AuthenticationControllers:ControllerBase
{
 private readonly UserManager<User> _userManager;
 private readonly IMapper _mapper;
 private readonly ILogger<AuthenticationControllers> _logger;
 private readonly IAuthenticationManager _authManager;
 public AuthenticationControllers(UserManager<User> userManager, IMapper mapper, ILogger<AuthenticationControllers> logger, IAuthenticationManager authManager)
 {
    _userManager = userManager;
    _mapper = mapper;
    _logger = logger;
    _authManager = authManager;
 }  
    [HttpPost]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
    {
         var user = _mapper.Map<User>(userForRegistration);
        var result = await _userManager.CreateAsync(user, userForRegistration.Password);
        // the one argument is possible but hasshing password not included and in this case hashing is implemented by identity itself
        if(!result.Succeeded)
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }
        await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
        _logger.LogInformation($"User {user.UserName} registered successfully");
        return StatusCode(201);
    }
    //  login user
    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
    {
        if(!await  _authManager.ValidateUser(user) )
        {
            _logger.LogWarning($"Authentication failed for user {user.UserName}: User not found.");
            return Unauthorized(new { Message = "Authentication failed: Invalid username or password." });
        }

      return Ok(new { Token = await _authManager.CreateToken() });
    }
}