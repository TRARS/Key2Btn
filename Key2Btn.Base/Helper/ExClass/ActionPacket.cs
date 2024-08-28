using System;

namespace Key2Btn.Base.Helper.ExClass
{
    public record ActionPacket(string Key, Action? Action, string? Comment)
    {
        public ActionPacket() : this(string.Empty, null, null) { }
    }
}
