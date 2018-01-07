// ******************************************************************
// Copyright (c) William Bradley
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Net;
using Android.Database;

namespace PlatformBindings.Services
{
    public static class PathResolver
    {
        public static string ResolveFile(Context context, Uri uri)
        {
            var isKitKat = Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat;

            // DocumentProvider
            if (isKitKat && DocumentsContract.IsDocumentUri(context, uri))
            {
                // ExternalStorageProvider
                if (IsExternalStorageDocument(uri))
                {
                    string docId = DocumentsContract.GetDocumentId(uri);
                    string[] split = docId.Split(':');
                    string type = split[0];

                    // TODO handle non-primary volumes
                    //if ("primary".Equals(type, System.StringComparison.InvariantCultureIgnoreCase))
                    //{
                    //    return Environment.ExternalStorageDirectory + "/" + split[1];
                    //}
                    return Environment.ExternalStorageDirectory + "/" + split[1];
                }
                // DownloadsProvider
                else if (IsDownloadsDocument(uri))
                {
                    string id = DocumentsContract.GetDocumentId(uri);
                    Uri contentUri = ContentUris.WithAppendedId(
                            Uri.Parse("content://downloads/public_downloads"), System.Convert.ToInt64(id));

                    return GetDataColumn(context, contentUri, null, null);
                }
                // MediaProvider
                else if (IsMediaDocument(uri))
                {
                    string docId = DocumentsContract.GetDocumentId(uri);
                    string[] split = docId.Split(':');
                    string type = split[0];

                    Uri contentUri = null;
                    switch (type.ToLower())
                    {
                        case "image":
                            contentUri = MediaStore.Images.Media.ExternalContentUri;
                            break;

                        case "video":
                            contentUri = MediaStore.Video.Media.ExternalContentUri;
                            break;

                        case "audio":
                            contentUri = MediaStore.Audio.Media.ExternalContentUri;
                            break;
                    }

                    string selection = "_id=?";
                    string[] selectionArgs = new string[] { split[1] };

                    return GetDataColumn(context, contentUri, selection, selectionArgs);
                }
                else
                {
                    return GetDataColumn(context, uri, null, null);
                }
            }
            // MediaStore (and general)
            else if (string.Equals("content", uri.Scheme, System.StringComparison.InvariantCultureIgnoreCase))
            {
                return GetDataColumn(context, uri, null, null);
            }
            // File
            else if (string.Equals("file", uri.Scheme, System.StringComparison.InvariantCultureIgnoreCase))
            {
                return uri.Path;
            }

            return null;
        }

        public static string GetDataColumn(Context context, Uri uri, string selection, string[] selectionArgs)
        {
            ICursor cursor = null;
            string column = "_data";
            string[] projection = {
                column
            };

            try
            {
                cursor = context.ContentResolver.Query(uri, projection, selection, selectionArgs, null);
                if (cursor != null && cursor.MoveToFirst())
                {
                    int column_index = cursor.GetColumnIndexOrThrow(column);
                    return cursor.GetString(column_index);
                }
            }
            finally
            {
                if (cursor != null)
                    cursor.Dispose();
            }
            return null;
        }

        public static bool IsExternalStorageDocument(Uri uri)
        {
            return "com.android.externalstorage.documents".Equals(uri.Authority);
        }

        public static bool IsDownloadsDocument(Uri uri)
        {
            return "com.android.providers.downloads.documents".Equals(uri.Authority);
        }

        public static bool IsMediaDocument(Uri uri)
        {
            return "com.android.providers.media.documents".Equals(uri.Authority);
        }
    }
}