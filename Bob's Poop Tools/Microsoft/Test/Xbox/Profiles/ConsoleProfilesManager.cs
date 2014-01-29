namespace Microsoft.Test.Xbox.Profiles
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using XDevkit;

    public class ConsoleProfilesManager
    {
        public const XboxLiveCountry DefaultCountry = XboxLiveCountry.UnitedStates;
        public const SubscriptionTier DefaultSubscriptionTier = SubscriptionTier.Gold;
        private IProfileSupport profileSupport;
        public const string RandomGamertag = null;

        internal ConsoleProfilesManager(IXboxConsole console, IProfileSupport profileSupport)
        {
            this.Console = console;
            this.profileSupport = profileSupport;
        }

        public ConsoleProfile CreateConsoleProfile(bool online)
        {
            return this.CreateConsoleProfile(online, XboxLiveCountry.UnitedStates, SubscriptionTier.Gold, null);
        }

        public ConsoleProfile CreateConsoleProfile(bool online, SubscriptionTier tier)
        {
            return this.CreateConsoleProfile(online, XboxLiveCountry.UnitedStates, tier, null);
        }

        public ConsoleProfile CreateConsoleProfile(bool online, XboxLiveCountry country)
        {
            return this.CreateConsoleProfile(online, country, SubscriptionTier.Gold, null);
        }

        public ConsoleProfile CreateConsoleProfile(bool online, XboxLiveCountry country, SubscriptionTier tier)
        {
            return this.CreateConsoleProfile(online, country, tier, null);
        }

        public ConsoleProfile CreateConsoleProfile(bool online, XboxLiveCountry country, SubscriptionTier tier, string gamertag)
        {
            return this.profileSupport.CreateConsoleProfile(online, country, tier, gamertag);
        }

        public void DeleteAllConsoleProfiles()
        {
            this.profileSupport.DeleteAllConsoleProfiles();
        }

        public void DeleteConsoleProfile(ConsoleProfile profile)
        {
            this.profileSupport.DeleteConsoleProfile(profile.OfflineXuid);
        }

        public IEnumerable<ConsoleProfile> EnumerateConsoleProfiles()
        {
            return this.profileSupport.EnumerateConsoleProfiles();
        }

        public ConsoleProfile GetDefaultProfile()
        {
            return this.profileSupport.GetDefaultProfile();
        }

        public IEnumerable<ConsoleProfile> GetSignedInUsers()
        {
            return this.profileSupport.GetSignedInUsers();
        }

        public ConsoleProfile GetUserAtUserIndex(UserIndex userIndex)
        {
            return this.profileSupport.GetUserAtUserIndex(userIndex);
        }

        public void SetDefaultProfile(ConsoleProfile profile)
        {
            this.profileSupport.SetDefaultProfile(profile.OfflineXuid);
        }

        public void SignOutAllUsers()
        {
            this.profileSupport.SignOutAllUsers();
        }

        public void SignOutUserAtUserIndex(UserIndex userIndex)
        {
            this.profileSupport.SignOutUserAtUserIndex(userIndex);
        }

        public IXboxConsole Console { get; private set; }
    }
}

