using System.Collections.Generic;
using System.Linq;

namespace Esquio.UI.Client.Services
{
    public enum PolicySubject
    {
        Product,
        Feature,
        Toggle,
        Ring,
        Permission,
        ApiKeys,
        Audit
    }

    public enum PolicyAction
    {
        Read,
        Modify
    }

    public class Policy
    {
        public Dictionary<PolicyAction, List<PolicySubject>> Policies
        {
            get;
            private set;
        }

        public Policy()
        {
            Policies = new Dictionary<PolicyAction, List<PolicySubject>>();
        }

        public void Allow(PolicyAction action, params PolicySubject[] subjects)
        {
            if (Policies.ContainsKey(action))
            {
                Policies[action] = subjects.ToList();
            }
            else
            {
                Policies.Add(action, subjects.ToList());
            }
        }

        public bool Can(PolicyAction action, PolicySubject subject)
        {
            var allowed = Policies.ContainsKey(action)
                ? Policies[action].Any(s => s == subject)
                : false;

            return allowed;
        }
    }
}
