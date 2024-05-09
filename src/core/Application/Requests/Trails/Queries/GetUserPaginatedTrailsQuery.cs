using Application.Repositories;
using Application.Repositories.Specifications;
using Application.Requests.Trails.Models;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Requests.Trails.Queries;

public record GetUserPaginatedTrailsQuery(string UserId, int Page, int PageSize) : IRequest<List<TrailVm>>;

internal sealed class GetUserPaginatedTrailsQueryHandler : IRequestHandler<GetUserPaginatedTrailsQuery, List<TrailVm>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserPaginatedTrailsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<TrailVm>> Handle(GetUserPaginatedTrailsQuery request, CancellationToken cancellationToken)
    {
        var spec = new Specification<Trail>(x => x.UserId == request.UserId)
            .ApplyOrderByDesc(x => x.DateTime)
            .ApplyPaging(request.PageSize * (request.Page - 1), request.PageSize);

        var trails = await _unitOfWork.TrailsRepository.GetAsync(spec);
        return trails.Adapt<List<TrailVm>>();
    }
}