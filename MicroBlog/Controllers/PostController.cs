using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MicroBlog.Helpers;
using MicroBlog.Models.ViewModels;
using MicroBlog.Entities;
using MicroBlog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MicroBlog.Controllers
{
    [Authorize(Roles = "Admin,User")]
    [Route("post")]
    public class PostController : Controller
    {
        readonly IPostService _service;
        readonly IPaginationService _paginationService;
        readonly IMapper _mapper;
        readonly IFileHandlerService _fileHandlerService;
        readonly IAccountService _accountService;

        public PostController(IPostService service, IPaginationService paginationService, IMapper mapper, IFileHandlerService fileHandlerService, IAccountService accountService)
        {
            _service = service;
            _paginationService = paginationService;
            _mapper = mapper;
            _fileHandlerService = fileHandlerService;
            _accountService = accountService;
        }


        [HttpGet(Name ="add-post")]
        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost("",Name = "create-post")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(PostViewModel postViewModel)
        {

            //return View(postViewModel);

            if (ModelState.IsValid)
            {
                var currentUser = await _accountService.GetCurrentUserAsync(HttpContext.User);
                var uploadResult = await _fileHandlerService.UploadFile(postViewModel.File);

                if (uploadResult.Succeeded)
                {
                    postViewModel.Image = uploadResult.FileName;
                    postViewModel.UserId = currentUser.Id;

                    var postEntity = _mapper.Map<Post>(postViewModel);
                    postEntity.Claps = 0;
                    _service.Create(postEntity);
                    var state = await _service.Commit();

                    if (!state)
                    {
                        ModelState.AddModelError(string.Empty, "An error occurred while trying to create the post. Please try again");
                        return View(postViewModel);
                    }

                    return RedirectToAction("index");
                }

                ModelState.AddModelError("Image", uploadResult.ErrorMessage);
            }

            return View(postViewModel);
        }

        //Paginate Post with number of comments
        [HttpGet("list", Name = "post-list")]
        [Route("/", Name = "home")]
        public IActionResult Index(PaginationInfo paginationInfo)
        {
            var posts = _service.GetPostsWithCommentCount();
            var paginated =  _paginationService.Paginate(posts, paginationInfo);

            var viewModel = new PostPagesViewModel()
            {
                Posts = paginated.PagedItems,
                PostPager = paginated.Pager
            };
            return View(viewModel);

            //return View();
        }


        [HttpGet("{id}")]
        public IActionResult Detail(long id)
        {
            var post = _service.GetPostWithComments(id);
            var paginated = _paginationService.Paginate(post.Comments.AsQueryable(), new PaginationInfo());
            var viewModel = new PostPagesViewModel()
            {
                Post = _mapper.Map<PostInfoViewModel>(post),
                Comments = _mapper.Map<IEnumerable<CommentInfoViewModel>>(paginated.PagedItems),
                CommentsPager = paginated.Pager
            };
            return View(viewModel);
        }

        [HttpGet("edit/{id}")]
        public async  Task<IActionResult> EditView(long id)
        {
            var post = await _service.GetByIdAsync(id);
            var viewModel = new PostPagesViewModel()
            {
                Post = _mapper.Map<PostInfoViewModel>(post)
            };

            return View(viewModel);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id,PostViewModel postViewModel)
        {
            return View();
        }

        [HttpPut("{id}/clap")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateClap(long id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Claps += 1;

            _service.Update(entity);
            var state = await _service.Commit();
            return !state ? StatusCode(500, "A problem occurred while handling your request") : (IActionResult)NoContent();
        }
    }
}
