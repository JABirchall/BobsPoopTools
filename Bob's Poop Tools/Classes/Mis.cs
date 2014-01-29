using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.Win32.SafeHandles;

namespace EatonWorks.Auxiliary.Miscellaneous
{
    ///Native C# class
    ///Created by Eaton
    ///Usage permitted in open or closed source environments.

    public static class Native
    {
        #region Raw Imports

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);

        [DllImport("shell32.dll", SetLastError = true)]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int DestroyIcon(IntPtr hIcon);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeFileHandle CreateFile(string lpFileName, DesiredAccesses dwDesiredAccess, ShareModes dwShareMode, IntPtr lpSecurityAttributes, CreationDispositions dwCreationDisposition, FlagsAndAttributes dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetFileValidData(SafeFileHandle hFile, long ValidDataLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetEndOfFile(SafeFileHandle hFile);

        public struct FILE_END_OF_FILE_INFO
        {
            public long Size { get; set; }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetFileInformationByHandle(SafeFileHandle hFile, uint FileInformationClass, ref FILE_END_OF_FILE_INFO lpFileInformation, uint dwBufferSize);

        public enum EMoveMethods : uint
        {
            Begin = 0,
            Current = 1,
            End = 2
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetFilePointerEx(SafeFileHandle hFile, long liDistanceToMove, out long lpNewFilePointer, EMoveMethods dwMoveMethod);

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID
        {
            private uint lp;
            private int hp;

            public uint LowPart
            {
                get { return lp; }
                set { lp = value; }
            }

            public int HighPart
            {
                get { return hp; }
                set { hp = value; }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LUID_AND_ATTRIBUTES
        {
            private LUID luid;
            private uint attributes;

            public LUID LUID
            {
                get { return luid; }
                set { luid = value; }
            }

            public uint Attributes
            {
                get { return attributes; }
                set { attributes = value; }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TOKEN_PRIVILEGES
        {
            private uint prvct;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            private LUID_AND_ATTRIBUTES[] privileges;

            public uint PrivilegeCount
            {
                get { return prvct; }
                set { prvct = value; }
            }

            public LUID_AND_ATTRIBUTES[] Privileges
            {
                get { return privileges; }
                set { privileges = value; }
            }
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle, TokenAccessLevels DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, uint Bufferlength, IntPtr PreviousState, IntPtr ReturnLength);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out LUID lpLuid);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, ref IntPtr lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, ref long lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, ref DISK_GEOMETRY lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);

        #endregion

        #region Other

        public static void EnableDisablePrivilege(string PrivilegeName, bool EnableDisable)
        {
            var htok = IntPtr.Zero;
            if (!OpenProcessToken(Process.GetCurrentProcess().Handle, TokenAccessLevels.AdjustPrivileges | TokenAccessLevels.Query, out htok))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                return;
            }
            var tkp = new TOKEN_PRIVILEGES { PrivilegeCount = 1, Privileges = new LUID_AND_ATTRIBUTES[1] };
            LUID luid;
            if (!LookupPrivilegeValue(null, PrivilegeName, out luid))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                return;
            }
            tkp.Privileges[0].LUID = luid;
            tkp.Privileges[0].Attributes = (uint)(EnableDisable ? 2 : 0);
            if (!AdjustTokenPrivileges(htok, false, ref tkp, 0, IntPtr.Zero, IntPtr.Zero))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                return;
            }
        }

        #endregion

        #region Icons and File Info

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            private IntPtr hIcon;
            private readonly int iIcon;
            private readonly uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            private string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            private string szTypeName;

            public IntPtr IconHandle
            {
                get { return hIcon; }
                set { hIcon = value; }
            }

            public string DisplayName
            {
                get { return szDisplayName; }
                set
                {
                    if (value.Length > 260) throw new Exception("Native.SHFILEINFO: Invalid display name specified. It must be less than or equal to 260 characters in length.");
                    szDisplayName = value;
                }
            }

            public string TypeName
            {
                get { return szTypeName; }
                set
                {
                    if (value.Length > 80) throw new Exception("Native.SHFILEINFO: Invalid type name specified. It must be less than or equal to 80 characters in length.");
                    szTypeName = value;
                }
            }
        };

        [Flags]
        public enum SHGFIFlags : uint
        {
            SHGFI_ICON = 0x000000100,
            SHGFI_DISPLAYNAME = 0x000000200,
            SHGFI_TYPENAME = 0x000000400,
            SHGFI_ATTRIBUTES = 0x000000800,
            SHGFI_ICONLOCATION = 0x000001000,
            SHGFI_EXETYPE = 0x000002000,
            SHGFI_SYSICONINDEX = 0x000004000,
            SHGFI_LINKOVERLAY = 0x000008000,
            SHGFI_SELECTED = 0x000010000,
            SHGFI_ATTR_SPECIFIED = 0x000020000,
            SHGFI_LARGEICON = 0x000000000,
            SHGFI_SMALLICON = 0x000000001,
            SHGFI_OPENICON = 0x000000002,
            SHGFI_SHELLICONSIZE = 0x000000004,
            SHGFI_PIDL = 0x000000008,
            SHGFI_USEFILEATTRIBUTES = 0x000000010,
            SHGFI_ADDOVERLAYS = 0x000000020,
            SHGFI_OVERLAYINDEX = 0x000000040
        }

        public static SHFILEINFO GetFileInfoFromLocation(string FileLocation, SHGFIFlags Flags)
        {
            var shinfo = new SHFILEINFO();
            SHGetFileInfo(FileLocation, (uint)FlagsAndAttributes.NORMAL, ref shinfo, (uint)Marshal.SizeOf(shinfo), (uint)Flags);
            return shinfo;
        }

        public static SHFILEINFO GetFileInfoFromExtension(string FileExtension, SHGFIFlags Flags)
        {
            FileExtension = "*" + FileExtension;
            var shinfo = new SHFILEINFO();
            SHGetFileInfo(FileExtension, (uint)FlagsAndAttributes.NORMAL, ref shinfo, (uint)Marshal.SizeOf(shinfo), (uint)Flags | (uint)SHGFIFlags.SHGFI_USEFILEATTRIBUTES);
            return shinfo;
        }

        #endregion

        #region Devices and File Management

        [Flags]
        public enum DesiredAccesses : uint
        {
            GENERIC_READ = 0x80000000,
            GENERIC_WRITE = 0x40000000,
            GENERIC_EXECUTE = 0x20000000,
            GENERIC_ALL = 0x10000000,

        }

        [Flags]
        public enum ShareModes : uint
        {
            NONE = 0,
            READ = 1,
            WRITE = 2,
            DELETE = 4
        }

        public enum CreationDispositions : uint
        {
            CREATE_NEW = 1,
            CREATE_ALWAYS = 2,
            OPEN_EXISTING = 3,
            OPEN_ALWAYS = 4,
            TRUNCATE_EXISTING = 5
        }

        [Flags]
        public enum FlagsAndAttributes : uint
        {
            READONLY = 0x00000001,
            HIDDEN = 0x00000002,
            SYSTEM = 0x00000004,
            DIRECTORY = 0x00000010,
            ARCHIVE = 0x00000020,
            DEVICE = 0x00000040,
            NORMAL = 0x00000080,
            TEMPORARY = 0x00000100,
            SPARSE_FILE = 0x00000200,
            REPARSE_POINT = 0x00000400,
            COMPRESSED = 0x00000800,
            OFFLINE = 0x00001000,
            NOT_CONTENT_INDEXED = 0x00002000,
            ENCRYPTED = 0x00004000,
            WRITE_THROUGH = 0x80000000,
            OVERLAPPED = 0x40000000,
            NO_BUFFERING = 0x20000000,
            RANDOM_ACCESS = 0x10000000,
            SEQUENTIAL_SCAN = 0x08000000,
            DELETE_ON_CLOSE = 0x04000000,
            BACKUP_SEMANTICS = 0x02000000,
            POSIX_SEMANTICS = 0x01000000,
            OPEN_REPARSE_POINT = 0x00200000,
            OPEN_NO_RECALL = 0x00100000,
            FIRST_PIPE_INSTANCE = 0x00080000,
        }

        public enum MediaTypes : byte
        {
            Unknown = 0x00,
            F5_1Pt2_512 = 0x01,
            F3_1Pt44_512 = 0x02,
            F3_2Pt88_512 = 0x03,
            F3_20Pt8_512 = 0x04,
            F3_720_512 = 0x05,
            F5_360_512 = 0x06,
            F5_320_512 = 0x07,
            F5_320_1024 = 0x08,
            F5_180_512 = 0x09,
            F5_160_512 = 0x0A,
            RemovableMedia = 0x0B,
            FixedMedia = 0x0C,
            F3_120M_512 = 0x0D,
            F3_640_512 = 0x0E,
            F5_640_512 = 0x0F,
            F5_720_512 = 0x10,
            F3_1Pt2_512 = 0x11,
            F3_1Pt23_1024 = 0x12,
            F5_1Pt23_1024 = 0x13,
            F3_128Mb_512 = 0x14,
            F3_230Mb_512 = 0x15,
            F8_256_128 = 0x16,
            F3_200Mb_512 = 0x17,
            F3_240M_512 = 0x18,
            F3_32M_512 = 0x19
        }

        private enum IOCTL_CONTROL_CODE_CONSTANTS : uint
        {
            IOCTL_DISK_GET_DRIVE_GEOMETRY = 0x70000,
            IOCTL_DISK_GET_LENGTH_INFO = 0x0007405C
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DISK_GEOMETRY
        {
            public long cylinders;
            public byte mediaType;
            public uint tracksPerCylinder;
            public uint sectorsPerTrack;
            public uint bytesPerSector;

            public long Cylinders
            {
                get { return cylinders; }
                set { cylinders = value; }
            }

            public MediaTypes MediaType
            {
                get { return (MediaTypes)mediaType; }
                set
                {
                    if (!Enum.IsDefined(typeof(MediaTypes), value)) throw new Exception("Native.DISK_GEOMETRY: Invalid media type specified.");
                    mediaType = (byte)value;
                }
            }

            public uint TracksPerCylinder
            {
                get { return tracksPerCylinder; }
                set { tracksPerCylinder = value; }
            }

            public uint SectorsPerTrack
            {
                get { return sectorsPerTrack; }
                set { sectorsPerTrack = value; }
            }

            public uint BytesPerSector
            {
                get { return bytesPerSector; }
                set { bytesPerSector = value; }
            }

            public long DiskSize { get { return Cylinders * TracksPerCylinder * SectorsPerTrack * BytesPerSector; } }
        }

        public static DISK_GEOMETRY GetGeometry(SafeFileHandle Handle)
        {
            if (Handle.IsInvalid || Handle.IsClosed) throw new Exception("Native.GetGeometry: Invalid handle specified. It is closed or invalid.");
            var geom = new DISK_GEOMETRY();
            uint returnedBytes;
            if (DeviceIoControl(Handle.DangerousGetHandle(), (uint)IOCTL_CONTROL_CODE_CONSTANTS.IOCTL_DISK_GET_DRIVE_GEOMETRY, IntPtr.Zero, 0, ref geom, (uint)Marshal.SizeOf(typeof(DISK_GEOMETRY)), out returnedBytes, IntPtr.Zero)) return geom;
            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            return new DISK_GEOMETRY();
        }

        public static long GetDiskSize(SafeFileHandle Handle)
        {
            if (Handle.IsInvalid || Handle.IsClosed) throw new Exception("Native.GetDiskSize: Invalid handle specified. It is closed or invalid.");
            long size = 0;
            uint returnedBytes;
            if (DeviceIoControl(Handle.DangerousGetHandle(), (uint)IOCTL_CONTROL_CODE_CONSTANTS.IOCTL_DISK_GET_LENGTH_INFO, IntPtr.Zero, 0, ref size, 8, out returnedBytes, IntPtr.Zero)) return size;
            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            return 0;
        }

        #endregion
    }
}
