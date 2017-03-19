using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Principal;
using System.Diagnostics;
using System.ComponentModel;

namespace CreateATestFile
{
    class Program
    {
        static void Main(string[] args)
        {
            GetAndDisplayRights();
        }


        private static void DoStuff()
        {
            try
            {
                var text = "YOU HAVE BEEN HACKED!";
                var path = @"C:\Windows\hacked.txt";

                Console.WriteLine(String.Format("Target file: {0}", path));

                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(text);
                    Console.WriteLine(String.Format("Wrote the text to file: {0}", text));


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            Console.Read();
        }


        private static void GetAndDisplayRights()
        {
            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);

            if (hasAdministrativeRight)
            {
                DoStuff();
            }
            else
            {
                btnElevate_Click();
            }



            
        }

        private static void btnElevate_Click()
        {
            RunElevated(System.Reflection.Assembly.GetExecutingAssembly().Location);
                      
        }


        private static void RunElevated(string fileName)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.Verb = "runas";
            processInfo.FileName = fileName;
            try
            {
                Process.Start(processInfo);
            }
            catch (Win32Exception)
            {
                //Do nothing. Probably the user canceled the UAC window
            }
        }
    }
}
