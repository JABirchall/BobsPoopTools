namespace Microsoft.Test.Xbox.Profiles
{
    using System;
    using System.Collections.Generic;

    internal interface IProfileSupport
    {
        ConsoleProfile CreateChildProfile(DateTime birthDate, DateTime parentBirthDate, XboxLiveCountry country, SubscriptionTier tier, string gamertag, string parentGamertag);
        ConsoleProfile CreateConsoleProfile(bool online, XboxLiveCountry country, SubscriptionTier tier, string Gamertag);
        void DeleteAllConsoleProfiles();
        void DeleteConsoleProfile(Xuid offlineXuid);
        IEnumerable<ConsoleProfile> EnumerateConsoleProfiles();
        ConsoleProfile GetDefaultProfile();
        IEnumerable<ConsoleProfile> GetSignedInUsers();
        ConsoleProfile GetUserAtUserIndex(UserIndex userIndex);
        UserIndex GetUserIndex(Xuid offlineXuid);
        SignInState GetUserSigninState(Xuid offlineXuid);
        void SetDefaultProfile(Xuid offlineXuid);
        void SignInUserAtUserIndex(Xuid offlineXuid, UserIndex userIndex);
        void SignOutAllUsers();
        void SignOutUser(Xuid offlineXuid);
        void SignOutUserAtUserIndex(UserIndex userIndex);
    }
}

