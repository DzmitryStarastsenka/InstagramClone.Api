using AutoMapper;
using InstagramClone.Application.Models.Post.Requests;
using InstagramClone.Application.Services.Post.Extensions;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Queries.User
{
    public class UpdatePostCommand : IRequest<Unit>
    {
        public UpdatePostCommand(int postId, UpdatePostRequest request)
        {
            PostId = postId;
            Request = request;
        }

        public int PostId { get; }
        public UpdatePostRequest Request { get; }
    }

    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Unit>
    {
        private readonly IRepository<UserPost> _userPostRepository;
        private readonly IMapper _mapper;

        public UpdatePostCommandHandler(IRepository<UserPost> userPostRepository, IMapper mapper)
        {
            _userPostRepository = userPostRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdatePostCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var post = await _userPostRepository.GetByIdAsync(command.PostId, cancellationToken);

            _mapper.Map(request, post);
            post.PostEditedAt = DateTime.UtcNow;

            _userPostRepository.Update(post);
            await _userPostRepository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}