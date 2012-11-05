using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebooker.FacebookObjects
{
    public class User
    {
        public string Name
        {
            get
            {
                return json["name"];
            }
        }
        public string ID
        {
            get
            {
                return json["id"];
            }
        }
        
        public DateTime Birthday
        {
            get
            {
                DateTime x;
                try 
                {
                    if (DateTime.TryParse(json["birthday"].ToString(), out x))
                    return x;
                } 
                catch (Exception) 
                {
                }
                
                return DateTime.MinValue;
            }
        }

        dynamic json;
        public User(dynamic json)
        {
            this.json = json;
        }
    }
}
