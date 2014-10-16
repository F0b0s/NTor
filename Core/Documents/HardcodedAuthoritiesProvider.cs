namespace Core.Documents
{
    public class HardcodedAuthoritiesProvider : IAuthoritiesProvider
    {
        private readonly NetworkEntity[] authorities = new[]
                                                           {
                                                               new NetworkEntity("moria1", "128.31.0.39", "9131"),
                                                               new NetworkEntity("tor26", "86.59.21.38", "80"),
                                                               new NetworkEntity("dizum", "194.109.206.212", "80"),
                                                               new NetworkEntity("turtles", "76.73.17.194", "9030"),
                                                               new NetworkEntity("gabelmoo", "212.112.245.170", "80"),
                                                               new NetworkEntity("dannenberg", "193.23.244.244", "80"),
                                                               new NetworkEntity("urras", "208.83.223.34", "443"),
                                                               new NetworkEntity("maatuska", "171.25.193.9", "443"),
                                                               new NetworkEntity("Faravahar", "154.35.32.5", "80")
                                                           };
    
        public NetworkEntity[] GetAuthorities()
        {
            return authorities;
        }
    }
}