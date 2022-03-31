using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Queries.User
{
    public class GetPostPhotoQuery : IRequest<byte[]>
    {
        public GetPostPhotoQuery(int postId)
        {
            PostId = postId;
        }

        public int PostId { get; }
    }

    public class GetPostPhotoQueryHandler : IRequestHandler<GetPostPhotoQuery, byte[]>
    {
        private readonly IRepository<UserPost> _userPostRepository;

        public GetPostPhotoQueryHandler(IRepository<UserPost> userPostRepository)
        {
            _userPostRepository = userPostRepository;
        }

        public async Task<byte[]> Handle(GetPostPhotoQuery command, CancellationToken cancellationToken)
        {
            return await _userPostRepository.Query.AsNoTracking()
                .Where(p => p.Id == command.PostId)
                .Select(p => p.Photo)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}