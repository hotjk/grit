using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Utility.Authentication
{
    public class CookieTicket
    {
        public DateTime IssueDate {get; private set;}
        public bool Persistent {get; private set;}
        public string Name {get; private set;}
        public Guid Id {get; private set;}
        public byte[] UserData { get; private set; }

        public CookieTicket(Guid id, bool persistent, string name, byte[] userData = null)
        {
            Id = id;
            Persistent = persistent;
            Name = name;
            UserData = userData;
            IssueDate = DateTime.UtcNow;
        }

        public CookieTicket(string name, byte[] userData = null)
            : this(Guid.NewGuid(), false, name, userData)
        {
        }

        private CookieTicket(byte[] data)
        {
            using (var memoryStream = new MemoryStream(data))
            {
                using (var binaryReader = new BinaryReader(memoryStream))
                {
                    Id = new Guid(binaryReader.ReadBytes(16));
                    Persistent = binaryReader.ReadBoolean();
                    IssueDate = DateTime.FromBinary(binaryReader.ReadInt64());
                    Name = binaryReader.ReadString();

                    var tagLength = binaryReader.ReadInt16();
                    if (tagLength == 0)
                    {
                        UserData = null;
                    }
                    else
                    {
                        UserData = binaryReader.ReadBytes(tagLength);
                    }
                }
            }
        }

        public byte[] Serialize()
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    binaryWriter.Write(Id.ToByteArray());
                    binaryWriter.Write(Persistent);
                    binaryWriter.Write(IssueDate.ToBinary());
                    binaryWriter.Write(Name);
                    if (UserData == null)
                    {
                        binaryWriter.Write((short)0);
                    }
                    else
                    {
                        binaryWriter.Write((short)UserData.Length);
                        binaryWriter.Write(UserData);
                    }
                }
                return memoryStream.ToArray();
            }
        }

        public static CookieTicket Deserialize(byte[] data)
        {
            return new CookieTicket(data);
        }

        public void Renew()
        {
            IssueDate = DateTime.UtcNow;
        }

        public bool IsExpired(TimeSpan validity)
        {
            return IssueDate.Add(validity) <= DateTime.UtcNow;
        }
    }
}
