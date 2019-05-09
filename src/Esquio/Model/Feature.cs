using Esquio.Abstractions;
using Esquio.Toggles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Esquio.Model
{
    public class Feature
    {
        private readonly List<Toggle> _toggles = new List<Toggle>();
        public string Name
        {
            get; private set;
        }
        public string Description
        {
            get; private set;
        }
        public bool IsEnabled
        {
            get; private set;
        }
        public bool IsPreview
        {
            get; private set;
        }
        public DateTime CreatedOn
        {
            get; private set;
        }
        public Feature(
            string name,
            string description = default,
            DateTime createdOn = default)
        {
            Ensure.Argument.NotNullOrEmpty(name, nameof(name));

            Name = name;
            Description = description;
            CreatedOn = createdOn == default ? DateTime.UtcNow : createdOn;
        }
        public Toggle GetToggle(string type)
        {
            return _toggles.SingleOrDefault(t => t.Type.StartsWith(type,StringComparison.InvariantCultureIgnoreCase));
        }
        public IEnumerable<Toggle> GetToggles()
        {
            return _toggles;
        }
        public void AddToggle(Toggle toggle)
        {
            Ensure.NotNull(toggle, nameof(toggle));

            if ((IsPreview && !_toggles.Any() && toggle.IsUserPreview())
                || !IsPreview)
            {
                _toggles.Add(toggle);
            }
            else
            {
                throw new InvalidOperationException($"Preview features only supports once {nameof(UserPreviewToggle)} toggle.");
            }   
        }
        public void AddToggles(IEnumerable<Toggle> toggles)
        {
            Ensure.NotNull(toggles, nameof(toggles));

            if(toggles.Any())
            {
                if (toggles.Count() == 1)
                {
                    AddToggle(toggles.Single());
                }
                else
                {
                    if (IsPreview)
                    {
                        throw new InvalidOperationException($"Preview features only supports once {nameof(UserPreviewToggle)} toggle.");
                    }
                    _toggles.AddRange(toggles);
                }
            }
        }
        public void Enabled()
        {
            IsEnabled = true;
        }
        public void Disabled()
        {
            IsEnabled = false;
        }
        public void MarkAsPreview()
        {
            IsPreview = true;
        }
    }
}
