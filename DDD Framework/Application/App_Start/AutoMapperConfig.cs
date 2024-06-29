using AutoMapper;
using Domain.Profiles;

namespace Application.App_Start
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AccountProfile());
            });

            return config;
        }
    }
}