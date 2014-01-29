namespace Suave.Classes
{
    using System;

    public class TimeHelper
    {
        public static uint SecondsToDays(uint seconds)
        {
            return (seconds / 0x15180);
        }

        public static uint SecondsToHours(uint seconds)
        {
            uint num = seconds - (((SecondsToDays(seconds) * 0x18) * 60) * 60);
            return (num / 0xe10);
        }

        public static uint SecondsToMinutes(uint seconds)
        {
            uint num = seconds - (((SecondsToDays(seconds) * 0x18) * 60) * 60);
            num -= (SecondsToHours(seconds) * 60) * 60;
            return (num / 60);
        }

        public static uint TimeToSeconds(int days, int hours, int minutes)
        {
            return (uint) (((((days * 0x18) * 60) * 60) + ((hours * 60) * 60)) + (minutes * 60));
        }
    }
}

