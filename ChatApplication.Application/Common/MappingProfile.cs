using AutoMapper;
using ChatApplication.Application.DTOs.Agent;
using ChatApplication.Application.DTOs.Chat;
using ChatApplication.Domain.Entities;

namespace ChatApplication.Application.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Create_Update_Agent_DTO, Agent>()
                .ForMember(d => d.ShiftEndHour, s => s.MapFrom(s => s.ShiftStartHour + 8 > 23 ?
                                                                    s.ShiftStartHour + 8 - 24 :
                                                                    s.ShiftStartHour + 8))
                .ForMember(d => d.MaxChatLoad, s => s.MapFrom(s => 10));

            CreateMap<InitiateChatDTO, Customer>()
                .ForMember(d => d.Id, s => s.Ignore());
        }

    }
}
