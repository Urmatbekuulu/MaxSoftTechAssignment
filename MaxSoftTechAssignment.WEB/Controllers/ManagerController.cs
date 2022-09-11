using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using MaxSoftTechAssignment.BLL.DTOs.ManageShopDtos;
using MaxSoftTechAssignment.BLL.DTOs.SalesmanDtos;
using MaxSoftTechAssignment.DAL;
using MaxSoftTechAssignment.DAL.Data;
using MaxSoftTechAssignment.DAL.Entities;
using MaxSoftTechAssignment.WEB.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace MaxSoftTechAssignment.WEB.Controllers;


public class ManagerController:ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<ManagerController> _logger;
    private readonly IMapper _mapper;
    private UserManager<User> _userManager;

    public ManagerController(ApplicationDbContext dbContext, ILogger<ManagerController> logger, IMapper mapper, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }


    
    [HttpGet("/api/products/{shopId:int}")]
    [SwaggerOperation(
        Summary = "Get shop's list of products.",
        Description = ""
    )]
    public async Task<ActionResult<IEnumerable<ProductsListViewModel>>> GetProductsListAsync(int shopId)
    {
        var products = await _dbContext.Products.Where(p => p.ShopId == shopId).ToListAsync();
        var response = _mapper.Map<IEnumerable<ProductsListViewModel>>(products);

        if (response is null) return BadRequest();

        return Ok(response);
    }

    [JwtAuthorize]

    [HttpGet("/api/shop/information/{id:int}")]
    [SwaggerOperation(
        Summary = "Get detailed information about shop and workers",
        Description = ""
    )]
    public async Task<ActionResult<ShopInformationViewModel>> ShopInformationAsync(int id)
    {
        if (!User.Identity.IsAuthenticated) return Unauthorized();

        var currentUser =
            await _dbContext.Users.FindAsync(
                User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?
                    .Value
                );
        
        var shop = await _dbContext.Shops
            .Include(s => s.Manager)
            .Include(c => c.Salesman)
            .FirstOrDefaultAsync(s => s.Id == id);
        
        if (shop == null || currentUser ==null) return BadRequest("Shop is not found, or user is not found");
        
        if (shop.ManagerId != currentUser?.Id) return BadRequest("This not your shop. Baby do our own job:)");


        var shopInformationResponse = new ShopInformationViewModel()
        {
            Name = shop.Name,
            ManagerUsername = shop.Manager?.UserName,
            SalesmanUsername = shop.Salesman?.UserName,
            ProductTypesCount = await _dbContext.Products.CountAsync(p => p.ShopId == shop.Id)
        };
        return Ok(shopInformationResponse);
    }


    [HttpPost("/api/shop/product/create")]
    [SwaggerOperation(
        Summary = "Creates and adds new product to shop",
        Description = ""
    )]
    public async Task<IActionResult> CreateProductAsync([FromBody] ProductViewModel request)
    {
        if (!ModelState.IsValid) return BadRequest("Model is not valid");

        if (!await _dbContext.Shops.AnyAsync(s => s.Id == request.ShopId)) return BadRequest("Shop is not found");

        var product = _mapper.Map<Product>(request);

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        
        return Ok();
    }

    [HttpDelete("/api/shop/product/{id:int}")]
    [SwaggerOperation(
        Summary = "Deletes product",
        Description = ""
    )]
    public async Task<IActionResult> DeleteProductAsync(int id)
    {
        var product =await _dbContext.Products.FindAsync(id);
        if (product != null) _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("/api/shop/product/update")]
    [SwaggerOperation(
        Summary = "Updates product information",
        Description = ""
    )]
    public async Task<IActionResult> UpdateProductAsync([FromBody] ProductViewModel? productViewModel)
    {
        if (!ModelState.IsValid) return BadRequest("Model is not valid");

        var product = await _dbContext.Products.FindAsync(productViewModel.Id);

        if (product is null) return NotFound();

        product.Quantity=productViewModel.Quantity;
        product.Price = productViewModel.Price;

        await _dbContext.SaveChangesAsync();
        
        return Ok();
    }

    [HttpPost("/api/shop/salesman/create")]
    [SwaggerOperation(
        Summary = "Creates salesman for the shop",
        Description = ""
    )]
    public async Task<IActionResult> CreateSalesmanAsync([FromBody] SalesmanViewModel request)
    {
        if (!ModelState.IsValid) return BadRequest();

        var shop =await _dbContext.Shops.FindAsync(request.ShopId);

        if (shop is null) return NotFound("Shop is not found");

        var user = _mapper.Map<User>(request);

        if (await _userManager.Users.AnyAsync(u=>u.Email==user.Email || u.UserName==user.UserName)) 
            return BadRequest("Username or email is registered");

        var result = await _userManager.CreateAsync(user,request.Password);

        if (!result.Succeeded) return BadRequest(String.Join(' ',result.Errors.Select(c=>c.Description)));
        
       await _userManager.AddToRoleAsync(user,Constants.Salesman);

        shop.Salesman = user;
        shop.SalesmanId = user.Id;    

        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("/api/shop/salesman/delete/{id}")]
    [SwaggerOperation(
        Summary = "Deletes salesman from shop",
        Description = ""
    )]
    public async Task<IActionResult> DeleteSalesmanAsync(string id)
    {
        var salesman =await _dbContext.Users
            .Include(u=>u.Roles)
            .FirstOrDefaultAsync(u=>u.Id==id);
        if (salesman is null || salesman.Roles is null)
            return BadRequest("Salesman is not found, or user is not salesman");
        //if(salesman.Roles.Any(s=>s.Name==Constants.Admin))
        
        _dbContext.Users.Remove(salesman);
        await _dbContext.SaveChangesAsync();
        
        return Ok();
    }


}