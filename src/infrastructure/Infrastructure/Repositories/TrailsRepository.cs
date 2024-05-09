using Application.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class TrailsRepository : EfRepository<Trail>, ITrailsRepository
{
    public TrailsRepository(ApplicationDbContext context) : base(context)
    {
    }
}