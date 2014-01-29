namespace Microsoft.Test.Xbox.Profiles
{
    using System;
    using System.Runtime.CompilerServices;

    public class PartyMember : Gamer
    {
        internal PartyMember(string gamertag, Xuid onlineXuid, bool isLocal, Microsoft.Test.Xbox.Profiles.UserIndex userIndex) : base(gamertag, onlineXuid)
        {
            this.IsLocal = isLocal;
            this.UserIndex = userIndex;
        }

        public bool IsLocal { get; private set; }

        public Microsoft.Test.Xbox.Profiles.UserIndex UserIndex { get; private set; }
    }
}

