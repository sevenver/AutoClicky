using CsautoLicker.Helpers;
using CsautoLicker.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsautoLicker.Services
{
    public class MouseHookService
    {

        private Point point;
        private Point Point
        {
            get { return point; }
            set
            {
                if (point != value)
                {
                    point = value;
                    if (MouseMoveEvent != null)
                    {
                        var e = new MouseEventArgs(MouseButtons.None, 0, point.X, point.Y, 0);
                        MouseMoveEvent(this, e);
                    }
                }
            }
        }
        private int hHook;
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_MBUTTONUP = 0x208;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MBUTTONDBLCLK = 0x209;
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int WH_MOUSE_LL = 14;
        public Win32Api.HookProc hProc;
        public MouseHookService()
        {
            this.Point = new Point();
        }



        public async Task ClickWithMouse(ClickyThingy details)
        {
            Win32Api.SetCursorPos(details.Location.X, details.Location.Y);
            await Task.Delay(20);
            Win32Api.mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, details.Location.X, details.Location.Y, 0, 0);
            await Task.Delay(details.Delay);
        }

        public int SetHook()
        {
            hProc = new Win32Api.HookProc(MouseHookProc);
            hHook = Win32Api.SetWindowsHookEx(WH_MOUSE_LL, hProc, IntPtr.Zero, 0);
            return hHook;
        }
        public void UnHook()
        {
            Win32Api.UnhookWindowsHookEx(hHook);
        }
        private int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            Win32Api.MouseHookStruct MyMouseHookStruct = (Win32Api.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(Win32Api.MouseHookStruct));
            if (nCode < 0)
            {
                return Win32Api.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
            else
            {

                MouseButtons button = MouseButtons.None;
                int clickCount = 0;
                switch ((Int32)wParam)
                {
                    case WM_LBUTTONDOWN:
                        button = MouseButtons.Left;
                        clickCount = 1;
                        MouseDownEvent?.Invoke(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                        break;
                    //case WM_RBUTTONDOWN:
                    //    button = MouseButtons.Right;
                    //    clickCount = 1;
                    //    MouseDownEvent?.Invoke(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                    //    break;
                    //case WM_MBUTTONDOWN:
                    //    button = MouseButtons.Middle;
                    //    clickCount = 1;
                    //    MouseDownEvent?.Invoke(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                    //    break;
                    //case WM_LBUTTONUP:
                    //    button = MouseButtons.Left;
                    //    clickCount = 1;
                    //    MouseUpEvent?.Invoke(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                    //    break;
                    //case WM_RBUTTONUP:
                    //    button = MouseButtons.Right;
                    //    clickCount = 1;
                    //    MouseUpEvent?.Invoke(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                    //    break;
                    //case WM_MBUTTONUP:
                    //    button = MouseButtons.Middle;
                    //    clickCount = 1;
                    //    MouseUpEvent?.Invoke(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                    //    break;
                    default:
                        break;
                }

                //var e = new MouseEventArgs(button, clickCount, point.X, point.Y, 0);
                //MouseClickEvent(this, e);

                this.Point = new Point(MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y);
                return Win32Api.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
        }

        public delegate void MouseMoveHandler(object sender, MouseEventArgs e);
        public event MouseMoveHandler MouseMoveEvent;

        public delegate void MouseClickHandler(object sender, MouseEventArgs e);
        public event MouseClickHandler MouseClickEvent;

        public delegate void MouseDownHandler(object sender, MouseEventArgs e);
        public event MouseDownHandler MouseDownEvent;

        public delegate void MouseUpHandler(object sender, MouseEventArgs e);
        public event MouseUpHandler MouseUpEvent;



    }
}
