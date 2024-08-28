namespace Key2Btn.Base.CustomInterfaces
{
    public interface IWindowHelper
    {
        void ShowEx(params object[] optionalParams);
        void HideEx();
        void CloseEx();
        nint GetHandle();
        void SetOwner(nint ptr);
    }
}
