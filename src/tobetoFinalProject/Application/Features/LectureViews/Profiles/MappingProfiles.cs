using Application.Features.LectureViews.Commands.Create;
using Application.Features.LectureViews.Commands.Delete;
using Application.Features.LectureViews.Commands.Update;
using Application.Features.LectureViews.Queries.GetById;
using Application.Features.LectureViews.Queries.GetList;
using AutoMapper;
using Core.Application.Responses;
using Domain.Entities;
using Core.Persistence.Paging;

namespace Application.Features.LectureViews.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<LectureView, CreateLectureViewCommand>().ReverseMap();
        CreateMap<LectureView, CreatedLectureViewResponse>().ReverseMap();
        CreateMap<LectureView, UpdateLectureViewCommand>().ReverseMap();
        CreateMap<LectureView, UpdatedLectureViewResponse>().ReverseMap();
        CreateMap<LectureView, DeleteLectureViewCommand>().ReverseMap();
        CreateMap<LectureView, DeletedLectureViewResponse>().ReverseMap();
        CreateMap<LectureView, GetByIdLectureViewResponse>().ReverseMap();
        CreateMap<LectureView, GetListLectureViewListItemDto>().ReverseMap();
        CreateMap<IPaginate<LectureView>, GetListResponse<GetListLectureViewListItemDto>>().ReverseMap();
    }
}