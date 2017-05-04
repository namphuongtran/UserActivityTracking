using System.ServiceProcess;

namespace UAT
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            try
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new UATService()
                };
                ServiceBase.Run(ServicesToRun);
            }
            catch (System.Exception ex)
            {
                Logging.WriteLog(ex.Message);
            }
            
        }
    }
}
