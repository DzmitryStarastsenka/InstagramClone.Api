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
    public class UpdateCommentCommand : IRequest<Unit>
    {
        public UpdateCommentCommand(int commentId, UpdateCommentRequest request)
        {
            CommentId = commentId;
            Request = request;
        }

        public int CommentId { get; }
        public UpdateCommentRequest Request { get; }
    }

    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Unit>
    {
        private readonly IRepository<PostComment> _postCommentRepository;
        private readonly IMapper _mapper;

        public UpdateCommentCommandHandler(IRepository<PostComment> postCommentRepository, IMapper mapper)
        {
            _postCommentRepository = postCommentRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateCommentCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var postComment = await _postCommentRepository.GetByIdAsync(command.CommentId, cancellationToken);

            _mapper.Map(request, postComment);
            postComment.CommentEditedAt = DateTime.UtcNow;

            _postCommentRepository.Update(postComment);
            await _postCommentRepository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}