using System;
using System.Collections.Generic;
using System.Linq;
using Esquio.Abstractions;
using Esquio.Toggles;

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
            string description,
            DateTime createdOn = default,
            bool enabled = false,
            bool isPreview = false)
        {
            Ensure.Argument.NotNullOrEmpty(name, nameof(name));

            Name = name;
            Description = description;
            CreatedOn = createdOn == default ? DateTime.UtcNow : createdOn;
            IsEnabled = enabled;
            IsPreview = isPreview;
        }
        public Toggle GetToggle(string type)
        {
            return _toggles.SingleOrDefault(t => t.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase));
        }
        public IEnumerable<Toggle> GetToggles()
        {
            return _toggles;
        }
        public void AddToggle(Toggle toggle)
        {
            Ensure.NotNull(toggle, nameof(toggle));

            if (IsPreview && !toggle.IsUserPreview())
            {
                throw new InvalidOperationException($"Preview features only supports {nameof(UserPreviewToggle)}");
            }

            _toggles.Add(toggle);
        }
        public void AddToggles(IEnumerable<Toggle> toggles)
        {
            Ensure.NotNull(toggles, nameof(toggles));

            if (IsPreview && (toggles.Count() > 1 || !toggles.First().IsUserPreview()))
            {
                throw new InvalidOperationException($"Preview features only supports one {nameof(UserPreviewToggle)}");
            }

            _toggles.AddRange(toggles);
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
        public void UnmarkAsPreview()
        {
            IsPreview = false;
        }
    }
}
