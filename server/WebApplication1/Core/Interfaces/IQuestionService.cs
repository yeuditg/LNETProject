using WebApplication1.Core.DTOs;

namespace WebApplication1.Core.Interfaces
{
    public interface IQuestionService
    {
        CreateQuestionResponse Create(CreateQuestionRequest request);
        GetQuestionResponse Get(int id);
        List<GetQuestionResponse> GetAll();
        VoteResponse Vote(VoteRequest request);
    }
}
