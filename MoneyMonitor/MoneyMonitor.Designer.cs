using System.Diagnostics;
using System.Windows.Forms;

namespace MoneyMonitor
{
    partial class MoneyMonitor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                // Release the icon resource.
                m_RunningIcon.Dispose();
                m_IdleIcon.Dispose();
                m_StartingIcon.Dispose();
                m_ExitingIcon.Dispose();
                m_TrayIcon.Dispose();


                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick+=timer1_Tick;

            // 
            // MoneyMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "MoneyMonitor";
            this.Text = "Money Monitor";
            this.ResumeLayout(false);

        }

        void timer1_Tick(object sender, System.EventArgs e)
        {
            switch (this.m_State)
            {
                case tState.eState_Idle:
                    if (MoneyRunning())
                        ChangeState(tState.eState_Starting);
                    break;

                case tState.eState_Starting:
                    if (MoneyRunning())
                    {
                        if (MoneyWindowExists())
                            ChangeState(tState.eState_Running);
                    }
                    else
                        ChangeState(tState.eState_Idle);
                    break;

                case tState.eState_Running:
                    if (MoneyRunning())
                    {
                        if (!MoneyWindowExists())
                            ChangeState(tState.eState_Exiting);
                    }
                    else
                        ChangeState(tState.eState_Idle);
                    break;

                case tState.eState_Exiting:
                    if (MoneyRunning())
                    {
                        if (m_Warning==null || !m_Warning.Visible)
                        {
                            m_Warning = new WarningForm();
                            m_Warning.Show();
                        }
                    }
                    else
                    {
                        if (m_Warning.Visible)
                            m_Warning.DoClose();

                        ChangeState(tState.eState_Idle);
                    }

                    break;
            }
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
    }
}

