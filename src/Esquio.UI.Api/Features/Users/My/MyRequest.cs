using MediatR;

namespace Esquio.UI.Api.Features.Users.My
{
    public class MyRequest 
        : IRequest<MyResponse>
    {
        public string SubjectId { get; set; }
    }
}
