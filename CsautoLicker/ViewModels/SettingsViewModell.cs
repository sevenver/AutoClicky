using CsautoLicker.Helpers;
using CsautoLicker.Models;
using CsautoLicker.Services;
using GalaSoft.MvvmLight.Messaging;
using LowLevelInput.Converters;
using LowLevelInput.Hooks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CsautoLicker.ViewModels
{
    public class SettingsViewModell : BaseViewModell, IDisposable
    {


        public ObservableCollection<KeyCombinationTrigger> RecordingCombinations { get; set; } = new ObservableCollection<KeyCombinationTrigger>();
        private InputManager inputManager = new InputManager();
        private LowLevelKeyboardHook keyboardHook = new LowLevelKeyboardHook();

        public SettingsViewModell() : base("Settings")
        {

            SetupCombinations();
            inputManager.OnKeyboardEvent += InputManager_OnKeyboardEvent;
            keyboardHook.OnKeyboardEvent += KeyboardHook_OnKeyboardEvent;
            inputManager.Initialize();
            keyboardHook.InstallHook();
        }

        private void KeyboardHook_OnKeyboardEvent(VirtualKeyCode key, KeyState state)
        {

        }

        private bool shift = false;
        private bool ctrl = false;
        private void InputManager_OnKeyboardEvent(VirtualKeyCode key, KeyState state)
        {
            if (key == VirtualKeyCode.Lshift || key == VirtualKeyCode.Rshift) { shift = state == KeyState.Down; return; }
            if (key == VirtualKeyCode.Lcontrol || key == VirtualKeyCode.Lcontrol) { ctrl = state == KeyState.Down; return; }


            if (state == KeyState.Down && (shift || ctrl))
            {
                var match = RecordingCombinations.FirstOrDefault(fod => fod.Compare(shift, ctrl, key));
                if (match != null)
                {
                    match.Execute();
                }
            }
        }



        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            var akarki = char.ToUpper(e.KeyChar);
            var akarkiki = (Keys)akarki;
            var akarmi = (Keys)e.KeyChar;


        }

        private void SetupCombinations()
        {
            if (File.Exists("paks1.json"))
            {
                var jsonString = File.ReadAllText("paks1.json");
                foreach (var clickyKey in JsonConvert.DeserializeObject<List<KeyCombinationTrigger>>(jsonString))
                {
                    RecordingCombinations.Add(clickyKey);
                }
            }
            else
            {
                var startRecording = new KeyCombinationTrigger(() => Messenger.Default.Send(TaskEnum.Recording), VirtualKeyCode.R, "Start recording", true, true);
                RecordingCombinations.Add(startRecording);
                var startRepeating = new KeyCombinationTrigger(() => Messenger.Default.Send(TaskEnum.Repeat), VirtualKeyCode.P, "Start Repeating", true, true);
                RecordingCombinations.Add(startRepeating);
            }
        }

        public void Dispose()
        {
            keyboardHook.Dispose();
            inputManager.Dispose();
        }
    }
}
