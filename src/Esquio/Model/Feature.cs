using System;
using System.Collections.Generic;
using System.Linq;

namespace Esquio.Model
{
    /// <summary>
    /// Represent the internal model for represent Features on Esquio library. Typically
    /// this class is used for configuration providers to mapping provider model to Esquio.
    /// </summary>
    public class Feature
    {
        private readonly List<Toggle> _toggles = new List<Toggle>();

        /// <summary>
        /// Represent the name of the feature.
        /// </summary>
        public string Name
        {
            get; private set;
        }

        /// <summary>
        /// Represent the Enable status.
        /// </summary>
        public bool IsEnabled
        {
            get; private set;
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="name">The feature name.</param>
        public Feature(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Get configured <see cref="Toggle"/> on this feature.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Toggle GetToggle(string type)
        {
            return _toggles.SingleOrDefault(t => t.Type.StartsWith(type, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Get all <see cref="Toggle"/> configured on this feature.
        /// </summary>
        /// <returns>The enumerable collection of configured <see cref="Toggle"/>.</returns>
        public IEnumerable<Toggle> GetToggles()
        {
            return _toggles;
        }

        /// <summary>
        /// Add a new <see cref="Toggle"/> on this feature.
        /// </summary>
        /// <param name="toggle">The <see cref="Toggle"/> to be added.</param>
        public void AddToggle(Toggle toggle)
        {
            if (toggle == null)
            {
                throw new ArgumentNullException(nameof(toggle));
            }

            _toggles.Add(toggle);
        }

        /// <summary>
        /// Add a collection of <see cref="Toggle"/> on this feature.
        /// </summary>
        /// <param name="toggles">The collection of <see cref="Toggle"/> to be added.</param>
        public void AddToggles(IEnumerable<Toggle> toggles)
        {
            if (toggles == null && toggles.Any())
            {
                _toggles.AddRange(toggles);
            }
        }
        /// <summary>
        /// Set this feature as enabled.
        /// </summary>
        public void Enabled()
        {
            IsEnabled = true;
        }

        /// <summary>
        /// Set this feature as disabled.
        /// </summary>
        public void Disabled()
        {
            IsEnabled = false;
        }
    }
}
