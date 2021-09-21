using FluentValidation;

namespace InstagramClone.Application.Models.Authentificate
{
	public class ValidateTokenRequestValidator : AbstractValidator<ValidateTokenRequest>
	{
		public ValidateTokenRequestValidator()
		{
			RuleFor(x => x.Token).NotEmpty();
		}
	}
}