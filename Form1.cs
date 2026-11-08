using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinXPEmulator
{
    public partial class Form1 : Form
    {
        // --- WinAPI для эмуляции клавиш ---
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        const byte VK_LWIN = 0x5B;
        const byte VK_TAB = 0x09;
        const byte VK_ESCAPE = 0x1B;
        const byte VK_CONTROL = 0x11;
        const byte VK_ALT = 0x12;
        const byte VK_F4 = 0x73;
        const byte VK_DELETE = 0x2E;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Windows XP Emulator (C#)";
            this.BackColor = System.Drawing.Color.FromArgb(58, 110, 165);
            this.ClientSize = new System.Drawing.Size(500, 300);
            SetupUI();
        }

        private void SetupUI()
        {
            // === БЛОК ПИТАНИЯ ===
            AddButton("Выключить ПК", 10, 10, (s, e) => ExecuteShutdown("/s /t 0"));
            AddButton("Перезагрузка", 140, 10, (s, e) => ExecuteShutdown("/r /t 0"));
            AddButton("Спящий режим", 270, 10, (s, e) => ExecuteShutdown("/h"));

            // === БЛОК ЭМУЛЯЦИИ КЛАВИШ ===
            AddButton("Пуск (Win)", 10, 60, (s, e) => EmulateKey(VK_LWIN));
            AddButton("Alt+Tab", 140, 60, (s, e) => SimulateAltTab());
            AddButton("Ctrl+Esc", 270, 60, (s, e) => SimulateCtrlEsc());
            
            // === СИСТЕМНЫЕ КОМБИНАЦИИ ===
            AddButton("Alt+F4 (Закрыть)", 10, 110, (s, e) => SimulateAltF4());
            AddButton("Ctrl+Alt+Del", 140, 110, (s, e) => SimulateCtrlAltDel());
            
            // === ЗАВЕРШЕНИЕ РАБОТЫ ПРИЛОЖЕНИЯ ===
            AddButton("Выход из эмулятора", 10, 160, (s, e) => Application.Exit());
        }

        private void AddButton(string text, int x, int y, EventHandler onClick)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Location = new System.Drawing.Point(x, y);
            btn.Size = new System.Drawing.Size(120, 35);
            btn.Font = new System.Drawing.Font("Tahoma", 9f);
            btn.Click += onClick;
            this.Controls.Add(btn);
        }

        // --- МЕТОДЫ ЭМУЛЯЦИИ ---

        private void EmulateKey(byte key)
        {
            keybd_event(key, 0, 0, UIntPtr.Zero); // Key Down
            keybd_event(key, 0, 2, UIntPtr.Zero); // Key Up
        }

        private void SimulateAltTab()
        {
            keybd_event(VK_ALT, 0, 0, UIntPtr.Zero);
            keybd_event(VK_TAB, 0, 0, UIntPtr.Zero);
            keybd_event(VK_TAB, 0, 2, UIntPtr.Zero);
            keybd_event(VK_ALT, 0, 2, UIntPtr.Zero);
        }

        private void SimulateCtrlEsc()
        {
            keybd_event(VK_CONTROL, 0, 0, UIntPtr.Zero);
            keybd_event(VK_ESCAPE, 0, 0, UIntPtr.Zero);
            keybd_event(VK_ESCAPE, 0, 2, UIntPtr.Zero);
            keybd_event(VK_CONTROL, 0, 2, UIntPtr.Zero);
        }

        private void SimulateAltF4()
        {
            keybd_event(VK_ALT, 0, 0, UIntPtr.Zero);
            keybd_event(VK_F4, 0, 0, UIntPtr.Zero);
            keybd_event(VK_F4, 0, 2, UIntPtr.Zero);
            keybd_event(VK_ALT, 0, 2, UIntPtr.Zero);
        }

        private void SimulateCtrlAltDel()
        {
            keybd_event(VK_CONTROL, 0, 0, UIntPtr.Zero);
            keybd_event(VK_ALT, 0, 0, UIntPtr.Zero);
            keybd_event(VK_DELETE, 0, 0, UIntPtr.Zero);
            keybd_event(VK_DELETE, 0, 2, UIntPtr.Zero);
            keybd_event(VK_ALT, 0, 2, UIntPtr.Zero);
            keybd_event(VK_CONTROL, 0, 2, UIntPtr.Zero);
        }

        // Реальное управление питанием
        private void ExecuteShutdown(string args)
        {
            Process.Start(new ProcessStartInfo("shutdown", args) { 
                CreateNoWindow = true, 
                UseShellExecute = false 
            });
        }
    }
}
