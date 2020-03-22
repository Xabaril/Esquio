using System;
using System.Collections.Generic;

namespace Esquio.UI.Client.Services
{
    public class ActAs
    {
        private static readonly IDictionary<string, ActAs> _actAs = new Dictionary<string, ActAs>();

        public static readonly ActAs Reader = new ActAs(nameof(Reader));
        public static readonly ActAs Contributor = new ActAs(nameof(Contributor));
        public static readonly ActAs Management = new ActAs(nameof(Management));

        static ActAs() 
        {
            _actAs.Add(nameof(Reader), Reader);
            _actAs.Add(nameof(Contributor), Contributor);
            _actAs.Add(nameof(Management), Management);
        }

        private ActAs(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public static ActAs From(string name)
        {
            if (!_actAs.ContainsKey(name))
            {
                throw new ArgumentOutOfRangeException("Invalid act as");
            }

            return _actAs[name];
        }
    }
}
