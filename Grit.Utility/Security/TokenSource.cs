using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Grit.Utility.Security
{
    public class TokenSource
    {
        public TokenSource(string userData)
        {
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            Noise = new byte[NOISE_BYTE_SIZE];
            csprng.GetBytes(Noise);

            TimeStamp = DateTime.Now;
            var byteTimeStamp = BitConverter.GetBytes(TimeStamp.Ticks);

            UserData = userData;
            var byteUserData = Encoding.UTF8.GetBytes(userData);

            ByteData = new byte[NOISE_BYTE_SIZE + TIMESTAMP_BYTE_SIZE + byteUserData.Length];
            Buffer.BlockCopy(Noise, 0, ByteData, 0, NOISE_BYTE_SIZE);
            Buffer.BlockCopy(byteTimeStamp, 0, ByteData, NOISE_BYTE_SIZE, TIMESTAMP_BYTE_SIZE);
            Buffer.BlockCopy(byteUserData, 0, ByteData, NOISE_BYTE_SIZE + TIMESTAMP_BYTE_SIZE, byteUserData.Length);
        }

        public TokenSource(byte[] byteData)
        {
            var byteUserDataLength = byteData.Length - NOISE_BYTE_SIZE - TIMESTAMP_BYTE_SIZE;
            if (byteUserDataLength <= 0)
            {
                throw new Exception("Invalid token byte data.");
            }

            var byteUserData = new byte[byteUserDataLength];
            Buffer.BlockCopy(byteData, NOISE_BYTE_SIZE + TIMESTAMP_BYTE_SIZE, byteUserData, 0, byteUserDataLength);
            UserData = Encoding.UTF8.GetString(byteUserData);

            TimeStamp = new DateTime(BitConverter.ToInt64(byteData, NOISE_BYTE_SIZE));

            Noise = new byte[NOISE_BYTE_SIZE];
            Buffer.BlockCopy(byteData, 0, Noise, 0, NOISE_BYTE_SIZE);
        }

        public const int NOISE_BYTE_SIZE = 24;
        public const int TIMESTAMP_BYTE_SIZE = 8; //int64

        public byte[] Noise { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public string UserData { get; private set; }
        public byte[] ByteData { get; private set; }
    }
}