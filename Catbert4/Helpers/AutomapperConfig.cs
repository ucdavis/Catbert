using AutoMapper;
using Catbert4.Core.Domain;

namespace Catbert4.Helpers
{
    public class AutomapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<ViewModelProfile>());
        }
    }

    public class ViewModelProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Unit, Unit>()
                .ForMember(x=>x.Id, x=>x.Ignore())
                .ForMember(x=>x.School, x=>x.Ignore());
        }
    }
}