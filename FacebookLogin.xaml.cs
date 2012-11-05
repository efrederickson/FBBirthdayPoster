using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Facebook;
using System.Windows.Navigation;

namespace Facebooker
{
    /// <summary>
    /// Interaction logic for FacebookLogin.xaml
    /// </summary>
    public partial class FacebookLogin : Window
    {
        public FacebookLogin()
        {
            InitializeComponent();
        }

        private readonly Uri _loginUrl;
        protected FacebookClient _fb;

        public FacebookOAuthResult FacebookOAuthResult { get; private set; }

        public FacebookLogin(string appId, string extendedPermissions)
            : this(new FacebookClient(), appId, extendedPermissions)
        {
        }

        public FacebookLogin(FacebookClient fb, string appId, string extendedPermissions)
        {
            if (fb == null)
                throw new ArgumentNullException("fb");
            if (string.IsNullOrWhiteSpace(appId))
                throw new ArgumentNullException("appId");

            _fb = fb;
            _loginUrl = GenerateLoginUrl(appId, extendedPermissions);

            InitializeComponent();
            webBrowser1.Navigated += new System.Windows.Navigation.NavigatedEventHandler(webBrowser1_Navigated);
            this.Loaded += new RoutedEventHandler(FacebookLogin_Loaded);
        }

        void FacebookLogin_Loaded(object sender, RoutedEventArgs e)
        {
            // make sure to use AbsoluteUri.
            webBrowser1.Navigate(_loginUrl.AbsoluteUri);
        }
        private Uri GenerateLoginUrl(string appId, string extendedPermissions)
        {
            dynamic parameters = new System.Dynamic.ExpandoObject();
            parameters.client_id = appId;
            parameters.redirect_uri = "https://www.facebook.com/connect/login_success.html";

            // The requested response: an access token (token), an authorization code (code), or both (code token).
            parameters.response_type = "token";

            // list of additional display modes can be found at http://developers.facebook.com/docs/reference/dialogs/#display
            parameters.display = "popup";

            // add the 'scope' parameter only if we have extendedPermissions.
            if (!string.IsNullOrWhiteSpace(extendedPermissions))
                parameters.scope = extendedPermissions;

            // when the Form is loaded navigate to the login url.
            return _fb.GetLoginUrl(parameters);
        }

        private void webBrowser1_Navigated(object sender, NavigationEventArgs e)
        {
            // whenever the browser navigates to a new url, try parsing the url.
            // the url may be the result of OAuth 2.0 authentication.

            FacebookOAuthResult oauthResult;
            if (_fb.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
            {
                // The url is the result of OAuth 2.0 authentication
                FacebookOAuthResult = oauthResult;
                DialogResult = FacebookOAuthResult.IsSuccess ? true : false;
            }
            else
            {
                // The url is NOT the result of OAuth 2.0 authentication.
                FacebookOAuthResult = null;
            }
        }
    }
}
