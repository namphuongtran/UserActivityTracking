using System;
using System.ServiceProcess;
using System.Management;

namespace UAT
{
    public partial class UATService : ServiceBase
    {
        private string userName = string.Empty;

        public UATService()
        {
            InitializeComponent();
            this.CanHandleSessionChangeEvent = true;
            this.AutoLog = true;
            this.CanHandlePowerEvent = true;
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            string messageInfo = string.Empty;
            switch (powerStatus)
            {
                case PowerBroadcastStatus.BatteryLow:
                    messageInfo = "Battery power is low.";
                    break;
                case PowerBroadcastStatus.OemEvent:
                    messageInfo = "BIOS signaled an APM OEM event.";
                    break;
                case PowerBroadcastStatus.PowerStatusChange:
                    messageInfo = "A change in the power status of the computer is detected.";
                    break;
                case PowerBroadcastStatus.QuerySuspend:
                    messageInfo = "The system has requested permission to suspend the computer.";
                    break;
                case PowerBroadcastStatus.QuerySuspendFailed:
                    messageInfo = "The system was denied permission to suspend the computer.";
                    break;
                case PowerBroadcastStatus.ResumeAutomatic:
                    messageInfo = "The computer has woken up automatically to handle an event.";
                    break;
                case PowerBroadcastStatus.ResumeCritical:
                    messageInfo = "The system has resumed operation after a critical suspension caused by a failing battery.";
                    break;
                case PowerBroadcastStatus.ResumeSuspend:
                    messageInfo = "The system has resumed operation after being suspended.";
                    break;
                case PowerBroadcastStatus.Suspend:
                    messageInfo = "The computer is about to enter a suspended state.";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(messageInfo))
            {
                string userName = GetUserName();
                string machineName = Environment.MachineName;
                string dateTime = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                string activity = messageInfo;
                Logging.WriteLog(userName, machineName, dateTime, activity);
            }
            return base.OnPowerEvent(powerStatus);
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            string messageInfo = string.Empty;
            switch (changeDescription.Reason)
            {
                case SessionChangeReason.SessionLock:
                    messageInfo = "Your computer Locked.";
                    break;
                case SessionChangeReason.SessionLogoff:
                    messageInfo = "Your computer Logoff.";
                    break;
                case SessionChangeReason.SessionLogon:
                    messageInfo = "Your computer Logon.";
                    break;
                case SessionChangeReason.SessionUnlock:
                    messageInfo = "Your computer Unlock.";
                    break;
                case SessionChangeReason.ConsoleConnect:
                    messageInfo = "A session has been connected from the console.";
                    break;
                case SessionChangeReason.ConsoleDisconnect:
                    messageInfo = "A session has been disconnected from the console.";
                    break;
                case SessionChangeReason.RemoteConnect:
                    messageInfo = "A session has been connected from a remote connection.";
                    break;
                case SessionChangeReason.RemoteDisconnect:
                    messageInfo = "A session has been disconnected from a remote connection.";
                    break;
                case SessionChangeReason.SessionRemoteControl:
                    messageInfo = "A session has changed its status to or from remote controlled mode.";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(messageInfo))
            {
                string userName = !string.IsNullOrEmpty(GetUserName()) ? GetUserName() : Environment.GetEnvironmentVariable("UserName");
                string machineName = Environment.MachineName;
                string dateTime = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                string activity = messageInfo;
                Logging.WriteLog(userName, machineName, dateTime, activity);
            }
            base.OnSessionChange(changeDescription);
        }

        private string GetUserName()
        {
            string userName = string.Empty;
            ManagementObjectSearcher Processes = new ManagementObjectSearcher("SELECT * FROM Win32_Process");
            foreach (ManagementObject Process in Processes.Get())
            {
                if (Process["ExecutablePath"] != null &&
                    System.IO.Path.GetFileName(Process["ExecutablePath"].ToString()).ToLower() == "explorer.exe")
                {
                    string[] OwnerInfo = new string[2];
                    Process.InvokeMethod("GetOwner", (object[])OwnerInfo);

                    userName = OwnerInfo[0];
                    break;
                }
            }
            if (!string.IsNullOrEmpty(userName))
            {
                this.userName = userName;
            }
            else
            {
                if (!string.IsNullOrEmpty(this.userName))
                {
                    userName = this.userName;
                }
            }
            return userName;
        }

        protected override void OnStart(string[] args)
        {
            //Logging.WriteLog("User activity tracking services started.");
            this.userName = GetUserName();
        }

        protected override void OnStop()
        {
            //Logging.WriteLog("User activity tracking services stopped.");
        }
    }
}
