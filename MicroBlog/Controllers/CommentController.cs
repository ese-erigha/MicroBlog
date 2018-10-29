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
        public IActionResult GetAll([FromQuery]PaginationInfo paginationInfo)
        {
            var comments = _service.GetCommentsWithUser();
            var paginated = _paginationService.Paginate(comments, paginationInfo);

            var viewModel = new
            {
                Comments = paginated.PagedItems,
                CommentPager = paginated.Pager
            };

            return new OkObjectResult(new ApiResponse<object> { Data = viewModel, Message="200 OK" });
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
                return new ObjectResult(new ApiResponse<object> { Message = "An error occurred while handling your request" })
                {
                    StatusCode = 500
                };
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
        public IActionResult GetById(long id)
        {
            var entity =  _service.GetCommentWithUser(id);
            if(entity == null)
            {
                return new NotFoundObjectResult(new ApiResponse<object> { Message = "Item does not exist" });
            }

            var model = _mapper.Map<ResponseDto.CommentDto>(entity);

            return new OkObjectResult(new ApiResponse<ResponseDto.CommentDto> { Data = model , Message = "200 OK" });
        }

        [HttpPut("{id}/clap")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateClap(long id)
        {
            ObjectResult result;
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
            {
                return new NotFoundObjectResult(new ApiResponse<object>{ Message = "Item does not exist" });

            }
            entity.Claps += 1;

            _service.Update(entity);
            var state = await _service.Commit();

            if(!state)
            {
                result = new ObjectResult(new ApiResponse<object> { Message = "An error occurred while handling your request" })
                {
                    StatusCode = 500
                };
            }

            result = new ObjectResult(new ApiResponse<object> { Message = "Item Update Successful" })
            {
                StatusCode = 204
            };

            return result;
        }
    }
}
