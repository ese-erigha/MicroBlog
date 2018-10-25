using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MicroBlog.Entities;
using CreateDto = MicroBlog.Models.Dto.CreateDto;
using ResponseDto = MicroBlog.Models.Dto.ResponseDto;
using MicroBlog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MicroBlog.Helpers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MicroBlog.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {

        readonly ICommentService _service;
        readonly IMapper _mapper;
        readonly IPaginationService _paginationService;

        public CommentController(ICommentService service, IMapper mapper, IPaginationService paginationService)
        {
            _service = service;
            _mapper = mapper;
            _paginationService = paginationService;
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult GetAll(PaginationInfo paginationInfo)
        {
            var comments = _service.GetCommentsWithUser();
            var paginated = _paginationService.Paginate(comments, paginationInfo);

            var viewModel = new
            {
                Comments = paginated.PagedItems,
                CommentPager = paginated.Pager
            };

            return Ok(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody]CreateDto.CommentDto model)
        {
            var entity = _mapper.Map<Comment>(model);
            entity.Claps = 0;

            _service.Create(entity);

            var state = await _service.Commit();

            if(!state){
                return StatusCode(500, "A problem occurred while handling your request");
            }

            var modelToReturn = _mapper.Map<ResponseDto.CommentDto>(entity);

            return CreatedAtRoute(
                //routeName: "GetById",
                routeValues: new {id = entity.Id},
                value: modelToReturn
            );
            
        }


        [HttpGet("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetById(long id)
        {
            var entity = await _service.GetByIdAsync(id);
            if(entity == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<ResponseDto.CommentDto>(entity);

            return Ok(model);
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
