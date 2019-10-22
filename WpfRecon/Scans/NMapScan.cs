﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;


namespace WpfRecon.Scans
{
    public class NMapScan
    {
    

        public async Task<string> RunScan(string IpAddress)
        {
            try
            {
               
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false;


                    //This will use the nmap external tool that is stored in the External Tools folder
                    //Running the nmap tool 
                    myProcess.StartInfo.FileName = "ExternalTools//nmap.exe";
                    //Running the options
                    var sb = new StringBuilder();
                    // Fast scan mode 
                    sb.Append("-T4 ");
                    
                    //if the mainpage All Ports checkbox was checked then it will run a all ports -p- argument
                    
                    if (MainPage.AP == true)
                    {
                        sb.Append("-p- ");
                      
                    }
                    else //if not checked then it will run a full enumeration scan on the target
                    {
                        //full enumeration scan 
                        sb.Append("-A ");
                        //smb enumeration as this port has a poor security track record.
                        //Brute force SSH, Telnet, FTP
                        sb.Append("--script ssh-brute,telnet-brute,ftp-brute,smb-os-discovery, ");

                        // Test popular ports 
                        sb.Append("-F ");
                    }
                    //if the Whole Network check box was ticked the scanner will scan a whole class C network.
                    if (MainPage.WN == true)
                    {
                        sb.Append(IpAddress + "/24 ");

                    }
                    //if not selected it will just target the IP Adress provided
                    else
                    {
                        sb.Append(IpAddress);
                    }
                    //add the arguments to the end of the nmap scan
                    myProcess.StartInfo.Arguments = sb.ToString();
                    //hide the window to avoid a popup
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.StartInfo.RedirectStandardOutput = true;
                    myProcess.StartInfo.RedirectStandardError = true;

                    // myProcess.StartInfo.CreateNoWindow = true;
                    
                    await Task.Run(() => myProcess.Start());
                    //myProcess.Start();
                    
                    var stdOutSb = new StringBuilder();
                    while (!myProcess.HasExited)
                    {
                        stdOutSb.Append(myProcess.StandardOutput.ReadToEnd());
                        stdOutSb.Append(myProcess.StandardError.ReadToEnd());
                    }
                                  

                    return stdOutSb.ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
        }
    }

    public class NmapEventHandler
    {
    }
}
