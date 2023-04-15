using AutoMapper;
using ToDoList.Core.Definitions.DalModels;
using ToDoList.Core.ViewModels;
using ToDoList.Core.ViewModels.Base;
using ToDoList.Core.ViewModels.Interfaces;
using ToDoList.Core.ViewModels.Items;

namespace ToDoList.Core;

public static class Mapper
{
    internal static IMapper Build() => GetMapperConfiguration().CreateMapper();

    private static MapperConfiguration GetMapperConfiguration() => new (cfg =>
    {
        cfg.CreateMap<ToDoListItemDalModel, ToDoListItemViewModel>()
            .ForMember(d => d.Item, o => o.MapFrom(s => s))
            .ValidateMemberList(MemberList.Destination);


        cfg.CreateMap<ToDoListItemViewModel, ToDoListItemDalModel>()
            .ValidateMemberList(MemberList.None);


        cfg.CreateMap<ToDoListItemViewModel, IBaseActionViewModel>()
            .ForMember(d => d.CurrentToDoList, o => o.MapFrom(s => s))
            .ValidateMemberList(MemberList.Destination);


        cfg.CreateMap<ToDoListItemViewModel, EditTaskViewModel>()
            .IncludeBase<ToDoListItemViewModel, IBaseActionViewModel>()
            .ForMember(d => d.CreatedAt, o => o.MapFrom((s, _) => s.Item.CreatedAt?.ToString("hh:mm dd-MM-yyyy")))
            .ForMember(d => d.UpdatedAt, o => o.MapFrom((s, _) => s.Item.UpdatedAt?.ToString("hh:mm dd-MM-yyyy")))
            .ValidateMemberList(MemberList.None);


        cfg.CreateMap<IBaseActionViewModel, ToDoListItemViewModel>()
            .ValidateMemberList(MemberList.None)
            .AfterMap((s, d, m) => m.Mapper.Map(d, d.Item));


        cfg.CreateMap<EditTaskViewModel, ToDoListItemViewModel>()
            .IncludeBase<IBaseActionViewModel, ToDoListItemViewModel>()
            .ForMember(d => d.Item, o => o.Ignore())
            .ValidateMemberList(MemberList.Destination);


        cfg.CreateMap<ToDoListItemViewModel, NewTaskViewModel>()
            .IncludeBase<ToDoListItemViewModel, IBaseActionViewModel>()
            .ValidateMemberList(MemberList.None);


        cfg.CreateMap<NewTaskViewModel, ToDoListItemViewModel>()
            .IncludeBase<IBaseActionViewModel, ToDoListItemViewModel>()
            .ValidateMemberList(MemberList.None);
    });
}
