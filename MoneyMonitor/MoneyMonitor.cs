using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Automation;

namespace MoneyMonitor
{
    public partial class MoneyMonitor : Form
    {
        private enum tState
        {
            eState_Idle,
            eState_Starting,
            eState_Running,
            eState_Exiting,
        };

        public MoneyMonitor()
        {
            this.m_State = tState.eState_Idle;

            InitializeComponent();

            // Create a simple tray menu with only one item.
            m_TrayMenu = new ContextMenu();
            m_TrayMenu.MenuItems.Add("Exit", OnExit);

            m_IdleIcon = new Icon(Properties.Resources.Idle, new Size(32,32));
            m_StartingIcon = new Icon(Properties.Resources.Starting, new Size(32, 32));
            m_RunningIcon = new Icon(Properties.Resources.Running, new Size(32, 32));
            m_ExitingIcon = new Icon(Properties.Resources.Exiting, new Size(32, 32));

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            m_TrayIcon = new NotifyIcon();
            m_TrayIcon.Text = CurrentStateText();
            m_TrayIcon.Icon = m_IdleIcon;

            // Add menu to tray icon and show it.
            m_TrayIcon.ContextMenu = m_TrayMenu;
            m_TrayIcon.Visible = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnShow(object sender, EventArgs e)
        {
            if (m_Warning == null || !m_Warning.Visible)
            {
                m_Warning = new WarningForm();
                m_Warning.Show();
            }
        }

        private void OnHide(object sender, EventArgs e)
        {
            if (m_Warning!=null)
                m_Warning.DoClose();
        }

        private string CurrentStateText()
        {
            string Ret="Money Monitor - ";

            switch (this.m_State)
            {
                case tState.eState_Idle:
                    Ret += "Idle";
                    break;

                case tState.eState_Starting:
                    Ret += "Starting";
                    break;

                case tState.eState_Running:
                    Ret += "Running";
                    break;

                case tState.eState_Exiting:
                    Ret += "Exiting";
                    break;
            }

            Debug.WriteLine("Returning state " + Ret);
            return Ret;
        }

        private bool MoneyRunning()
        {
            bool Ret = false;

            Process[] pname = Process.GetProcessesByName("msmoney");
            if (pname.Length != 0)
                Ret = true;

            return Ret;
        }

        private bool MoneyWindowExists()
        {
            bool Ret = false;

            Process[] pname = Process.GetProcessesByName("msmoney");
            if (pname.Length != 0)
            {
                Process Money = pname[0];

                if (Money.MainWindowHandle != IntPtr.Zero)
                {
                    AutomationElement Window = AutomationElement.FromHandle(Money.MainWindowHandle);

                    Debug.WriteLine("Offscreen: " + Window.Current.IsOffscreen);

                    if (!Window.Current.IsOffscreen)
                        Ret = true;
                }
            }

            return Ret;
        }

        private void ChangeState(tState NewState)
        {
            if (NewState != this.m_State)
            {
                this.m_State = NewState;
                m_TrayIcon.Text = CurrentStateText();
                m_TrayIcon.Icon = CurrentStateIcon();
            }
        }

        private System.Drawing.Icon CurrentStateIcon()
        {
            switch (this.m_State)
            {
                case tState.eState_Idle:
                    return m_IdleIcon;

                case tState.eState_Starting:
                    return m_StartingIcon;

                case tState.eState_Running:
                    return m_RunningIcon;

                case tState.eState_Exiting:
                    return m_ExitingIcon;
            }

            return m_IdleIcon;
        }

        private Icon m_IdleIcon;
        private Icon m_StartingIcon;
        private Icon m_RunningIcon;
        private Icon m_ExitingIcon;
        private NotifyIcon m_TrayIcon;
        private ContextMenu m_TrayMenu;
        private tState m_State;
        private WarningForm m_Warning;
    }
}
