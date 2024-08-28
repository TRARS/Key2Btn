using Key2Btn.Base.CustomInterfaces;
using Key2Btn.Base.Helper.ExClass;
using System.Collections.Generic;

namespace Key2Btn.SpecialAction
{
    public class ActionList1 : IActionCollection
    {
        public IEnumerable<ActionPacket> ActionKeyList { get; init; }

        public ActionList1()
        {
            ActionKeyList = new List<ActionPacket>()
            {
                new ActionPacket("cc", () => { System.Windows.MessageBox.Show("custom action test 33"); }, "动作3_打印文本"),
                new ActionPacket("dd", () => { System.Windows.MessageBox.Show("custom action test 44"); }, "动作4_打印文本")
            };
        }
    }
}
