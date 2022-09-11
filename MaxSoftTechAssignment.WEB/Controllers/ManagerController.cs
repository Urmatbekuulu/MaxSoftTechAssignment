using AutoMapper;
using MaxSoftTechAssignment.BLL.DTOs.ManageShopDtos;
using MaxSoftTechAssignment.DAL.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    
    [HttpGet("/api/manager/products/{shopId:int}")]
    [SwaggerOperation(
        Summary = "Get products from manager's shops",
        Description = ""
    )]
    public async Task<ActionResult<IEnumerable<ProductsListViewModel>>> GetProductsListAsync(int shopId)
    {
        var products = await _dbContext.Products.Where(p => p.ShopId == shopId).ToListAsync();
        var response = _mapper.Map<IEnumerable<ProductsListViewModel>>(products);

        if (response is null) return BadRequest();

        return Ok(response);
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