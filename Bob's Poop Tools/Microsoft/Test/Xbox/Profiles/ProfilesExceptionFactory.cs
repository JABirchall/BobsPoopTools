namespace Microsoft.Test.Xbox.Profiles
{
    using System;

    public class ProfilesExceptionFactory
    {
        //public static ProfilesException CreateExceptionFromErrorCode(uint errorCode)
        //{
        //    switch (errorCode)
        //    {
        //        case 2:
        //        case 0x80070525:
        //            return new NoSuchUserException();

        //        case 0x80151802:
        //        case 0x80151903:
        //        case 0x80151906:
        //        case 0x80151907:
        //            return new ConnectionToLiveFailedException(errorCode);

        //        case 0x80155209:
        //            return new UserNotConnectedToLiveException();

        //        case 0x807d0003:
        //            return new NotInPartyException();

        //        case 0x80154000:
        //            return new AccountNameTakenException();
        //    }
        //    return new ProfilesException("Operation failed. The error code returned by the console was 0x{0:x}", errorCode);
        //}

        //public static ProfilesException CreatePartyExceptionFromErrorCode(PartyErrorCodes errorCode)
        //{
        //    //switch (errorCode)
        //    //{
        //    //    case PartyErrorCodes.XPARTY_ERROR_CONNECTFAILED:
        //    //        return new PartyConnectFailedException();

        //    //    case PartyErrorCodes.XPARTY_ERROR_CONNECTIONLOST:
        //    //        return new PartyConnectionLostException();

        //    //    case PartyErrorCodes.XPARTY_ERROR_PARTYFULL:
        //    //        return new PartyFullException();

        //    //    case PartyErrorCodes.XPARTY_ERROR_PARTYINVITEONLY:
        //    //        return new PartyInviteOnlyException();

        //    //    case PartyErrorCodes.XPARTY_ERROR_PARTYFRIENDSONLY:
        //    //        return new PartyFriendsOnlyException();
        //    //}
        //    return new ProfilesException("Operation failed. The error code returned by the console was 0x{0:x}", new object[] { errorCode });
        //}
    }
}

