using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Utility.Security
{
    public class EncryptSignManager : IDisposable
    {
        private SymmetricAlgorithm _symmetric;
        private KeyedHashAlgorithm _keyedHash;

        public EncryptSignManager(byte[] encryptKey, byte[] encryptIV, byte[] signKey)
        {
            _symmetric = SymmetricAlgorithm.Create("rijndael");
            _symmetric.Key = encryptKey;
            _symmetric.IV = encryptIV;

            _keyedHash = KeyedHashAlgorithm.Create("hmacsha256");
            _keyedHash.Key = signKey;
        }
        public byte[] EncryptThenSign(byte[] data)
        {
            byte[] encrypted;
            using (var output = new MemoryStream())
			{
                using (var cryptoOutput = new CryptoStream(output, _symmetric.CreateEncryptor(), CryptoStreamMode.Write))
				{
                    cryptoOutput.Write(data, 0, data.Length);
				}
				encrypted = output.ToArray();
			}

            byte[] sign = _keyedHash.ComputeHash(encrypted);
            byte[] combined = new byte[encrypted.Length + sign.Length];
            Buffer.BlockCopy(encrypted, 0, combined, 0, encrypted.Length);
            Buffer.BlockCopy(sign, 0, combined, encrypted.Length, sign.Length);
            return combined;
        }

        public bool ValidateThenDecrypt(byte[] data, out byte[] decrypted)
        {
            decrypted = null;
            int signLength = _keyedHash.HashSize / 8;
            
            byte[] encrypted = new byte[data.Length - signLength];
            byte[] sign = new byte[signLength];
            Buffer.BlockCopy(data, 0, encrypted, 0, data.Length - signLength);
            Buffer.BlockCopy(data, data.Length-signLength, sign, 0, signLength);

            byte[] newSign = _keyedHash.ComputeHash(encrypted);
            if (!sign.SequenceEqual(newSign)) return false;

            using (var output = new MemoryStream())
            {
                using (var cryptoOutput = new CryptoStream(output, _symmetric.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cryptoOutput.Write(encrypted, 0, encrypted.Length);
                }
                decrypted = output.ToArray();
            }
            return true;
        }

        public string GenerateEncryptKey()
        {
            using (var rijndael = new RijndaelManaged())
            {
                rijndael.GenerateKey();
                return (new SoapHexBinary(rijndael.Key)).ToString();
            }
        }

        public string GenerateEncryptIV()
        {
            using (var rijndael = new RijndaelManaged())
            {
                rijndael.GenerateKey();
                return (new SoapHexBinary(rijndael.IV)).ToString();
            }
        }

        public string GenerateValidationKey()
        {
            using (var hmacsha256 = new HMACSHA256())
            {
                return (new SoapHexBinary(hmacsha256.Key)).ToString();
            }
        }

        public void Dispose()
        {
            if (_symmetric != null)
            {
                _symmetric.Dispose();
            }
            if (_keyedHash != null)
            {
                _keyedHash.Dispose();
            }
        }
    }
}
