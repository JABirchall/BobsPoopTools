namespace Microsoft.Test.Xbox.Profiles
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class FriendsManager
    {
        private IFriendsSupport friendsSupport;

        internal FriendsManager(Microsoft.Test.Xbox.Profiles.ConsoleProfile profile, IFriendsSupport friendsSupport)
        {
            this.ConsoleProfile = profile;
            this.friendsSupport = friendsSupport;
        }

        public void AcceptFriendRequest(Gamer fromGamer)
        {
            this.friendsSupport.AcceptFriendRequest(this.ConsoleProfile.OnlineXuid, fromGamer.OnlineXuid);
        }

        public IEnumerable<Friend> EnumerateFriends()
        {
            return this.friendsSupport.EnumerateFriends(this.ConsoleProfile.GetUserIndex());
        }

        public void RejectFriendRequest(Gamer fromGamer, bool block)
        {
            this.friendsSupport.RejectFriendRequest(this.ConsoleProfile.OnlineXuid, fromGamer.OnlineXuid, block);
        }

        public void RemoveFriend(Gamer friend)
        {
            this.friendsSupport.RemoveFriend(this.ConsoleProfile.OnlineXuid, friend.OnlineXuid);
        }

        public void SendFriendRequest(Gamer toGamer)
        {
            this.friendsSupport.SendFriendRequest(this.ConsoleProfile.OnlineXuid, toGamer.OnlineXuid);
        }

        public Microsoft.Test.Xbox.Profiles.ConsoleProfile ConsoleProfile { get; private set; }
    }
}

