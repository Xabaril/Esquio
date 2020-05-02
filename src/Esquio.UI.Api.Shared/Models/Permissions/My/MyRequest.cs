using MediatR;

namespace Esquio.UI.Api.Shared.Models.Permissions.My
{
    public class MyRequest 
        : IRequest<MyResponse>
    {
        public string SubjectId { get; set; }
    }
}
