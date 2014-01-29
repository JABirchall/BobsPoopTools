namespace Microsoft.Test.Xbox.Profiles
{
    using System;

    internal enum CommonErrorCodes : uint
    {
        E_PENDING = 0x8000000a,
        ERROR_FILE_NOT_FOUND = 2,
        ERROR_FUNCTION_FAILED = 0x8007065b,
        ERROR_NO_MORE_FILES = 0x12,
        ERROR_NO_SUCH_USER = 0x80070525,
        ERROR_SUCCESS = 0,
        XPARTY_E_NOT_IN_PARTY = 0x807d0003
    }
}

