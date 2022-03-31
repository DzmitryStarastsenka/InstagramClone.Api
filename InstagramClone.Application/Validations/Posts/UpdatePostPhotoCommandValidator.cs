using FluentValidation;
using InstagramClone.Domain.DAL;
using System.Threading;
using System.Threading.Tasks;
using InstagramClone.Application.Queries.User;
using InstagramClone.Application.Extensions;
using InstagramClone.Domain.Exceptions;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Application.Services.Post.Extensions;
using InstagramClone.Application.Validations.Posts.ErrorMessages;
using Microsoft.AspNetCore.Http;
using InstagramClone.Domain.Constants.Posts;

namespace InstagramClone.Application.Validations.Users
{
    public class UpdatePostPhotoCommandValidator : AbstractValidator<UpdatePostPhotoCommand>
    {
        private readonly IRepository<UserPost> _userPostRepository;

        public UpdatePostPhotoCommandValidator(IRepository<UserPost> userPostRepository)
        {
            _userPostRepository = userPostRepository;

            RuleFor(x => x.PostId).MustAsync(IsPostExists).WithValidationErrorCode(ValidationErrorCode.NotFound)
                .WithMessage(PostValidationErrorMessages.PostNotFound);

            RuleFor(x => x.Photo).NotEmpty().WithValidationErrorCode(ValidationErrorCode.Required)
                .WithMessage(PostValidationErrorMessages.EmptyFile);
            RuleFor(x => x.Photo).Must(IsTypeCorrect).WithValidationErrorCode(ValidationErrorCode.Invalid)
                .WithMessage(PostValidationErrorMessages.WrongFileType);
        }

        private async Task<bool> IsPostExists(int postId, CancellationToken token)
        {
            return await _userPostRepository.PostExistsAsync(postId, token);
        }

        private bool IsTypeCorrect(IFormFile file)
        {
            return file.ContentType == PostsConstants.CorrectFileType;
        }
    }
}