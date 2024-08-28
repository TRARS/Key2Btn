using Key2Btn.Base.Helper.ExClass;
using System.Collections.Generic;

namespace Key2Btn.Base.CustomInterfaces
{
    /// <summary>
    /// 继承该接口的类的类名必须包含 'ActionList'，且命名空间必须包含'Key2Btn'和'SpecialAction'
    /// </summary>
    public interface IActionCollection
    {
        IEnumerable<ActionPacket> ActionKeyList { get; }
    }

    public interface IActionExecutor : IActionCollection
    {
        void RegisterAction(ActionPacket packet);
        void UnregisterAction(string actionKey);
        void Invoke(string actionKey);
    }
}
