using Esquio.UI.Api.Shared.Models.Users.My;

namespace Esquio.UI.Client.Services
{
    public interface IPolicyBuilder
    {
        IPolicy Build(MyResponse my);
    }

    public class DefaultPolicyBuilder : IPolicyBuilder
    {
        private readonly PolicySubject[] subjects;

        public DefaultPolicyBuilder()
        {
            this.subjects = new PolicySubject[]
            {
                PolicySubject.Product,
                PolicySubject.Feature,
                PolicySubject.Toggle,
                PolicySubject.Ring,
                PolicySubject.Token
            };
        }

        public IPolicy Build(MyResponse my)
        {
            var policy = new Policy();
            var actAs = ActAs.From(my.ActAs);

            if (actAs == ActAs.Reader || actAs == ActAs.Contributor || actAs == ActAs.Management)
            {
                policy.Allow(PolicyAction.Read, subjects);
            }

            if (actAs == ActAs.Contributor || actAs == ActAs.Management)
            {
                policy.Allow(PolicyAction.Create, subjects);
                policy.Allow(PolicyAction.Update, subjects);
                policy.Allow(PolicyAction.Delete, subjects);
            }

            if (actAs == ActAs.Management)
            {
                policy.Allow(PolicyAction.Manage, PolicySubject.Permission);
            }

            return policy;
        }
    }
}
