using System;

namespace Profiles
{
    public class Xuid : IEquatable<Xuid>
    {
        public ulong value;

        private Xuid(ulong value)
        {
            this.value = value;
        }

        public bool Equals(Xuid other)
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
            return (this.value == other.value);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Xuid);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        public static bool operator ==(Xuid leftHandXuid, Xuid rightHandXuid)
        {
            if (!object.ReferenceEquals(leftHandXuid, null))
            {
                return leftHandXuid.Equals(rightHandXuid);
            }
            return object.ReferenceEquals(rightHandXuid, null);
        }

        public static implicit operator ulong(Xuid xuid)
        {
            return xuid.value;
        }

        public static implicit operator Xuid(ulong value)
        {
            return new Xuid(value);
        }

        public static bool operator !=(Xuid leftHandXuid, Xuid rightHandXuid)
        {
            return !(leftHandXuid == rightHandXuid);
        }

        public override string ToString()
        {
            return string.Format("Xuid:{0:X16}", this.value);
        }

        public bool IsOfflineXuid
        {
            get
            {
                return ((this.value & 17293822569102704640L) == 16140901064495857664L);
            }
        }

        public bool IsOnlineXuid
        {
            get
            {
                return ((this.value & 18446462598732840960L) == 0x9000000000000L);
            }
        }

        public bool IsValidXuid
        {
            get
            {
                if (!this.IsOfflineXuid)
                {
                    return this.IsOnlineXuid;
                }
                return true;
            }
        }
    }
}

