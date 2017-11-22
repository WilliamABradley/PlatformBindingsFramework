using Android.Content;
using System;
using System.Linq;
using Android.Database;
using Android.Text;
using Android.OS;
using Java.IO;
using Android.Systems;

namespace PlatformBindings.Services
{
    /// <summary>
    /// Creates a Resharable URI for a SAF File. <para/>
    /// Ported from https://stackoverflow.com/a/31283751, Made by user1643723
    /// </summary>
    public abstract class FileReshareProvider : ContentProvider
    {
        public FileReshareProvider(string Authority)
        {
            BASE_URI = Android.Net.Uri.Parse($"content://{Authority}/");
            Current = this;
        }

        public static FileReshareProvider Current { get; private set; }

        private static readonly string ORIGINAL_URI = "o";
        private static readonly string FD = "fd";
        private static readonly string PATH = "p";

        private Android.Net.Uri BASE_URI;

        public Android.Net.Uri GetShareableURI(ParcelFileDescriptor fd, Android.Net.Uri trueUri)
        {
            var path = fd == null ? null : GetFdPath(fd);
            var uri = trueUri.ToString();

            var builder = BASE_URI.BuildUpon();

            if (!TextUtils.IsEmpty(uri))
                builder.AppendQueryParameter(ORIGINAL_URI, uri);

            if (fd != null && !TextUtils.IsEmpty(path))
                builder.AppendQueryParameter(FD, fd.Fd.ToString())
                       .AppendQueryParameter(PATH, path);

            return builder.Build();
        }

        public override ParcelFileDescriptor OpenFile(Android.Net.Uri uri, String mode)
        {
            var o = uri.GetQueryParameter(ORIGINAL_URI);
            var fd = uri.GetQueryParameter(FD);
            var path = uri.GetQueryParameter(PATH);

            if (TextUtils.IsEmpty(o)) return null;

            // offer the descriptor directly, if our process still has it
            try
            {
                if (!TextUtils.IsEmpty(fd) && !TextUtils.IsEmpty(path))
                {
                    var intFd = int.Parse(fd);

                    ParcelFileDescriptor desc = ParcelFileDescriptor.FromFd(intFd);

                    if (intFd >= 0 && path.Equals(GetFdPath(desc)))
                    {
                        return desc;
                    }
                }
            }
            catch (Exception) { }

            // otherwise just forward the call
            try
            {
                var trueUri = Android.Net.Uri.Parse(o);

                return Context.ContentResolver.OpenFileDescriptor(trueUri, mode);
            }
            catch (Exception) { }

            throw new FileNotFoundException();
        }

        public override string GetType(Android.Net.Uri uri)
        {
            string o = uri.GetQueryParameter(ORIGINAL_URI);
            if (TextUtils.IsEmpty(o)) return "*/*";

            try
            {
                var trueUri = Android.Net.Uri.Parse(o);

                return Context.ContentResolver.GetType(trueUri);
            }
            catch (Exception) { return null; }
        }

        public override ICursor Query(Android.Net.Uri uri, string[] projection, string selection, string[] selectionArgs, string sortOrder)
        {
            var o = uri.GetQueryParameter(ORIGINAL_URI);

            if (TextUtils.IsEmpty(o)) return null;

            try
            {
                var trueUri = Android.Net.Uri.Parse(o);

                return Context.ContentResolver.Query(trueUri, projection,
                    selection, selectionArgs, sortOrder);
            }
            catch (Exception) { }

            return null;
        }

        public override Android.Net.Uri Insert(Android.Net.Uri uri, ContentValues values)
        {
            return null;
        }

        public override bool OnCreate()
        {
            return true;
        }

        public override int Update(Android.Net.Uri uri, ContentValues values, string selection, string[] selectionArgs)
        {
            return 0;
        }

        public override int Delete(Android.Net.Uri uri, string selection, string[] selectionArgs)
        {
            return 0;
        }

        private static string GetFdPath(ParcelFileDescriptor fd)
        {
            string resolved;

            try
            {
                File procfsFdFile = new File("/proc/self/fd/" + fd.Fd);

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    // Returned name may be empty or "pipe:", "socket:", "(deleted)" etc.
                    resolved = Os.Readlink(procfsFdFile.AbsolutePath);
                }
                else
                {
                    // Returned name is usually valid or empty, but may start from
                    // funny prefix if the file does not have a name
                    resolved = procfsFdFile.CanonicalPath;
                }

                if (TextUtils.IsEmpty(resolved) || resolved.ElementAt(0) != '/'
                              || resolved.StartsWith("/proc/") || resolved.StartsWith("/fd/"))
                    return null;
            }
            catch (IOException)
            {
                // This exception means, that given file DID have some name, but it is
                // too long, some of symlinks in the path were broken or, most
                // likely, one of it's directories is inaccessible for reading.
                // Either way, it is almost certainly not a pipe.
                return "";
            }
            catch (Exception)
            {
                // Actually ErrnoException, but base type avoids VerifyError on old versions
                // This exception should be VERY rare and means, that the descriptor
                // was made unavailable by some Unix magic.
                return null;
            }

            return resolved;
        }
    }
}