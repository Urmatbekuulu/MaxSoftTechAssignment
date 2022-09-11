using AutoMapper;
using MaxSoftTechAssignment.BLL.DTOs.AdminDtos;
using MaxSoftTechAssignment.DAL;
using MaxSoftTechAssignment.DAL.Data;
using MaxSoftTechAssignment.DAL.Entities;
using MaxSoftTechAssignment.WEB.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace MaxSoftTechAssignment.WEB.Controllers;


public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<AdminController> _logger;
    private readonly IMapper _mapper;
    public AdminController(ApplicationDbContext dbContext, UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<AdminController> logger, IMapper mapper)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpPost("/api/user/create")]
    [SwaggerOperation(
            Summary = "",
            Description = ""
    )]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserRegisterViewModel request)
    {
        if (!ModelState.IsValid) return BadRequest();

        for (int i = 0; i < request.Roles.Count; i++) request.Roles[i] = request.Roles[i].ToUpper();

        var roles = await _dbContext.Roles
            .Where(
                r => request.Roles.Any(s => s == r.Name)
            )
            .ToListAsync();

        if (!roles.Any()) return BadRequest("Roles are not found");


        var user = _mapper.Map<UserRegisterViewModel, User>(request);


        if (await _userManager.Users.AnyAsync(u => u.Email == user.Email || u.UserName == user.UserName))
            return BadRequest("Username or email is registered");
        user.Roles = roles;
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded) return BadRequest(String.Join(' ', result.Errors.Select(c => c.Description)));


        return Ok();
    }

    [HttpPut("/api/user/update/{id}")]
    [SwaggerOperation(
        Summary = "Update user information",
        Description = "Not realized"
    )]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UserRegisterViewModel user, [FromRoute] string id)
    {
        //not realized

        return BadRequest("Not realized yet");

    }

    [HttpGet("/api/users/{role}")]
    [SwaggerOperation(
        Summary = "Get users by role",
        Description = ""
    )]
    public async Task<ActionResult<IEnumerable<UserRegisterViewModel>>> GetUsersByRoleAsync(string? role)
    {
        if (role is null ||
            Constants.Admin != role.ToUpper()
            && Constants.Manager != role.ToUpper()
            && Constants.Salesman != role.ToUpper())

            return BadRequest("Role not found");

        var users = await _dbContext.Users.Include(u => u.Roles)
            .Where(u => u.Roles.Any(r => r.Name == role.ToUpper()))
            .ToListAsync();

        if (users.Count == 0) return BadRequest($"{role} are not found");

        var response = _mapper.Map<IEnumerable<UserRegisterViewModel>>(users);

        return Ok(response);
    }

    [HttpPost("/api/shop/create")]
    [SwaggerOperation(
        Summary = "Creates shop",
        Description = "Creates shop with workers"
    )]
    public async Task<IActionResult> CreateShopAsync([FromBody] ShopViewModel request)
    {
        if (!ModelState.IsValid) return BadRequest("Not valid view model");

        var shop = _mapper.Map<ShopViewModel, Shop>(request);

        if (string.IsNullOrEmpty(shop.SalesmanId)) shop.SalesmanId = null;
        if (string.IsNullOrEmpty(shop.ManagerId)) shop.ManagerId = null;

        if (!string.IsNullOrEmpty(shop.SalesmanId) && !(await _dbContext.Users.AnyAsync(u => u.Id == shop.SalesmanId)))
            return BadRequest("Salesman is not found");

        if (!string.IsNullOrEmpty(shop.ManagerId) && !(await _dbContext.Users.AnyAsync(u => u.Id == shop.ManagerId)))
            return BadRequest("Manager is not found");

        _dbContext.Shops.Add(shop);


        await _dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("/api/shops")]
    [SwaggerOperation(
        Summary = "Get list of all shops",
        Description = "Returns all shops")]

    public async Task<ActionResult<IEnumerable<ShopViewModel>>> GetShopsAsync()
    {
       var shops = await _dbContext.Shops.ToListAsync();

        var response = _mapper.Map<List<ShopViewModel>>(shops);

        return Ok(shops);
    }

}