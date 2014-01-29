namespace Microsoft.Test.Xbox.Profiles
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using XDevkit;

    public class PartyManager
    {
        public const int DefaultPartyTimeOutInSeconds = 10;
        private IPartySupport partySupport;

        internal PartyManager(IXboxConsole console, IPartySupport partySupport)
        {
            this.Console = console;
            this.partySupport = partySupport;
        }

        public void AddLocalUserToParty(ConsoleProfile profile)
        {
            this.partySupport.AddLocalUserToParty(profile.GetUserIndex());
        }

        public void CreateParty(ConsoleProfile leader)
        {
            this.partySupport.CreateParty(leader.GetUserIndex(), TimeSpan.FromSeconds(10.0));
        }

        public void CreateParty(ConsoleProfile leader, TimeSpan timeout)
        {
            this.partySupport.CreateParty(leader.GetUserIndex(), timeout);
        }

        public IEnumerable<PartyMember> GetPartyMembers()
        {
            return this.partySupport.GetPartyMembers();
        }

        public void JoinParty(ConsoleProfile profile, Gamer remoteGamer)
        {
            this.partySupport.JoinParty(profile.GetUserIndex(), remoteGamer.OnlineXuid);
        }

        public void KickUserFromParty(Gamer gamerToKick)
        {
            this.partySupport.KickUserFromParty(gamerToKick.OnlineXuid);
        }

        public void LeaveParty()
        {
            this.partySupport.LeaveParty();
        }

        public void RemoveLocalUserFromParty(ConsoleProfile profile)
        {
            this.partySupport.RemoveLocalUserFromParty(profile.GetUserIndex());
        }

        public IXboxConsole Console { get; private set; }
    }
}

