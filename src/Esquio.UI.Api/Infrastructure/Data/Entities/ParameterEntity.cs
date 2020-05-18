using System;

namespace Esquio.UI.Api.Infrastructure.Data.Entities
{
    public sealed class ParameterEntity : IAuditable
    {
        public int Id { get; set; }

        public int ToggleEntityId { get; set; }

        public ToggleEntity ToggleEntity { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public string DeploymentName { get; set; }


        public ParameterEntity(int toggleEntityId, string deploymentName,string name, string value)
        {
            ToggleEntityId = toggleEntityId;
            DeploymentName = deploymentName ?? throw new ArgumentNullException(nameof(deploymentName));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
