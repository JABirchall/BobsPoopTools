namespace Microsoft.Test.Xbox.Profiles
{
    using System;
    using System.Runtime.CompilerServices;
    using XDevkit;

    public class ConsoleProfile : Gamer
    {
        private IProfileSupport profileSupport;

        internal ConsoleProfile(string gamertag, Xuid onlineXuid, Xuid offlineXuid, SubscriptionTier tier, XboxLiveCountry country, IXboxConsole console, IProfileSupport profileSupport) : base(gamertag, onlineXuid)
        {
            this.OfflineXuid = offlineXuid;
            this.Tier = tier;
            this.Country = country;
            this.Console = console;
            this.profileSupport = profileSupport;
            //this.Friends = FriendsManagerFactory.CreateFriendsManager(this);
        }

        public bool Equals(ConsoleProfile other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }
            if (base.GetType() != other.GetType())
            {
                return false;
            }
            return ((this.OfflineXuid == other.OfflineXuid) && (base.Gamertag == other.Gamertag));
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ConsoleProfile);
        }

        public override int GetHashCode()
        {
            return this.OfflineXuid.GetHashCode();
        }

        public UserIndex GetUserIndex()
        {
            return this.profileSupport.GetUserIndex(this.OfflineXuid);
        }

        public SignInState GetUserSigninState()
        {
            return this.profileSupport.GetUserSigninState(this.OfflineXuid);
        }

        public void SignIn(UserIndex userIndex)
        {
            this.profileSupport.SignInUserAtUserIndex(this.OfflineXuid, userIndex);
        }

        public void SignOut()
        {
            this.profileSupport.SignOutUser(this.OfflineXuid);
        }

        public IXboxConsole Console { get; private set; }

        public XboxLiveCountry Country { get; private set; }

        public FriendsManager Friends { get; private set; }

        public Xuid OfflineXuid { get; private set; }

        public SubscriptionTier Tier { get; private set; }
    }
}

