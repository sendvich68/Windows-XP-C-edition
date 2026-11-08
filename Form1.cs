using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinXPEmulator
{
    public partial class Form1 : Form
    {
        // Импорт WinAPI для эмуляции нажатий клавиш
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        const byte VK_LWIN = 0x5B;
        const byte VK_TAB = 0x09;
        const byte VK_ESCAPE = 0x1B;
        const byte VK_CONTROL = 0x11;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Windows XP Emulator (C#)";
            this.BackColor = System.Drawing.Color.FromArgb(58, 110, 165); // Синий XP
            SetupUI();
        }

        private void SetupUI()
        {
            // Кнопка "Пуск" (эмуляция)
            Button btnStart = new Button();
            btnStart.Text = "Пуск";
            btnStart.Location = new System.Drawing.Point(10, 10);
            btnStart.Size = new System.Drawing.Size(80, 30);
            btnStart.Click += (s, e) => EmulateKey(VK_LWIN);
            this.Controls.Add(btnStart);

            // Кнопка "Выключить ПК" (реальное выключение)
            Button btnShutdown = new Button();
            btnShutdown.Text = "Выключить ПК";
            btnShutdown.Location = new System.Drawing.Point(100, 10);
            btnShutdown.Size = new System.Drawing.Size(120, 30);
            btnShutdown.Click += (s, e) => ExecuteShutdown("/s /t 0");
            this.Controls.Add(btnShutdown);

            // Кнопка "Перезагрузка"
            Button btnRestart = new Button();
            btnRestart.Text = "Перезагрузка";
            btnRestart.Location = new System.Drawing.Point(230, 10);
            btnRestart.Size = new System.Drawing.Size(120, 30);
            btnRestart.Click += (s, e) => ExecuteShutdown("/r /t 0");
            this.Controls.Add(btnRestart);

            // Кнопка "Alt+Tab" (переключение окон)
            Button btnAltTab = new Button();
            btnAltTab.Text = "Alt+Tab";
            btnAltTab.Location = new System.Drawing.Point(10, 60);
            btnAltTab.Size = new System.Drawing.Size(80, 30);
            btnAltTab.Click += (s, e) => SimulateAltTab();
            this.Controls.Add(btnAltTab);
        }

        // Эмуляция одиночного нажатия (например, Win)
        private void EmulateKey(byte key)
        {
            keybd_event(key, 0, 0, UIntPtr.Zero);
            keybd_event(key, 0, 2, UIntPtr.Zero); // KEYEVENTF_KEYUP
        }

        // Эмуляция Alt+Tab
        private void SimulateAltTab()
        {
            keybd_event(0x12, 0, 0, UIntPtr.Zero); // Alt down
            keybd_event(VK_TAB, 0, 0, UIntPtr.Zero); // Tab down
            keybd_event(VK_TAB, 0, 2, UIntPtr.Zero); // Tab up
            keybd_event(0x12, 0, 2, UIntPtr.Zero); // Alt up
        }

        // Реальное управление питанием через shutdown.exe
        private void ExecuteShutdown(string args)
        {
            Process.Start(new ProcessStartInfo("shutdown", args) { 
                CreateNoWindow = true, 
                UseShellExecute = false 
            });
        }
    }
}
