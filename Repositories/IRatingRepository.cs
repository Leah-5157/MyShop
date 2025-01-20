using Entities;

namespace Repositories
{
    public interface IRatingRepository
    {
        Rating Post(Rating rating);
    }
}