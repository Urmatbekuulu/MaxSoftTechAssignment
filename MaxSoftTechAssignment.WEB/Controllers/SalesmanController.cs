using System.IdentityModel.Tokens.Jwt;
using System.Transactions;
using AutoMapper;
using MaxSoftTechAssignment.BLL.DTOs.SalesmanDtos;
using MaxSoftTechAssignment.DAL;
using MaxSoftTechAssignment.DAL.Data;
using MaxSoftTechAssignment.WEB.Configurations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MaxSoftTechAssignment.WEB.Controllers;


public class SalesmanController:ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<SalesmanController> _logger;
    private readonly IMapper _mapper;

    public SalesmanController(ApplicationDbContext dbContext
        ,ILogger<SalesmanController> logger, IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }
    
    [JwtAuthorize]
    [HttpGet("/api/salesman/information")]
    [SwaggerOperation(
        Summary = "Get information of current salesman",
        Description = "On technical assignment it is not written clearly"
    )]
    public async Task<ActionResult<SalesmanViewModel>> GetCurrentSalesmanInformationAsync()
    {
        if (!User.Identity.IsAuthenticated) return BadRequest("User is not authenticated");
     
        var currentSalesman =await _dbContext.Users
            .FindAsync(
                User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value
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
        using (var scope = new TransactionScope(
                   TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.RepeatableRead }))
        {
            try
            {
                var product = await _dbContext.Products.FindAsync(productId);
            
                if (product is null || product.Quantity < quantity)
                    return BadRequest(
                        "Transaction is impossible, product is not found, or quantity of product is not enough");

                product.Quantity -= quantity;

                _dbContext.SaveChanges();

                scope.Complete();
            }
            catch (Exception)
            {
                return BadRequest("Transaction is not possible");
            }
        }
        
        return Ok();
    }

}