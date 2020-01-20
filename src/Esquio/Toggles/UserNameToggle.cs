using Esquio.Abstractions;
using Esquio.Abstractions.Providers;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Esquio.Toggles
{
    /// <summary>
    /// A binary <see cref="IToggle"/> that is active depending on the current User name and if this is contained in configured Users property.
    /// </summary>
    [DesignType(Description = "Toggle that is active depending on current user name.", FriendlyName = "Identity Username")]
    [DesignTypeParameter(ParameterName = Users, ParameterType = EsquioConstants.SEMICOLON_LIST_PARAMETER_TYPE, ParameterDescription = "The collection of user(s) to activate this toggle separated by ';' character")]
    public class UserNameToggle
        : IToggle
    {
        internal const string Users = nameof(Users);

        private readonly IUserNameProviderService _userNameProviderService;

        /// <summary>
        /// Create a new instance of <see cref="UserNameToggle"/>.
        /// </summary>
        /// <param name="userNameProviderService">The <see cref="IUserNameProviderService"/> service to be used.</param>
        public UserNameToggle(IUserNameProviderService userNameProviderService)
        {
            _userNameProviderService = userNameProviderService ?? throw new ArgumentNullException(nameof(userNameProviderService));
        }

        ///<inheritdoc/>
        public async Task<bool> IsActiveAsync(ToggleExecutionContext context, CancellationToken cancellationToken = default)
        {
            var currentUserName = await _userNameProviderService
                .GetCurrentUserNameAsync();

            if (currentUserName != null)
            {
                string activeUserNames = context.Data[Users]?.ToString();

                if (activeUserNames != null)
                {
                    var tokenizer = new StringTokenizer(activeUserNames, EsquioConstants.DEFAULT_SPLIT_SEPARATOR);

                    return tokenizer.Contains(
                        currentUserName, StringSegmentComparer.OrdinalIgnoreCase);
                }
            }

            return false;
        }
    }
}