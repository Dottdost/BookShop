using AutoMapper;
using BookShop.Auth.DTOAuth.Requests;
using BookShop.Auth.ModelsAuth;
using BookShop.Data.Models;

namespace BookShop.Auth.DataAuth.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Создаём маппинг между RegisterRequest и User
        CreateMap<RegisterRequest, User>()
            // Маппим поле Username из RegisterRequest в UserName в модели User
            .ForMember(dest => dest.UserName, opt =>
                opt.MapFrom(src => src.Username))
            // Маппим поле Email в модели User
            .ForMember(dest => dest.Email, opt =>
                opt.MapFrom(src => src.Email))
            // Маппим поле Password в модели User
            .ForMember(dest => dest.PasswordHash, opt =>
                opt.MapFrom(src => src.Password));
    }
}

/*Data Mapping в программировании — это процесс сопоставления данных между
 двумя разными системами или источниками. Он позволяет определить, какие данные из одного источника
  могут быть использованы в другом, а также
 как эти данные должны быть представлены и преобразованы для использования в другом контексте.*/