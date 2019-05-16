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
        public bool IsEnabled
        {
            get; private set;
        }
        public bool IsUserPreview
        {
            get; private set;
        }

        public Feature(string name)
        {
            Ensure.Argument.NotNullOrEmpty(name, nameof(name));

            Name = name;
        }
        public Toggle GetToggle(string type)
        {
            return _toggles.SingleOrDefault(t => t.Type.StartsWith(type, StringComparison.InvariantCultureIgnoreCase));
        }
        public IEnumerable<Toggle> GetToggles()
        {
            return _toggles;
        }
        public void AddToggle(Toggle toggle)
        {
            Ensure.Argument.NotNull(toggle, nameof(toggle));

            _toggles.Add(toggle);
        }
        public void AddToggles(IEnumerable<Toggle> toggles)
        {
            Ensure.Argument.NotNull(toggles, nameof(toggles));

            if (toggles.Any())
            {
                _toggles.AddRange(toggles);
            }
        }
        public void RemoveToggle(Toggle toggle)
        {
            Ensure.Argument.NotNull(toggle);

            _toggles.Remove(toggle);
        }
        public void RemoveToggles()
        {
            _toggles.Clear();
        }
        public void Enabled()
        {
            IsEnabled = true;
        }
        public void Disabled()
        {
            IsEnabled = false;
        }
    }
}
