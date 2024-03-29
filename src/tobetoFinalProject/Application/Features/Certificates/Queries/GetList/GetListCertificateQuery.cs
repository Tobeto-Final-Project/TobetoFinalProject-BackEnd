using Application.Features.Certificates.Constants;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Caching;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Paging;
using MediatR;
using static Application.Features.Certificates.Constants.CertificatesOperationClaims;

namespace Application.Features.Certificates.Queries.GetList;

public class GetListCertificateQuery : IRequest<GetListResponse<GetListCertificateListItemDto>>, ISecuredRequest, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => new[] { Admin, Read };

    public bool BypassCache { get; }
    public string CacheKey => $"GetListCertificates({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string CacheGroupKey => "GetCertificates";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListCertificateQueryHandler : IRequestHandler<GetListCertificateQuery, GetListResponse<GetListCertificateListItemDto>>
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IMapper _mapper;

        public GetListCertificateQueryHandler(ICertificateRepository certificateRepository, IMapper mapper)
        {
            _certificateRepository = certificateRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListCertificateListItemDto>> Handle(GetListCertificateQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Certificate> certificates = await _certificateRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                orderBy: ce => ce.OrderByDescending(x => x.CreatedDate),
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListCertificateListItemDto> response = _mapper.Map<GetListResponse<GetListCertificateListItemDto>>(certificates);
            return response;
        }
    }
}