namespace WebBlog.Data.Repository.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private DataContext _context;
        public IBlogRepository blogRepo { get; }
        public IUserRepository userRepo { get; }

        public IPostCommentRepository commentRepo { get; }

        public UnitOfWork(DataContext dataContext)
        {
            _context = dataContext;
            userRepo = new UserRepository(_context);
            blogRepo = new BlogRepository(_context);
            commentRepo = new PostCommentRepository(_context);
        }

        public void Complete()
        {
            _context.SaveChanges(); 
        }
    }
}
