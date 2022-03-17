namespace WebBlog.Data.Repository
{
    public interface IUnitOfWork
    {
        public IBlogRepository blogRepo { get; }
        public IUserRepository userRepo { get; }
        public IPostCommentRepository commentRepo { get; }
        public void Complete();
    }
}
