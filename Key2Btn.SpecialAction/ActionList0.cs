using Key2Btn.Base.CustomInterfaces;
using Key2Btn.Base.Helper.ExClass;
using System.Collections.Generic;

namespace Key2Btn.SpecialAction
{
    public class ActionList0 : IActionCollection
    {
        public IEnumerable<ActionPacket> ActionKeyList { get; init; }

        public ActionList0()
        {
            ActionKeyList = new List<ActionPacket>()
            {
                new ActionPacket("aa", () => { System.Windows.MessageBox.Show("custom action test 11"); }, "����1_��ӡ�ı�"),
                new ActionPacket("bb", () => { System.Windows.MessageBox.Show("custom action test 22"); }, "����2_��ӡ�ı�222222222222222222222222222222222222")
            };
        }
    }
}
