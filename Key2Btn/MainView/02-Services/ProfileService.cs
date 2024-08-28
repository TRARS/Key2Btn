using Key2Btn.MainView.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Key2Btn.MainView.Services
{
    internal class ProfileService : IProfileService
    {
        private const string filrName = "MacroButtonList";
        private string fullPath = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}{filrName}.json";
        private JsonSerializerOptions options = new JsonSerializerOptions()
        {
            WriteIndented = true,
            AllowTrailingCommas = true,
            IgnoreReadOnlyProperties = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public async Task SaveProfile<T>(T profile) where T : new()
        {
            try
            {
                using (var sw = new StreamWriter(fullPath, false, Encoding.Unicode))
                {
                    await sw.WriteLineAsync(JsonSerializer.Serialize(profile, options));
                    Debug.WriteLine($"save to: {fullPath}");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"{ex.Message}");
            }

        }

        public async Task<T> LoadProfile<T>() where T : new()
        {
            try
            {
                if (File.Exists(fullPath))
                {
                    Debug.WriteLine($"load from: {fullPath}");
                    return JsonSerializer.Deserialize<T>(await File.ReadAllTextAsync(fullPath))!;
                }
                else
                {
                    Debug.WriteLine($"File does not exist.\n{fullPath}");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"{ex.Message}");
            }

            return default;
        }
    }
}
