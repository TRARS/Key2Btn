using Key2Btn.Base.Helper.ExClass;
using System;
using System.Collections.Generic;

namespace Key2Btn.Base.Helper.Extensions
{
    public static class ActionCollectionExtensions
    {
        public static void AddActions(this Dictionary<string, ActionPacket> target, ActionPacket packet)
        {
            if (target.TryAdd(packet.Key, packet) is false)
            {
                throw new NotImplementedException();
            }
        }
    }
}
