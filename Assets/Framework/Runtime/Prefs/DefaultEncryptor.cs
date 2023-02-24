﻿/*
 * MIT License
 *
 * Copyright (c) 2018 Clark Yang
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in 
 * the Software without restriction, including without limitation the rights to 
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
 * of the Software, and to permit persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all 
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
 * SOFTWARE.
 */

using System;
using System.Text;
using System.Security.Cryptography;

namespace Framework
{

    public class DefaultEncryptor : IEncryptor
    {
        private const int IV_SIZE = 16;
        private static readonly byte[] DEFAULT_IV;
        private static readonly byte[] DEFAULT_KEY;
        private RijndaelManaged _cipher;

        private readonly byte[] _iv;
        private readonly byte[] _key;

        static DefaultEncryptor()
        {
            DEFAULT_IV = Encoding.ASCII.GetBytes("5CyM5tcL3yDFiWlN");
            DEFAULT_KEY = Encoding.ASCII.GetBytes("W8fnmqMynlTJXPM1");
        }

        /// <summary>
        /// 
        /// </summary>
        public DefaultEncryptor() : this(null, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        public DefaultEncryptor(byte[] key, byte[] iv)
        {
            if (iv == null)
                _iv = DEFAULT_IV;

            if (key == null)
                _key = DEFAULT_KEY;

            CheckIV(_iv);
            CheckKey(_key);

#if NETFX_CORE
            SymmetricKeyAlgorithmProvider provider =
 SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesCbcPkcs7);
            cryptographicKey = provider.CreateSymmetricKey(this.key.AsBuffer());
#else
            _cipher = new RijndaelManaged()
            {
                Mode = CipherMode.CBC, //use CBC
                Padding = PaddingMode.PKCS7, //default PKCS7
                KeySize = 128, //default 256
                BlockSize = 128, //default 128
                FeedbackSize = 128 //default 128
            };
#endif
        }

        protected static bool CheckKey(byte[] bytes)
        {
            if (bytes == null || (bytes.Length != 16 && bytes.Length != 24 && bytes.Length != 32))
                throw new ArgumentException("The 'Key' must be 16byte 24byte or 32byte!");
            return true;
        }

        protected static bool CheckIV(byte[] bytes)
        {
            if (bytes == null || bytes.Length != IV_SIZE)
                throw new ArgumentException("The 'IV' must be 16byte!");
            return true;
        }

        public byte[] Encode(byte[] plainData)
        {
#if NETFX_CORE
            IBuffer bufferEncrypt = CryptographicEngine.Encrypt(cryptographicKey, plainData.AsBuffer(), iv.AsBuffer());
            return bufferEncrypt.ToArray();
#else
            ICryptoTransform encryptor = _cipher.CreateEncryptor(_key, _iv);
            return encryptor.TransformFinalBlock(plainData, 0, plainData.Length);
#endif
        }

        public byte[] Decode(byte[] cipherData)
        {
#if NETFX_CORE
            IBuffer bufferDecrypt = CryptographicEngine.Decrypt(cryptographicKey, cipherData.AsBuffer(), iv.AsBuffer());
            return bufferDecrypt.ToArray();
#else
            ICryptoTransform decryptor = _cipher.CreateDecryptor(_key, _iv);
            return decryptor.TransformFinalBlock(cipherData, 0, cipherData.Length);
#endif
        }
    }
}