using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facebooker.FacebookObjects
{
    public class NewsFeedEntry
    {
        public dynamic json;

        public string ID
        {
            get
            {
                return json["id"];
            }
        }

        public string From
        {
            get
            {
                return json["from"]["name"];
            }
        }

        public string Story
        {
            get
            {
                try
                {
                    return json["story"];
                }
                catch (Exception)
                {
                    try
                    {
                        return json["message"];
                    }
                    catch (Exception)
                    {
                        return "<UNKNOWN CONTENT TYPE>";
                    }
                }
            }
        }

        public string Picture
        {
            get
            {
                return json["picture"];
            }
        }

        public string Link
        {
            get
            {
                return json["link"];
            }
        }

        public string Type
        {
            get
            {
                return json["type"];
            }
        }

        public DateTime CreatedTime
        {
            get
            {
                return DateTime.Parse(json["created_time"].ToString());
            }
        }

        public DateTime UpdatedTime
        {
            get
            {
                return DateTime.Parse(json["updated_time"]);
            }
        }

        //"comments":{"count":0}

        public NewsFeedEntry(dynamic o)
        {
            json = o;
        }

        public override string ToString()
        {
            return string.Format("ID: {0},\nFrom: {1},\nStory: {2},\nTime: {3}",
                ID, From, Story, CreatedTime);
        }
    }
}
