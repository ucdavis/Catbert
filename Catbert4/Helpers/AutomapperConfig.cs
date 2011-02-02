using AutoMapper;
using Catbert4.Core.Domain;
using Catbert4.Models;
using Catbert4.Services;

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
            CreateMap<Unit, Unit>();

            CreateMap<School, School>();

            CreateMap<Application, Application>()
                .ForMember(x => x.ApplicationRoles, x => x.Ignore());

            CreateMap<AccessToken, AccessToken>();

            CreateMap<Message, Message>();

            CreateUserMaps();
        }

        private void CreateUserMaps()
        {
            CreateMap<User, UserListModel>()
                .ForMember(x => x.Login, x => x.MapFrom(p => p.LoginId));

            CreateMap<User, UserShowModel>()
                .ForMember(x => x.Login, x => x.MapFrom(p => p.LoginId))
                .ForMember(x => x.Permissions, x => x.Ignore())
                .ForMember(x => x.UnitAssociations, x => x.Ignore());

            CreateMap<User, User>()
                .ForMember(x => x.Permissions, x => x.Ignore())
                .ForMember(x => x.UnitAssociations, x => x.Ignore());

            CreateMap<DirectoryUser, User>()
                .ForMember(x => x.Email, x => x.MapFrom(p => p.EmailAddress))
                .ForMember(x => x.Phone, x => x.MapFrom(p => p.PhoneNumber));

            CreateMap<DirectoryUser, ServiceUser>()
                .ForMember(x => x.Login, x => x.MapFrom(p => p.LoginId))
                .ForMember(x => x.Email, x => x.MapFrom(p => p.EmailAddress))
                .ForMember(x => x.Phone, x => x.MapFrom(p => p.PhoneNumber));

            CreateMap<ServiceUser, User>()
                .ForMember(x => x.LoginId, x => x.MapFrom(p => p.Login));
        }
    }
}