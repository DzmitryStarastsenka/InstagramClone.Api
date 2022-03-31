using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramClone.Application.Commands.User
{
    public class UpdateUserCommand
    {
    }
}


//public async Task UpdateAsync(UserProfileDto userParam, string password = null)
//{
//    var user = _userProfileRepository.Find(userParam.Id);

//    if (user == null)
//        throw new NotFoundApiException(UserValidationErrorMessages.UserNotFound);

//    if (userParam.UserName != user.UserName)
//    {
//        if (await UserExistsAsync(userParam.UserName))
//            throw new AlreadyExistsApiException(string.Format(UserValidationErrorMessages.UserAlreadyExists, user.UserName));
//    }

//    user.FirstName = userParam.FirstName;
//    user.LastName = userParam.LastName;
//    user.UserName = userParam.UserName;

//    PasswordHashHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

//    user.PasswordHash = passwordHash;
//    user.PasswordSalt = passwordSalt;

//    _userProfileRepository.Update(user);
//    await _userProfileRepository.SaveChangesAsync(CancellationToken.None);
//}