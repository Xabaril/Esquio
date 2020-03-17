using System;
using System.Collections.Generic;

namespace Esquio.UI.Client.Services
{
    public class ActAs
    {
        private static readonly IDictionary<string, ActAs> actas = new Dictionary<string, ActAs>();
        public static readonly ActAs Reader = new ActAs(nameof(Reader));
        public static readonly ActAs Contributor = new ActAs(nameof(Contributor));
        public static readonly ActAs Management = new ActAs(nameof(Management));

        protected ActAs() { }

        private ActAs(string name)
        {
            Name = name;
            actas.Add(name, this);
        }

        public string Name { get; private set; }

        public static ActAs From(string name)
        {
            if (!actas.ContainsKey(name)) throw new ArgumentOutOfRangeException("Invalid act as");
            return actas[name];
        }
    }
}
