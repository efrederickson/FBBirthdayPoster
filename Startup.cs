/*
 * User: elijah
 * Date: 10/26/2012
 * Time: 16:37
 * Copyright 2012 LoDC
 */
using System;
using Microsoft.Win32;

namespace Facebooker
{
    /// <summary>
    /// Description of Startup.
    /// </summary>
    public class Startup
    {
        public static void Check()
        {
            RegistryKey k = Registry.CurrentUser;
            // HKEY_CURRENT_USER\\MICROSOFT\\WINDOWS\\CURRENTVERSION\\RUN
            k = k.OpenSubKey("SOFTWARE\\MICROSOFT\\WINDOWS\\CURRENTVERSION\\RUN", true);
            if (k != null)
            {
                object o = k.GetValue("FBBirthdayPoster");
                if (o == null)
                {
                    k.SetValue("FBBirthdayPoster", "\"" + typeof(Startup).Assembly.Location + "\"");
                }
                k.Close();
            }
        }
        
        public static bool Rantoday()
        {
            RegistryKey k = Registry.CurrentUser;
            // HKEY_CURRENT_USER\\MICROSOFT\\WINDOWS\\CURRENTVERSION\\RUN
            k = k.CreateSubKey("SOFTWARE\\LODC\\FBBDAYPOSTER");
            if (k != null)
            {
                object o = k.GetValue("LastRan");
                if (o == null)
                {
                    k.SetValue("LastRan", DateTime.Now.ToString());
                }
                else
                {
                    DateTime last = DateTime.Parse(o.ToString());
                    if (last.Day == DateTime.Now.Day
                        && last.Month == DateTime.Now.Month 
                        && last.Year == DateTime.Now.Year)
                        return true;
                    else
                        k.SetValue("LastRan", DateTime.Now.ToString());
                }
                k.Close();
            }
            return false;
        }
    }
}
