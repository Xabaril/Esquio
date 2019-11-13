using MediatR;

namespace Esquio.UI.Api.Features.Flags.Details
{
    public class DetailsFlagRequest : IRequest<DetailsFlagResponse>
    {
        public string ProductName { get; set; }

        public string FeatureName { get; set; }
    }
}
