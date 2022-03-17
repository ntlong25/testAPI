using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using WebBlog.Data.Repository;
using WebBlog.Data.Model;
using Microsoft.AspNetCore.Hosting;
using WebBlog.Data.Models.Responses;
using WebBlog.Data.Models.Requests;

namespace WebBlog.Business.Services.Implement
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IWebHostEnvironment _hostEnvironment;
        //public string userId;

        public BlogService(IUnitOfWork unitOfWork 
                            //IHttpContextAccessor httpContextAccessor,
                            //IWebHostEnvironment webHostEnvironment
            )
        {
            _unitOfWork = unitOfWork;
            //userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            //_hostEnvironment = webHostEnvironment;
        }
        //private string UploadedFile(Post post)
        //{
        //    string uniqueFileName = null;



        //    if (post.ImageUpload != null)
        //    {
        //        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
        //        uniqueFileName = Guid.NewGuid().ToString() + "_" + post.ImageUpload.FileName;
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            post.ImageUpload.CopyTo(fileStream);
        //        }
        //    }
        //    return uniqueFileName;
        //}
        public void CreatePost(Post post)
        {
            try
            {
                string imageName = "";//UploadedFile(post);

                var result = new Post
                {
                    authorId = post.authorId,
                    parentId = post.parentId,
                    title = post.title,
                    metaTitle = post.metaTitle,
                    slug = post.slug,
                    summary = post.summary,
                    published = post.published,
                    createAt = DateTime.Now,
                    updateAt = DateTime.Now,
                    publishedAt = DateTime.Now,
                    content = post.content,
                    image = imageName
                };
                _unitOfWork.blogRepo.Create(result);
                _unitOfWork.Complete();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void UpdatePost(Post post)
        {
            try
            {
                string imageName = "";// UploadedFile(post);
                var result = _unitOfWork.blogRepo.Get(post.ID);
                if (post != null)
                {
                    result.authorId = post.authorId;
                    result.parentId = post.parentId;
                    result.title = post.title;
                    result.metaTitle = post.metaTitle;
                    result.slug = post.slug;
                    result.summary = post.summary;
                    result.published = post.published;
                    result.updateAt = DateTime.Now;
                    result.publishedAt = DateTime.Now;
                    result.content = post.content;
                    result.image = imageName;

                    _unitOfWork.blogRepo.Update(result);
                    _unitOfWork.Complete();
                }
                else
                {
                    throw new Exception($"Id {post.ID} not found.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public IEnumerable<Post> GetPost()
        {
            var result = _unitOfWork.blogRepo.GetAll();
            return result;
        }

        public IEnumerable<BlogItemResponse> GetAllItemBlog()
        {
            var result = _unitOfWork.blogRepo.GetAllItemBlog();
            return result;
        }
        public Post GetPost(long id)
        {
            try
            {
                var result = _unitOfWork.blogRepo.Get(id);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void DeletePost(long id)
        {
            try
            {
                _unitOfWork.blogRepo.Delete(id);
                _unitOfWork.Complete();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public IEnumerable<BlogItemResponse> GetNewPost()
        {
            try
            {
                var result = _unitOfWork.blogRepo.GetAllItemBlog().Take(3).ToList();
                result.Sort((x, y) => x.createAt.CompareTo(y.createAt));
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<Post> SearchPost(string name)
        {
            try
            {
                return _unitOfWork.blogRepo.List(p => p.title!.Contains(name) ||
                                                        p.metaTitle!.Contains(name) ||
                                                        p.User.firstName!.Contains(name) ||
                                                        p.User.middleName!.Contains(name) ||
                                                        p.User.lastName!.Contains(name) ||
                                                        p.summary!.Contains(name));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<PostComment> GetComment()
        {
            return _unitOfWork.commentRepo.GetAll();
        }
        public IQueryable<PostComment> GetComment(long id)
        {
            return _unitOfWork.commentRepo.Get(id);
        }

        public void AddComment(AddCommentRequest addComment)
        {
            try
            {
                var result = new PostComment
                {
                    postId = addComment.postId,
                    content = addComment.content,
                    userId = addComment.userId,
                    createAt = DateTime.Now,
                    publishedAt = DateTime.Now,
                    published = 1,
                    title = ""
                };
                _unitOfWork.commentRepo.Create(result);
                _unitOfWork.Complete();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void AddComment(AddSubCommentRequest addSubComment)
        {
            try
            {
                var result = new PostComment
                {
                    postId = addSubComment.postId,
                    content = addSubComment.content,
                    userId = addSubComment.userId,
                    parentId = addSubComment.parentId,
                    createAt = DateTime.Now,
                    publishedAt = DateTime.Now,
                    published = 1,
                    title = ""
                };
                _unitOfWork.commentRepo.Create(result);
                _unitOfWork.Complete();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
