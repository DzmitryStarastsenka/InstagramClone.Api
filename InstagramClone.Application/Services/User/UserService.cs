using AutoMapper;
using InstagramClone.Application.Helpers;
using InstagramClone.Application.Models.Authentificate;
using InstagramClone.Application.Models.User;
using InstagramClone.Application.Services.User.Extensions;
using InstagramClone.Application.Services.User.Interfaces;
using InstagramClone.Application.Validations.Users.ErrorMessages;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.Exceptions;
using InstagramClone.Domain.Jwt.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IUserJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public UserService(IRepository<UserProfile> userProfileRepository,
            IUserJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper)
        {
            _userProfileRepository = userProfileRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<UserProfileDTO> AuthenticateAsync(LoginRequest loginRequest)
        {
            var user = await _userProfileRepository.GetByUsernameAsync(loginRequest.UserName);

            if (user == null)
                throw new NotFoundApiException();

            if (!PasswordHashHelper.VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return _mapper.Map<UserProfileDTO>(user);
        }

        public async Task<List<UserProfileDTO>> GetUserProfiles(SieveModel request)
        {
            return _mapper.Map<List<UserProfileDTO>>(await _userProfileRepository.FilterUserProfiles(request).ToListAsync());
        }

        public UserProfileDTO GetById(int id)
        {
            return _mapper.Map<UserProfileDTO>(_userProfileRepository.Find(id));
        }

        public async Task<UserProfileDTO> GetByUserNameAsync(string userName)
        {
            return _mapper.Map<UserProfileDTO>(await _userProfileRepository.Query.FirstOrDefaultAsync(u => u.UserName == userName));
        }

        public async Task<UserProfileDTO> CreateAsync(UserProfileDTO user, string password)
        {
            if (await UserExistsAsync(user.UserName))
                throw new AlreadyExistsApiException(string.Format(UserErrorMessages.UserAlreadyExists, user.UserName));

            PasswordHashHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var userProfile = _mapper.Map<UserProfile>(user);

            userProfile.PasswordHash = passwordHash;
            userProfile.PasswordSalt = passwordSalt;

            _userProfileRepository.Insert(userProfile);
            await _userProfileRepository.SaveChangesAsync(CancellationToken.None);

            return user;
        }

        public async Task<bool> UserExistsAsync(string userName)
        {
            return await _userProfileRepository.Query.AnyAsync(u => u.UserName == userName);
        }

        //var post = _postRepository.Find(request.Id);
        //if (post == null)
        //    throw new NotFoundApiException(PostErrorMessages.PostNotFound);

        //var requestPost = _mapper.Map<UserPostDTO>(request);

        //post.Description = requestPost.Description;
        //post.PostEditedAt = DateTime.Now;

        //_postRepository.Update(post);
        //await _postRepository.SaveChangesAsync(CancellationToken.None);

        //return _mapper.Map<UserPostDTO>(post);


        public async Task UpdateAsync(UserProfileDTO userParam, string password = null)
        {
            var user = _userProfileRepository.Find(userParam.Id);

            if (user == null)
                throw new NotFoundApiException(UserErrorMessages.UserNotFound);

            if (userParam.UserName != user.UserName)
            {
                if (await UserExistsAsync(userParam.UserName))
                    throw new AlreadyExistsApiException(string.Format(UserErrorMessages.UserAlreadyExists, user.UserName));
            }

            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.UserName = userParam.UserName;

            PasswordHashHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _userProfileRepository.Update(user);
            await _userProfileRepository.SaveChangesAsync(CancellationToken.None);
        }

        public string GenerateToken(UserProfileDTO user)
        {
            return _jwtTokenGenerator.GenerateToken(user.UserName, user.FirstName + " " + user.LastName);
        }

        public bool ValidateToken(string token)
        {
            return _jwtTokenGenerator.ValidateToken(token);
        }
    }
}