using InstagramClone.Application.Helpers;
using InstagramClone.Application.Services.Post.Extensions;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Queries.User
{
    public class UpdatePostPhotoCommand : IRequest<Unit>
    {
        public UpdatePostPhotoCommand(int postId, IFormFile photo)
        {
            PostId = postId;
            Photo = photo;
        }

        public int PostId { get; }
        public IFormFile Photo { get; }
    }

    public class UpdatePostPhotoCommandHandler : IRequestHandler<UpdatePostPhotoCommand, Unit>
    {
        private readonly IRepository<UserPost> _userPostRepository;

        public UpdatePostPhotoCommandHandler(IRepository<UserPost> userPostRepository)
        {
            _userPostRepository = userPostRepository;
        }

        public async Task<Unit> Handle(UpdatePostPhotoCommand command, CancellationToken cancellationToken)
        {
            var post = await _userPostRepository.GetByIdAsync(command.PostId, cancellationToken);

            post.Photo = FileConvertHelper.ConvertToBytes(command.Photo);
            post.PostEditedAt = DateTime.UtcNow;

            _userPostRepository.Update(post);
            await _userPostRepository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}