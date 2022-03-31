using FluentValidation;
using InstagramClone.Domain.DAL;
using System.Threading;
using System.Threading.Tasks;
using InstagramClone.Application.Queries.User;
using InstagramClone.Application.Extensions;
using InstagramClone.Domain.Exceptions;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Application.Validations.Posts.ErrorMessages;
using InstagramClone.Application.Services.Post.Extensions;

namespace InstagramClone.Application.Validations.Users
{
    public class GetCommentsQueryValidator : AbstractValidator<GetCommentsQuery>
    {
        private readonly IRepository<UserPost> _userPostRepository;

        public GetCommentsQueryValidator(IRepository<UserPost> userPostRepository)
        {
            _userPostRepository = userPostRepository;

            RuleFor(x => x.PostId).MustAsync(IsPostExists).WithValidationErrorCode(ValidationErrorCode.NotFound)
                .WithMessage(PostValidationErrorMessages.PostNotFound);
        }

        private async Task<bool> IsPostExists(int postId, CancellationToken token)
        {
            return await _userPostRepository.PostExistsAsync(postId, token);
        }
    }
}
