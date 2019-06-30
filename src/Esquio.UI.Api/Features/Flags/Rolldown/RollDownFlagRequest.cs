using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esquio.UI.Api.Features.Flags.Rolldown
{
    public class RolldownFlagRequest
     : IRequest
    {
        public int FeatureId { get; set; }
    }
}
