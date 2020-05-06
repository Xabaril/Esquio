using System;
using System.Collections.Generic;
using System.Linq;

namespace Esquio.UI.Api.Infrastructure.Data.Entities
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

        public void AddOrUpdateParameter(DeploymentEntity currentRing, DeploymentEntity defaultDeployment, string parameterName, string value)
        {
            if (currentRing.ByDefault)
            {
                AddOrUpdateParameter(currentRing, parameterName, value);
            }
            else
            {
                if (!HasParameter(defaultDeployment, parameterName))
                {
                    AddOrUpdateParameter(defaultDeployment, parameterName, value);
                }

                AddOrUpdateParameter(currentRing, parameterName, value);
            }
        }

        void AddOrUpdateParameter(DeploymentEntity deployment, string parameterName, string value)
        {
            var parameter = this.Parameters
                .Where(p => p.Name == parameterName && p.DeploymentName == deployment.Name)
                .SingleOrDefault();

            if (parameter != null)
            {
                parameter.Value = value;
                parameter.DeploymentName = deployment.Name;
            }
            else
            {
                this.Parameters.Add(
                    new ParameterEntity(Id, deployment.Name, parameterName, value));
            }
        }

        bool HasParameter(DeploymentEntity deployment, string parameterName)
        {
            return this.Parameters
                .Where(p => p.Name == parameterName && p.DeploymentName == deployment.Name)
                .Any();
        }
    }
}
