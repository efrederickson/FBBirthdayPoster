using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebooker.FacebookObjects
{
    public class Message
    {
        public string From
        {
            get
            {
                return json["from"]["name"];
            }
        }

        public string ID
        {
            get
            {
                return json["id"];
            }
        }

        public string To
        {
            get
            {
                foreach (dynamic obj in json["to"]["data"])
                {
                    if (obj["id"] != json["from"]["id"])
                        return obj["name"];
                }
                return string.Empty;
            }
        }

        public string Content
        {
            get
            {
                return json["message"];
            }
        }

        public DateTime TimeSent
        {
            get
            {
                return DateTime.Parse(json["created_time"]);
            }
        }

        dynamic json;

        public Message(dynamic j)
        {
            this.json = j;
        }

        public override string ToString()
        {
            return string.Format("To: {0}, \nFrom: {1}, \nID: {2}, \nMessage: {3}",
                To, From, ID, Content);
        }
    }
}
