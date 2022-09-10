using AutoMapper;
using MaxSoftTechAssignment.BLL.DTOs.AdminDtos;
using MaxSoftTechAssignment.BLL.DTOs.SalesmanDtos;
using MaxSoftTechAssignment.DAL.Entities;

namespace MaxSoftTechAssignment.WEB.Controllers.Mapper;

public class AutoMapperProfile:Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UserRegisterViewModel, User>().ReverseMap();
        CreateMap<ShopViewModel, Shop>().ReverseMap();
        CreateMap<User, SalesmanViewModel>().ReverseMap();

    }
    
}