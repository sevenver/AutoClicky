using CsautoLicker.Helpers;
using CsautoLicker.Models;
using CsautoLicker.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace CsautoLicker.ViewModels
{
    public class ClickerViewModell : BaseViewModell
    {
        public MouseHookService MouseHook { get; set; } = new MouseHookService();
        public ObservableCollection<ClickyThingy> ClickPositions { get; set; } = new ObservableCollection<ClickyThingy>();
        public Stopwatch Stopwatch { get; set; } = new Stopwatch();
        public ClickerViewModell() : base("Clicky")
        {
            if (File.Exists("paks2.json"))
            {
                var jsonString = File.ReadAllText("paks2.json");
                foreach (var clicky in JsonConvert.DeserializeObject<List<ClickyThingy>>(jsonString))
                {
                    ClickPositions.Add(clicky);
                }
            }
        }
        private bool recording;




        private ICommand startRecording;

        public ICommand StartRecordingCommand
        {
            get { return startRecording ?? (startRecording = new RelayCommand(s => HookUpMouse())); }
        }
        private ICommand stopRecording;

        public ICommand StopRecordingCommand
        {
            get { return stopRecording ?? (stopRecording = new RelayCommand(s => UnHookMouse())); }
        }
        private ICommand repeatRecording;

        public ICommand RepeatRecordingCommand
        {
            get { return repeatRecording ?? (repeatRecording = new RelayCommand(async s => await RepeatRecording())); }
        }
        private ICommand repeatEndlessRecordingCommand;

        public ICommand RepeatEndlessRecordingCommand
        {
            get { return repeatEndlessRecordingCommand ?? (repeatEndlessRecordingCommand = new RelayCommand(s => HandleRepeat())); }
        }
        private ICommand save;

        public ICommand SaveCommand
        {
            get { return save ?? (save = new RelayCommand(async s => await SaveClicky())); }
        }
        private ICommand clear;

        public ICommand ClearCommand
        {
            get { return clear ?? (clear = new RelayCommand( s =>  ClearClicky())); }
        }

        private void ClearClicky()
        {
            ClickPositions.Clear();
            File.Delete("paks2.json");
        }

        private Task SaveClicky()
        {
            var jsonString = JsonConvert.SerializeObject(ClickPositions);
            File.Create("paks2.json").Dispose();
            return File.WriteAllTextAsync("paks2.json", jsonString);
        }

        public bool IsRepeating { get; set; }

        public void HandleRepeat()
        {
            IsRepeating = !IsRepeating;
            if (IsRepeating)
            {
                RepeatEndlessRecording(); ;
            }
        }

        private async Task RepeatEndlessRecording()
        {
            if (ClickPositions.Any())
            {

                while (IsRepeating)
                {
                    await RepeatRecording();
                }
            }
        }

        public async Task RepeatRecording()
        {
            if (recording) UnHookMouse();
            foreach (var click in ClickPositions.Where(w => w.Enabled))
            {
                if (click.Repeats == 0)
                {
                    await MouseHook.ClickWithMouse(click);
                }
                else
                {
                    foreach (var index in Enumerable.Range(0, click.Repeats - 1))
                    {
                        await MouseHook.ClickWithMouse(click);
                    }
                }
            }
        }
        private void UnHookMouse()
        {
            recording = false;
            Stopwatch.Start();

            MouseHook.UnHook();
            //MouseHook.MouseMoveEvent -= mh_MouseMoveEvent;
            // MouseHook.MouseClickEvent -= mh_MouseClickEvent;
            MouseHook.MouseDownEvent -= mh_MouseDownEvent;
            Stopwatch.Stop();
            Stopwatch.Reset();
            //MouseHook.MouseUpEvent -= mh_MouseUpEvent;
        }

        private void HookUpMouse()
        {
            recording = true;
            MouseHook.SetHook();
            //MouseHook.MouseMoveEvent += mh_MouseMoveEvent;
            //MouseHook.MouseClickEvent += mh_MouseClickEvent;
            MouseHook.MouseDownEvent += mh_MouseDownEvent;
            Stopwatch.Start();


            //MouseHook.MouseUpEvent += mh_MouseUpEvent;
        }




        private void mh_MouseDownEvent(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                Stopwatch.Stop();
                ClickPositions.Add(new ClickyThingy() { Location = e.Location, Delay = (int)Stopwatch.ElapsedMilliseconds });
                Stopwatch.Reset();
                Stopwatch.Start();
            }
        }

        private void mh_MouseUpEvent(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
            }
            if (e.Button == MouseButtons.Right)
            {
            }
        }
        private void mh_MouseClickEvent(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {

            }
        }

        private void mh_MouseMoveEvent(object sender, MouseEventArgs e)
        {
            int x = e.Location.X;
            int y = e.Location.Y;
        }

    }
}
