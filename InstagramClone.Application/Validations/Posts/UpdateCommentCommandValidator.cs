using FluentValidation;
using InstagramClone.Application.Queries.User;
using InstagramClone.Application.Extensions;
using InstagramClone.Domain.Exceptions;
using InstagramClone.Application.Validations.Posts.ErrorMessages;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using System.Threading.Tasks;
using System.Threading;
using InstagramClone.Application.Services.Post.Extensions;

namespace InstagramClone.Application.Validations.Users
{
    public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        private readonly IRepository<PostComment> _postCommentRepository;

        public UpdateCommentCommandValidator(IRepository<PostComment> postCommentRepository)
        {
            _postCommentRepository = postCommentRepository;

            RuleFor(x => x.CommentId).MustAsync(IsCommentExists).WithValidationErrorCode(ValidationErrorCode.NotFound)
                .WithMessage(PostValidationErrorMessages.CommentNotFound);
            RuleFor(x => x.Request.Description).NotEmpty().WithValidationErrorCode(ValidationErrorCode.Required)
                .WithMessage(PostValidationErrorMessages.EmptyDescription);
        }

        private async Task<bool> IsCommentExists(int commentId, CancellationToken token)
        {
            return await _postCommentRepository.CommentExistsAsync(commentId, token);
        }
    }
}