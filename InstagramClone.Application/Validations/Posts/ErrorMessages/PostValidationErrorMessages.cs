using InstagramClone.Domain.Constants.Posts;

namespace InstagramClone.Application.Validations.Posts.ErrorMessages
{
    public class PostValidationErrorMessages
    {
        public const string PostNotFound = "Post not found.";
        public const string CommentNotFound = "Comment not found.";
        public const string EmptyFile = "File is required.";
        public const string WrongFileType = $"Wrong file type. For now supports only {PostsConstants.CorrectFileType} file type.";
        public const string AlreadyLiked = "Post has been already liked.";
        public const string NotLiked = "Post is not liked.";

        public const string EmptyDescription = "Desciption is required.";
        public const string DontHaveAccessToPost = "You don't have permission to access this post.";
        public const string DontHaveAccessToComment = "You don't have permission to access this comment.";
    }
}