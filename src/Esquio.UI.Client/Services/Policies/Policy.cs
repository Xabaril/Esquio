using System.Collections.Generic;
using System.Linq;

namespace Esquio.UI.Client.Services
{
    public interface IPolicy
    {
        bool Can(PolicyAction action, PolicySubject subject);
        void Allow(PolicyAction action, params PolicySubject[] subjects);
    }

    public class Policy : IPolicy
    {
        private readonly Dictionary<PolicyAction, List<PolicySubject>> policies;

        public Policy()
        {
            this.policies = new Dictionary<PolicyAction, List<PolicySubject>>();
        }

        public IReadOnlyDictionary<PolicyAction, List<PolicySubject>> Policies => policies;

        public void Allow(PolicyAction action, params PolicySubject[] subjects)
        {
            if (policies.ContainsKey(action))
            {
                policies[action] = subjects.ToList();
            }
            else
            {
                policies.Add(action, subjects.ToList());
            }
        }

        public bool Can(PolicyAction action, PolicySubject subject)
        {
            return policies.ContainsKey(action)
                ? policies[action].Any(s => s == subject)
                : false;
        }
    }
}
