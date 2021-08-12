using System;
using System.Linq;
using API.DTO;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<AppUser, MemberDTO>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDTO>();
            CreateMap<MemberUpdateDTO, AppUser>();
            CreateMap<RegisterDTO, AppUser>();
            CreateMap<Message, MessageDTO>()
            .ForMember(dest => dest.SenderPhotoUrl, opt => opt
            .MapFrom(src => src.Sender.Photos.FirstOrDefault(p => p.IsMain).Url
            ))
            .ForMember(dest => dest.RecipientPhotoUrl, opt => opt
            .MapFrom(src => src.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url
            ));
            // For Making the Time with letter Z at the end .
            CreateMap<DateTime, DateTime>()
            .ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));


        }
    }
}