using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLauncherCloud_Client
{
    // FirebaseAuthentication.net https://github.com/step-up-labs/firebase-authentication-dotnet
    //https://support.ably.io/support/solutions/articles/3000065278-where-can-i-find-my-google-firebase-cloud-messaging-api-key-
    class FirebaseAuthenticationHandler
    {
        private const string PathToApiKey = @"./Resources/firebaseAPIKey.txt";
        private const string PathToUser = @"./Resources/firebaseuser.txt";
        private string firebaseApiKey;
        private string[] userInfo;

        public FirebaseAuthenticationHandler()
        {
        }

        public bool Start()
        {
            // Load the auth id from the file.
            if (!File.Exists(PathToApiKey))
            {
                return false;
            }
            firebaseApiKey = File.ReadAllText(PathToApiKey).Trim();

            // Load the user info from the file.
            if (!File.Exists(PathToUser))
            {
                return false;
            }
            userInfo = File.ReadAllText(PathToUser).Trim().Split(';');

            return true;
        }

        public async Task<FirebaseAuthLink> GetAuth()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(firebaseApiKey));
            FirebaseAuthLink auth = await authProvider.SignInWithEmailAndPasswordAsync(userInfo[0], userInfo[1]);

            return auth;
        }
    }
}
