using Key2Btn.Base.CustomInterfaces;
using Key2Btn.Base.Helper.ExClass;
using Key2Btn.Base.Helper.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Key2Btn.MainView.Services
{
    public partial class ActionExecutor : IActionExecutor
    {
        // Dictionary to store actionKey and corresponding Action.
        private readonly Dictionary<string, ActionPacket> _actions = new();

        // Registers an action with a given key.
        public void RegisterAction(ActionPacket packet)
        {
            if (string.IsNullOrWhiteSpace(packet.Key))
            {
                throw new ArgumentException("Action key cannot be null or whitespace.", nameof(packet.Key));
            }

            if (_actions.ContainsKey(packet.Key))
            {
                throw new InvalidOperationException($"An action with the key '{packet.Key}' is already registered.");
            }

            _actions[packet.Key] = packet;
        }

        // Unregisters an action associated with the given key.
        public void UnregisterAction(string actionKey)
        {
            if (!_actions.Remove(actionKey))
            {
                throw new KeyNotFoundException($"No action found for the key '{actionKey}'.");
            }
        }

        // Invokes the action associated with the given key.
        public void Invoke(string actionKey)
        {
            if (!_actions.TryGetValue(actionKey, out var packet))
            {
                throw new KeyNotFoundException($"No action found for the key '{actionKey}'.");
            }

            packet.Action?.Invoke();
        }

        //
        public IEnumerable<ActionPacket> ActionKeyList => _actions.Values;
    }

    public partial class ActionExecutor
    {
        private bool is_in_designmode => (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;

        public ActionExecutor()
        {
            Task.Run(() => LoadFromAssembly()).Wait();
        }

        private void LoadFromAssembly()
        {
            Func<string?, bool> IsFileNameMatch = (_) =>
            {
                if (_ is null) return false;
                return Regex.IsMatch(_, ".*Key2Btn.*") && Regex.IsMatch(_, ".*SpecialAction.*");
            };
            Func<string?, bool> IsNamespaceMatch = (_) =>
            {
                if (_ is null) return false;
                return Regex.IsMatch(_, ".*Key2Btn.*") && Regex.IsMatch(_, ".*SpecialAction.*");
            };
            Func<string?, bool> IsClassNameMatch = (_) =>
            {
                if (_ is null) return false;
                return Regex.IsMatch(_, ".*ActionList.*");
            };

            try
            {
                if (is_in_designmode is false)
                {
                    string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll");
                    foreach (string _dllpath in files)
                    {
                        if (IsFileNameMatch(Path.GetFileName(_dllpath)))
                        {
                            var rawAssembly = File.ReadAllBytes(_dllpath);
                            var tpList = (from t in Assembly.Load(rawAssembly).GetTypes()
                                          where (t.IsClass) &&
                                                (t.GetInterfaces().Contains(typeof(IActionCollection))) &&
                                                (IsNamespaceMatch(t.Namespace)) &&
                                                (IsClassNameMatch(t.Name))
                                          select t);

                            PushToDic(tpList);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show($"GetActionList Error: {ex.Message}"); }
        }

        private void PushToDic(IEnumerable<Type> tpList)
        {
            foreach (Type item in tpList)
            {
                try
                {
                    if (item.FullName is not null && item.Assembly.CreateInstance(item.FullName) is IActionCollection obj)
                    {
                        foreach (var packet in obj.ActionKeyList)
                        {
                            _actions.AddActions(packet);
                        };
                    }
                }
                catch (Exception ex) { MessageBox.Show($"{item.Name}: {ex.Message}"); }
            }
        }
    }
}
