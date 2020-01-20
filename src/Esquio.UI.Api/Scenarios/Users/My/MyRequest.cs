using MediatR;

namespace Esquio.UI.Api.Scenarios.Users.My
{
    public class MyRequest 
        : IRequest<MyResponse>
    {
        public string SubjectId { get; set; }
    }
}
