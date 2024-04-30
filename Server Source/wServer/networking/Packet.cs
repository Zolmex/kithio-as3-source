﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using System.Net.Sockets;
using System.Net;

namespace wServer.networking
{
    public abstract class Packet
    {
        public static Dictionary<PacketID, Packet> Packets = new Dictionary<PacketID, Packet>();
        static Packet()
        {
            foreach (var i in typeof(Packet).Assembly.GetTypes())
                if (typeof(Packet).IsAssignableFrom(i) && !i.IsAbstract)
                {
                    Packet pkt = (Packet)Activator.CreateInstance(i);
                    if (!(pkt is ServerPacket))
                        Packets.Add(pkt.ID, pkt);
                }
        }
        public abstract PacketID ID { get; }
        public abstract Packet CreateInstance();

        public abstract void Crypt(Client client, byte[] dat, int offset, int len);

        public void Read(Client client, byte[] body, int offset, int len)
        {
            Crypt(client, body, offset, len);
            Read(new NReader(new MemoryStream(body)));
        }
        public int Write(Client client, byte[] buff, int offset)
        {
            MemoryStream s = new MemoryStream(buff, offset + 5, buff.Length - offset - 5);
            this.Write(new NWriter(s));

            var len = (int)s.Position;
            Crypt(client, buff, offset + 5, len);
            Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(len + 5)), 0, buff, offset, 4);
            buff[offset + 4] = (byte)this.ID;
            return len + 5;
        }

        protected abstract void Read(NReader rdr);
        protected abstract void Write(NWriter wtr);

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder("{");
            var arr = GetType().GetProperties();
            for (var i = 0; i < arr.Length; i++)
            {
                if (i != 0) ret.Append(", ");
                ret.AppendFormat("{0}: {1}", arr[i].Name, arr[i].GetValue(this, null));
            }
            ret.Append("}");
            return ret.ToString();
        }
    }

    public class NopPacket : Packet
    {
        public override PacketID ID { get { return PacketID.Packet; } }
        public override Packet CreateInstance() { return new NopPacket(); }
        public override void Crypt(Client client, byte[] dat, int offset, int len) { }
        protected override void Read(NReader rdr) { }
        protected override void Write(NWriter wtr) { }
    }
}
