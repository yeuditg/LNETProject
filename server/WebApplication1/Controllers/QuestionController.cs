using Microsoft.AspNetCore.Mvc;
using WebApplication1.Core.DTOs;
using WebApplication1.Core.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/questions")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _service;

        public QuestionsController(IQuestionService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult<CreateQuestionResponse> Create(CreateQuestionRequest request)
        {
            try
            {
                var result = _service.Create(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // אפשר לשנות ל-StatusCode לפי סוג השגיאה
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<GetQuestionResponse> Get(int id)
        {
            try
            {
                var result = _service.Get(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult<GetQuestionResponse> GetAll()
        {
            try
            {
                var result = _service.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("vote")]
        public ActionResult<VoteResponse> Vote(VoteRequest request)
        {
            try
            {
                var result = _service.Vote(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

