using System;
using System.Drawing;
using NzbDrone.Common.EnvironmentInfo;

namespace NzbDrone.Common.Disk
{
    public static class GdiPlusInterop
    {
        private static Exception _gdiPlusException;

        static GdiPlusInterop()
        {
            Console.WriteLine("TestingLibrary");
            TestLibrary();
            Console.WriteLine("FinishedLibraryTest");
        }

        private static void TestLibrary()
        {
            if (OsInfo.IsWindows)
            {
                return;
            }

            try
            {
                Console.WriteLine("Trying StringFormat");
                // We use StringFormat as test coz it gets properly cleaned up by the finalizer even if gdiplus is absent and is relatively non-invasive.
                var strFormat = new StringFormat();
                Console.WriteLine("Disposing StringFormat");
                strFormat.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Setting Exception");
                _gdiPlusException = ex;
            }
        }

        public static void CheckGdiPlus()
        {
            if (_gdiPlusException != null)
            {
                Console.WriteLine("Throwing Exception");
                throw new DllNotFoundException("Couldn't load GDIPlus library", _gdiPlusException);
            }
        }
    }
}
