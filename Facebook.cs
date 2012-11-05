using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Facebook;
using System.Dynamic;
using Facebooker.FacebookObjects;

namespace Facebooker
{
    public static partial class Facebook
    {
        private const string ExtendedPermissions = "user_about_me,publish_stream,read_mailbox,read_stream,user_status,friends_status,friends_birthday,user_birthday"; 
        //user_activies,user_birthday,user_checkins,user_education_history,user_events,user_groups,user_hometown,user_interests,user_likes,user_location,user_notes,user_photos,user_questions,user_relationships,user_relationship_details,user_religion_politics,user_status,user_videos,user_website,user_work_history,email,read_friendlists,read_insights,read_mailbox,read_requests,manage_notifications,xmpp_login,create_event,manage_friendlists,user_online_presence,friends_online_presence,publish_checkins,publish_stream,rsvp_event";
        static bool LoggedIn = false;
        static bool wasLoginCancelled = true;
        static string AccessToken = "";
        static FacebookClient fb = null;

        public static void Login()
        {
            // open the Facebook Login Dialog and ask for user permissions.
            var fbLoginDlg = new FacebookLogin(API_KEY, ExtendedPermissions);
            fbLoginDlg.ShowDialog();

            // The user has taken action, either allowed/denied or cancelled the authorization,
            // which can be known by looking at the dialogs FacebookOAuthResult property.
            // Depending on the result take appropriate actions.
            CheckLoginResults(fbLoginDlg.FacebookOAuthResult);
        }

        private static void CheckLoginResults(FacebookOAuthResult facebookOAuthResult)
        {
            if (facebookOAuthResult == null)
            {
                // the user closed the FacebookLoginDialog, so do nothing.
                throw new Exception("Login was cancelled!");
            }

            // Even though facebookOAuthResult is not null, it could had been an 
            // OAuth 2.0 error, so make sure to check IsSuccess property always.
            if (facebookOAuthResult.IsSuccess)
            {
                // since our respone_type in FacebookLoginDialog was token,
                // we got the access_token
                // The user now has successfully granted permission to our app.
                LoggedIn = true;
                AccessToken = facebookOAuthResult.AccessToken;
                wasLoginCancelled = false;
                fb = new FacebookClient(AccessToken);
                fb.AppId = API_KEY;
                fb.AppSecret = APP_SECRET;
            }
            else
            {
                // for some reason we failed to get the access token.
                // most likely the user clicked don't allow.
                wasLoginCancelled = false;
                throw new Exception(facebookOAuthResult.ErrorDescription);
            }
        }

        public static void PostToWall(string msg)
        {
            if (wasLoginCancelled == true)
                Login();
            if (LoggedIn != true)
                throw new Exception("User is not logged in!");

            var fb = new FacebookClient(AccessToken);

            // make sure to add event handler for PostCompleted.
            fb.PostCompleted += (o, e) =>
            {
                // incase you support cancellation, make sure to check
                // e.Cancelled property first even before checking (e.Error!=null).
                if (e.Cancelled)
                {
                    // for this example, we can ignore as we don't allow this
                    // example to be cancelled.

                    // you can check e.Error for reasons behind the cancellation.
                    throw e.Error;
                }
                else if (e.Error != null)
                {
                    // error occurred
                    throw e.Error;
                }
                else
                {
                    // the request was completed successfully

                    // now we can either cast it to IDictionary<string, object> or IList<object>
                    // depending on the type. or we could use dynamic.
                    dynamic result = e.GetResultData();
                    //_lastMessageId = result.id;
                }
            };

            dynamic parameters = new ExpandoObject();
            parameters.message = msg;

            fb.PostAsync("me/feed", parameters);
        }

        public static void PostLink(string msg, string url)
        {
            if (LoggedIn == false)
                throw new Exception("User is not logged in!");

            var fb = new FacebookClient(AccessToken);

            // make sure to add event handler for PostCompleted.
            fb.PostCompleted += (o, e) =>
            {
                // incase you support cancellation, make sure to check
                // e.Cancelled property first even before checking (e.Error!=null).
                if (e.Cancelled)
                {
                    // for this example, we can ignore as we don't allow this
                    // example to be cancelled.

                    // you can check e.Error for reasons behind the cancellation.
                    throw e.Error;
                }
                else if (e.Error != null)
                {
                    // error occurred
                    throw e.Error;
                }
                else
                {
                    // the request was completed successfully

                    // now we can either cast it to IDictionary<string, object> or IList<object>
                    // depending on the type. or we could use dynamic.
                    dynamic result = e.GetResultData();
                    //_lastMessageId = result.id;
                }
            };

            dynamic parameters = new ExpandoObject();
            parameters.comment = msg;
            parameters.url = url;

            fb.PostAsync("links.post", parameters);
        }

        public static string Name
        {
            get
            {
                dynamic result = fb.Get("me");
                return result["name"].ToString();
            }
        }

        public static string FirstName
        {
            get
            {
                dynamic result = fb.Get("me");
                return result["first_name"].ToString();
            }
        }

        public static string LastName
        {
            get
            {
                dynamic result = fb.Get("me");
                return result["last_name"].ToString();
            }
        }

        public static long NumberOfUnreadMessages
        {
            get
            {
                dynamic o = fb.Get("/me/inbox");
                //System.Windows.MessageBox.Show(o.ToString());
                return o["summary"]["unread_count"];
                //return o["data"].Count;
            }
        }

        public static FacebookObjects.Message[] Messages
        {
            get
            {
                dynamic o = fb.Get("/me/inbox");
                List<FacebookObjects.Message> msgs = new List<FacebookObjects.Message>();
                for (int i = 0; i < o["data"].Count; i++)
                    msgs.Add(new FacebookObjects.Message(o["data"][i]));
                    //return new FacebookObjects.Message(o["data"][0]);
                return msgs.ToArray();
            }
        }

        public static FacebookObjects.NewsFeedEntry[] NewsFeed
        {
            get
            {
                List<FacebookObjects.NewsFeedEntry> ret = new List<FacebookObjects.NewsFeedEntry>();
                dynamic o = fb.Get("/me/feed");
                //System.IO.File.WriteAllText("out.log", o.ToString());
                foreach (dynamic o2 in o["data"])
                    ret.Add(new FacebookObjects.NewsFeedEntry(o2));
                return ret.ToArray();
            }
        }
        
        public static User[] Friends
        {
            get
            {
                List<User> ret = new List<User>();
                dynamic f = fb.Get("me/friends?fields=id,name,birthday");
                
                //MessageBox.Show(f.ToString());
                
                foreach (dynamic o2 in f["data"])
                    ret.Add(new User(o2));
                
                return ret.ToArray();
            }
        }
        
        public static void PostToFriendsWall(User u, string msg)
        {
            if (wasLoginCancelled == true)
                Login();
            if (LoggedIn != true)
                throw new Exception("User is not logged in!");

            var fb = new FacebookClient(AccessToken);

            // make sure to add event handler for PostCompleted.
            fb.PostCompleted += (o, e) =>
            {
                if (e.Cancelled)
                {
                    throw e.Error;
                }
                else if (e.Error != null)
                {
                    throw e.Error;
                }
                else
                {
                    dynamic result = e.GetResultData();
                }
            };

            dynamic parameters = new ExpandoObject();
            parameters.message = msg;

            //MessageBox.Show("post to " + u.ID + "'s wall");
            fb.PostAsync("/" + u.ID + "/feed", parameters);
        }
    }
}
