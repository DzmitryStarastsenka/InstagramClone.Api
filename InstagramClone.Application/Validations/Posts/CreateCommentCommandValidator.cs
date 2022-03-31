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
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        private readonly IRepository<UserPost> _userPostRepository;

        public CreateCommentCommandValidator(IRepository<UserPost> userPostRepository)
        {
            _userPostRepository = userPostRepository;

            RuleFor(x => x.PostId).MustAsync(IsPostExists).WithValidationErrorCode(ValidationErrorCode.NotFound)
                .WithMessage(PostValidationErrorMessages.PostNotFound);
            RuleFor(x => x.Request.Description).NotEmpty().WithValidationErrorCode(ValidationErrorCode.Required)
                .WithMessage(PostValidationErrorMessages.EmptyDescription);
        }

        private async Task<bool> IsPostExists(int postId, CancellationToken token)
        {
            return await _userPostRepository.PostExistsAsync(postId, token);
        }
    }
}