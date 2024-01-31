using Application.Features.StudentExperiences.Commands.Create;
using Application.Features.StudentExperiences.Commands.Delete;
using Application.Features.StudentExperiences.Commands.Update;
using Application.Features.StudentExperiences.Queries.GetById;
using Application.Features.StudentExperiences.Queries.GetList;
using AutoMapper;
using Core.Application.Responses;
using Domain.Entities;
using Core.Persistence.Paging;
using Application.Features.StudentEducations.Queries.GetList;

namespace Application.Features.StudentExperiences.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<StudentExperience, CreateStudentExperienceCommand>().ReverseMap();
        CreateMap<StudentExperience, CreatedStudentExperienceResponse>().ReverseMap();
        CreateMap<StudentExperience, UpdateStudentExperienceCommand>().ReverseMap();
        CreateMap<StudentExperience, UpdatedStudentExperienceResponse>().ReverseMap();
        CreateMap<StudentExperience, DeleteStudentExperienceCommand>().ReverseMap();
        CreateMap<StudentExperience, DeletedStudentExperienceResponse>().ReverseMap();
        CreateMap<StudentExperience, GetByIdStudentExperienceResponse>().ReverseMap();
        CreateMap<StudentExperience, GetListStudentExperienceListItemDto>().ReverseMap();
        CreateMap<IPaginate<StudentExperience>, GetListResponse<GetListStudentExperienceListItemDto>>().ReverseMap();

        CreateMap<StudentExperience, GetListStudentExperienceListItemDto>()
       .ForMember(dest => dest.StudentFirstName, opt => opt.MapFrom(src => src.Student.User.FirstName))
       .ForMember(dest => dest.StudentLastName, opt => opt.MapFrom(src => src.Student.User.LastName))
       .ForMember(dest => dest.StudentEmail, opt => opt.MapFrom(src => src.Student.User.Email))
       ;

    }
}