using System;
using System.Net;
using System.Threading.Tasks;
using Firebase.Auth;
using UnityEngine;

namespace FirebaseIntegration
{
    public class FirebaseGoogleSignIn : MonoBehaviour
    {
        [Header("Google OAuth Settings")]
        [Tooltip("Google Client ID from Firebase Console.")]
        [SerializeField] private string webClientId;
        [Tooltip("Redirect URI configured in Firebase Console.")]
        [SerializeField] private string redirectUri;

        private HttpListener httpListener;

        private void Start() 
        {
#if UNITY_EDITOR
            InitializeHttpListener();
#endif
        }

        public async Task StartGoogleSignIn(Action<FirebaseUser> callback)
        {
            var nonce = Guid.NewGuid().ToString();
            var oauthUrl = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={webClientId}&redirect_uri={redirectUri}&response_type=token%20id_token&scope=email%20profile%20openid&nonce={nonce}";
            Application.OpenURL(oauthUrl);

            Application.deepLinkActivated += OnDeepLinkActivated;

            while (FirebaseAuth.DefaultInstance.CurrentUser == null)
            {
                await Task.Yield();
            }

            callback(FirebaseAuth.DefaultInstance.CurrentUser);
        }

        private static void OnDeepLinkActivated(string url)
        {
            if (!url.StartsWith("your-app-scheme://auth")) return;

            Application.deepLinkActivated -= OnDeepLinkActivated;
            var uri = new Uri(url);
            var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var idToken = queryParams["id_token"];
            var accessToken = queryParams["access_token"];
            AuthenticateWithFirebase(idToken, accessToken);
        }
        
        private void InitializeHttpListener() 
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:5000/"); // Remember to add this in Google Console if you want to test in the Editor
            httpListener.Start();
            httpListener.BeginGetContext(OnRequestReceived, null);
            return;

            void OnRequestReceived(IAsyncResult result)
            {
                var context = httpListener.EndGetContext(result);
                var idToken = context.Request.QueryString["id_token"];
                var accessToken = context.Request.QueryString["access_token"];
                Debug.Log("Received OAuth Token: " + accessToken);

                httpListener.Stop();
                httpListener.Close();
                AuthenticateWithFirebase(idToken, accessToken);
            }
        }

        private static async void AuthenticateWithFirebase(string idToken, string accessToken)
        {
            try
            {
                var credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
                var newUser = await FirebaseAuth.DefaultInstance.SignInWithCredentialAsync(credential);
                Debug.Log($"Firebase sign-in successful: {newUser.DisplayName}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Firebase authentication failed: {e.Message}");
            }
        }
    }
}
