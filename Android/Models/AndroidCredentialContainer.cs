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

using System;
using PlatformBindings.Models.Settings;
using Javax.Crypto;
using Java.Security;
using Javax.Crypto.Spec;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;
using Java.IO;

namespace PlatformBindings.Models
{
    internal class AndroidCredentialContainer : CredentialContainer
    {
        public AndroidCredentialContainer(ISettingsContainer Container, CredentialContainer Credentials) : this()
        {
            this.Container = Container;
            var newheader = $"{Credentials.ResourceName}^R^{Credentials.Username}";
            EncryptedHeader = Encrypt(newheader, writer);
            SetHeaderData(newheader);

            Password = Credentials.Password;
        }

        public AndroidCredentialContainer(ISettingsContainer Container, string Header) : this()
        {
            this.Container = Container;
            EncryptedHeader = Header;
            try
            {
                var decryptedHeader = Decrypt(Header);
                SetHeaderData(decryptedHeader);
            }
            catch
            {
                Container.RemoveKey(Header);
                HeaderData = new string[] { DecryptionError, DecryptionError };
            }
        }

        private void SetHeaderData(string DecryptedHeader)
        {
            HeaderData = DecryptedHeader.Split(new string[] { "^R^" }, StringSplitOptions.None);
        }

        public override string ResourceName { get => HeaderData[0]; }
        public override string Username { get => HeaderData[1]; }

        public override string Password
        {
            get
            {
                try
                {
                    var encryptedVal = Container.GetValue<string>(EncryptedHeader);
                    return Decrypt(encryptedVal);
                }
                catch { return null; }
            }
            set
            {
                var encryptedVal = Encrypt(value, writer);
                Container.SetValue(EncryptedHeader, encryptedVal);
            }
        }

        /// <summary>
        /// Decrypted HeaderData
        /// </summary>
        private string[] HeaderData { get; set; }

        internal string EncryptedHeader { get; }
        internal ISettingsContainer Container { get; }

        /*
        Copyright (C) 2012 Sveinung Kval Bakken, sveinung.bakken@gmail.com

        Permission is hereby granted, free of charge, to any person obtaining
        a copy of this software and associated documentation files (the
        "Software"), to deal in the Software without restriction, including
        without limitation the rights to use, copy, modify, merge, publish,
        distribute, sublicense, and/or sell copies of the Software, and to
        permit persons to whom the Software is furnished to do so, subject to
        the following conditions:

        The above copyright notice and this permission notice shall be
        included in all copies or substantial portions of the Software.

        THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
        EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
        MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
        NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
        LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
        OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
        WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

         */
        // Converted to C# for PlatformBindings-Android.
        #region Encryption

        private AndroidCredentialContainer()
        {
            try
            {
                writer = Cipher.GetInstance(TRANSFORMATION);
                reader = Cipher.GetInstance(TRANSFORMATION);
                Encoding = Encoding.GetEncoding(CHARSET);

                using (var key = AndroidAppServices.KeyGenerator.GetSecureKey())
                {
                    InitCiphers(key);
                }
            }
            catch (GeneralSecurityException e)
            {
                throw new SecurityException(e.Message);
            }
            catch (UnsupportedEncodingException e)
            {
                throw new SecurityException(e.Message);
            }
        }

        private readonly string TRANSFORMATION = "AES/CBC/PKCS5Padding";
        private readonly string SECRET_KEY_HASH_TRANSFORMATION = "SHA-256";
        private readonly string CHARSET = "UTF-8";
        public static readonly string DecryptionError = "ERR_DECRYPT";

        private Encoding Encoding;
        private Cipher writer;
        private Cipher reader;

        protected void InitCiphers(SecureString secureKey)
        {
            var bstr = Marshal.SecureStringToBSTR(secureKey);
            var key = Marshal.PtrToStringBSTR(bstr);

            IvParameterSpec ivSpec = GetIv();
            SecretKeySpec secretKey = GetSecretKey(key);

            writer.Init(CipherMode.EncryptMode, secretKey, ivSpec);
            reader.Init(CipherMode.DecryptMode, secretKey, ivSpec);
        }

        protected IvParameterSpec GetIv()
        {
            byte[] iv = new byte[writer.BlockSize];
            Array.Copy(Encoding.GetBytes("fldsjfodasjifudslfjdsaofshaufihadsf"), 0, iv, 0, writer.BlockSize);
            return new IvParameterSpec(iv);
        }

        protected SecretKeySpec GetSecretKey(string key)
        {
            byte[] keyBytes = CreateKeyBytes(key);
            return new SecretKeySpec(keyBytes, TRANSFORMATION);
        }

        protected byte[] CreateKeyBytes(string key)
        {
            MessageDigest md = MessageDigest.GetInstance(SECRET_KEY_HASH_TRANSFORMATION);
            md.Reset();
            var keyBytes = md.Digest(Encoding.GetBytes(key));
            return keyBytes;
        }

        protected String Encrypt(string value, Cipher writer)
        {
            byte[] secureValue;
            try
            {
                secureValue = Convert(writer, Encoding.GetEncoding(CHARSET).GetBytes(value));
            }
            catch (UnsupportedEncodingException e)
            {
                throw new SecurityException(e.Message);
            }
            var secureValueEncoded = System.Convert.ToBase64String(secureValue);
            return secureValueEncoded;
        }

        protected string Decrypt(string securedEncodedValue)
        {
            byte[] securedValue = System.Convert.FromBase64String(securedEncodedValue);
            byte[] value = Convert(reader, securedValue);
            try
            {
                return Encoding.GetString(value);
            }
            catch (UnsupportedEncodingException e)
            {
                throw new SecurityException(e.Message);
            }
        }

        private static byte[] Convert(Cipher cipher, byte[] bs)
        {
            try
            {
                return cipher.DoFinal(bs);
            }
            catch (Exception e)
            {
                throw new SecurityException(e.Message);
            }
        }

        ~AndroidCredentialContainer()
        {
            writer?.Dispose();
            reader?.Dispose();
        }

        #endregion Encryption
    }
}