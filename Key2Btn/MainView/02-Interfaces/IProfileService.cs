using System.Threading.Tasks;

namespace Key2Btn.MainView.Interfaces
{
    public interface IProfileService
    {
        Task<T> LoadProfile<T>() where T : new();
        Task SaveProfile<T>(T profile) where T : new();
    }
}
