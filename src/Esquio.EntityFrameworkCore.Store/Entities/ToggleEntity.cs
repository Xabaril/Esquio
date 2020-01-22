using System;
using System.Collections.Generic;
using System.Linq;

namespace Esquio.EntityFrameworkCore.Store.Entities
{
    public sealed class ToggleEntity : IAuditable
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public int FeatureEntityId { get; set; }

        public FeatureEntity FeatureEntity { get; set; }

        public ICollection<ParameterEntity> Parameters { get; set; }

        public ToggleEntity(int featureEntityId, string type)
        {
            FeatureEntityId = featureEntityId;
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Parameters = new List<ParameterEntity>();
        }

        public void AddOrUpdateParameter(RingEntity currentRing, RingEntity defaultRing, string parameterName, string value)
        {
            if (currentRing.ByDefault)
            {
                AddOrUpdateParameter(currentRing, parameterName, value);
            }
            else
            {
                if (!HasParameter(defaultRing, parameterName))
                {
                    AddOrUpdateParameter(defaultRing, parameterName, value);
                }

                AddOrUpdateParameter(currentRing, parameterName, value);
            }
        }

        void AddOrUpdateParameter(RingEntity ring, string parameterName, string value)
        {
            var parameter = this.Parameters
                .Where(p => p.Name == parameterName && p.RingEntityId == ring.Id)
                .SingleOrDefault();

            if (parameter != null)
            {
                parameter.Value = value;
                parameter.RingEntityId = ring.Id;
            }
            else
            {
                this.Parameters.Add(
                    new ParameterEntity(Id, ring.Id, parameterName, value));
            }
        }

        bool HasParameter(RingEntity ring, string parameterName)
        {
            return this.Parameters
                .Where(p => p.Name == parameterName && p.RingEntityId == ring.Id)
                .Any();
        }
    }
}
