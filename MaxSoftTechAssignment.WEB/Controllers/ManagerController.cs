using AutoMapper;
using MaxSoftTechAssignment.DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MaxSoftTechAssignment.WEB.Controllers;

public class ManagerController:ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ManagerController> _logger;
    private readonly IMapper _mapper;

    public ManagerController(ApplicationDbContext dbContext, ILogger<ManagerController> logger, IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }
    
    [HttpGet("/api/manager/products")]
    [SwaggerOperation(
        Summary = "Get products from manager's shops",
        Description = ""
    )]
    public async Task<IActionResult> GetProductsAsync()
    {
        //Not yet implemented

        return BadRequest("Not yet implemented");
    }
    
    [HttpGet("/api/shop/information")]
    [SwaggerOperation(
        Summary = "Get detailed information about shop and workers",
        Description = ""
    )]
    public async Task<IActionResult> ShopInformationAsync()
    {
        //Not yet implemented

        return BadRequest("Not yet implemented");
    }


    [HttpPost("/api/shop/product/create")]
    [SwaggerOperation(
        Summary = "Creates and adds new product to shop",
        Description = ""
    )]
    public async Task<IActionResult> CreateProductAsync()
    {

        return BadRequest("Not yet implemented");
    }

    [HttpDelete("/api/shop/product/{id:int}")]
    [SwaggerOperation(
        Summary = "Deletes product",
        Description = ""
    )]
    public async Task<IActionResult> DeleteProductAsync(int id)
    {
        return BadRequest("Not yet implemented");
    }

    [HttpPut("/api/shop/product/update")]
    [SwaggerOperation(
        Summary = "Updates product information",
        Description = ""
    )]
    public async Task<IActionResult> UpdateProductAsync()
    {

        return BadRequest("Not yet implemented");
    }

    [HttpPost("/api/shop/salesman/create")]
    [SwaggerOperation(
        Summary = "Creates salesman for the shop",
        Description = ""
    )]
    public async Task<IActionResult> CreateSalesmanAsync()
    {
        return BadRequest("Not implemented yet");
    }

    [HttpDelete("/api/shop/salesman/delete/{id}")]
    [SwaggerOperation(
        Summary = "Deletes salesman from shop",
        Description = ""
    )]
    public async Task<IActionResult> DeleteSalesmanAsync(string id)
    {
        return (BadRequest("Not yet implemented"));
    }


}