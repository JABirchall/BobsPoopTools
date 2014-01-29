namespace Microsoft.Test.Xbox.Profiles
{
    using System;
    using System.Collections.Generic;

    internal interface IFriendsSupport
    {
        void AcceptFriendRequest(Xuid xuid, Xuid xuidFrom);
        IEnumerable<Friend> EnumerateFriends(UserIndex userIndex);
        void RejectFriendRequest(Xuid xuid, Xuid xuidFrom, bool block);
        void RemoveFriend(Xuid xuid, Xuid friendXuid);
        void SendFriendRequest(Xuid xuidFrom, Xuid xuidTo);
    }
}

