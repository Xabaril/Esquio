using System;
using System.Collections.Generic;
using System.Text;

namespace Esquio.UI.Api.Infrastructure.Data.Entities
{
    public sealed class FeatureStateEntity
    {
        public int FeatureEntityId { get; set; }

        public FeatureEntity FeatureEntity { get; set; }

        public int RingEntityId { get; set; }

        public RingEntity RingEntity { get; set; }

        public bool Enabled { get; set; }
    }
}
