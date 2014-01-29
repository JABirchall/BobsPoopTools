using System;
using System.Runtime.CompilerServices;

namespace Profiles
{
    public class Gamer : IEquatable<Gamer>
    {
        public Gamer(string gamertag, Xuid onlineXuid)
        {
            this.Gamertag = gamertag;
            this.OnlineXuid = onlineXuid;
        }

        public bool Equals(Gamer other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }
            return (object.ReferenceEquals(this, other) || ((this.OnlineXuid == other.OnlineXuid) && (this.Gamertag == other.Gamertag)));
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Gamer);
        }

        public override int GetHashCode()
        {
            return this.OnlineXuid.GetHashCode();
        }

        public static bool operator ==(Gamer leftHandGamer, Gamer rightHandGamer)
        {
            if (!object.ReferenceEquals(leftHandGamer, null))
            {
                return leftHandGamer.Equals(rightHandGamer);
            }
            return object.ReferenceEquals(rightHandGamer, null);
        }

        public static bool operator !=(Gamer leftHandGamer, Gamer rightHandGamer)
        {
            return !(leftHandGamer == rightHandGamer);
        }

        public override string ToString()
        {
            return this.Gamertag.ToString();
        }

        public string Gamertag { get; private set; }

        public bool IsLiveProfile
        {
            get
            {
                return this.OnlineXuid.IsValidXuid;
            }
        }

        public Xuid OnlineXuid { get; private set; }
    }
}

