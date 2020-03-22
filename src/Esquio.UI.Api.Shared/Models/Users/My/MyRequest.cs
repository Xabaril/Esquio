using MediatR;

namespace Esquio.UI.Api.Shared.Models.Users.My
{
    public class MyRequest 
        : IRequest<MyResponse>
    {
        public string SubjectId { get; set; }
    }
}
