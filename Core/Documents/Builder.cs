using System;
using System.Linq;

namespace Core.Documents
{
    public class Builder
    {
        public void Build(IAuthoritiesProvider provider, IRouterDescriptorLoader loader)
        {
            var authories = provider.GetAuthorities();

            if(authories == null || authories.Length == 0)
            {
                throw new InvalidOperationException("Authority provider doesn't provide any authority address.");
            }

            var authority = authories.First();
            var data = loader.GetroutersDescriptions(authority);
            var parser = new RouterDescriptorParser();
            var routerDescriptions = parser.GetDescriptors(data);


        }
    }
}
