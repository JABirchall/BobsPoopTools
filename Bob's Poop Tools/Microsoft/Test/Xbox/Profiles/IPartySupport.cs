namespace Microsoft.Test.Xbox.Profiles
{
    using System;
    using System.Collections.Generic;

    internal interface IPartySupport
    {
        void AddLocalUserToParty(UserIndex userIndex);
        void CreateParty(UserIndex leaderUserIndex, TimeSpan timeout);
        IEnumerable<PartyMember> GetPartyMembers();
        void JoinParty(UserIndex userIndex, Xuid xuidContact);
        void KickUserFromParty(Xuid xuidToKick);
        void LeaveParty();
        void RemoveLocalUserFromParty(UserIndex userIndex);
    }
}

