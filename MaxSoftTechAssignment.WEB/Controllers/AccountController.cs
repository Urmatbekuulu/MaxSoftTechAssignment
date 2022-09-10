using MaxSoftTechAssignment.BLL.DTOs.AccountDtos;
using MaxSoftTechAssignment.DAL.Entities;
using MaxSoftTechAssignment.DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MaxSoftTechAssignment.WEB.Controllers;

public class AccountController:ControllerBase
{
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtFactory _jwtFactory;
    private readonly ILogger<AccountController> _logger;

    public AccountController(SignInManager<User> signInManager, IJwtFactory jwtFactory, ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _jwtFactory = jwtFactory;
        _logger = logger;
    }
    
    [AllowAnonymous]
    [HttpPost("/api/signin")]
    [SwaggerOperation(
        Summary = "SignIn",
        Description = ""
    )]
    public async Task<ActionResult<SignInResponse>> SignIn([FromBody] SignIn request)
    {
        if (!ModelState.IsValid) return BadRequest();
                
        var user = await _signInManager.UserManager.FindByNameAsync(request.UserName);

        if (user == null) return BadRequest();

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

        if (!result.Succeeded) return BadRequest();

        var (jwtToken, expires) = await _jwtFactory.CreateTokenAsync(user.Id, user.Email);
        
        return new SignInResponse()
        {
            Token = jwtToken,
            ExpireDate = expires
        };

    }
}