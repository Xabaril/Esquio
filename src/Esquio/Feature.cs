using System;
using System.Collections.Generic;
using System.Linq;
using Esquio.Abstractions;
using Esquio.Toggles;

namespace Esquio
{
    public class Feature
    {
        private readonly List<IToggle> _toggles = new List<IToggle>();
        public string Name
        {
            get; private set;
        }
        public string Description
        {
            get; private set;
        }
        public bool Enabled
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
            Ensure.Argument.IsNot(string.IsNullOrWhiteSpace(name), nameof(name));
            Ensure.Argument.IsNot(string.IsNullOrWhiteSpace(description), nameof(description));

            Name = name;
            Description = description;
            CreatedOn = createdOn == default ? DateTime.UtcNow : createdOn;
            Enabled = enabled;
            IsPreview = isPreview;
        }
        public IEnumerable<IToggle> GetToggles()
        {
            return _toggles;
        }
        public void AddToggle(IToggle toggle)
        {
            Ensure.NotNull(toggle, nameof(toggle));

            if (IsPreview && !toggle.IsUserPreview())
            {
                throw new InvalidOperationException($"Preview features only supports {nameof(UserPreviewToggle)}");
            }

            _toggles.Add(toggle);
        }
        public void AddToggles(IEnumerable<IToggle> toggles)
        {
            Ensure.NotNull(toggles, nameof(toggles));

            if (IsPreview && (toggles.Count() > 1 || !toggles.First().IsUserPreview()))
            {
                throw new InvalidOperationException($"Preview features only supports one {nameof(UserPreviewToggle)}");
            }

            _toggles.AddRange(toggles);
        }
    }
}
