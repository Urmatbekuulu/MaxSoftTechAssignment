using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using MaxSoftTechAssignment.BLL.DTOs.SalesmanDtos;
using MaxSoftTechAssignment.DAL.Data;
using MaxSoftTechAssignment.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MaxSoftTechAssignment.WEB.Controllers;

public class SalesmanController:ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<SalesmanController> _logger;
    private readonly IMapper _mapper;

    public SalesmanController(ApplicationDbContext dbContext, UserManager<User> userManager
        ,RoleManager<Role> roleManager, ILogger<SalesmanController> logger, IMapper mapper)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet("/api/salesman/products")]
    [SwaggerOperation(
        Summary = "Get all products",
        Description = ""
    )]
    public async Task<IActionResult> GetListOfProducts()
    {
        //Not yet implemented
        
        return BadRequest("Not yet implemented");
    }


    [HttpGet("/api/salesman/information")]
    [SwaggerOperation(
        Summary = "Get information of current salesman",
        Description = "On technical assignment it is not written clearly"
    )]
    public async Task<ActionResult<SalesmanViewModel>> GetCurrentSalesmanInformationAsync()
    {
        var currentSalesman =await _dbContext.Users
            .FindAsync(
                User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)
            );
        
        if (currentSalesman is null) return BadRequest("Current user is not found");

        var response = _mapper.Map<SalesmanViewModel>(currentSalesman);

        return Ok(response);
    }

    [HttpGet("/api/salesman/saleproduct/{productId:int}/{quantity:int?}")]
    [SwaggerOperation(
        Summary = "Decrements the quantity of products, which means sold",
        Description = "Default sold quantity of product is 1"
    )]
    public async Task<IActionResult> DecrementQuantityOfProduct(int productId, int quantity = 1)
    { 
        
        return BadRequest("Not yet implemented");
    }

}