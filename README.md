# Unity-FirebaseGoogleSignIn
A Unity C# script for integrating a Web-Based Google Sign-In with Firebase Authentication into Android/iOS applications.

![MIT License](https://img.shields.io/badge/license-MIT-green.svg)

## Introduction

This script provides a **web-based solution** for integrating Google Sign-In with Unity on Android and iOS platforms. After struggling with the complexities of Google Sign-In integration in Unity for years, I decided to build a more streamlined and adaptable approach.

The script allows you to **redirect the user to a browser**, where Google will handle the authentication process. After the user signs in, they are redirected back to a custom URL (which you set up in Google Cloud Console), and the tokens are sent to your Unity app. With these tokens, you can easily use the Firebase API to retrieve user information.

Once the initial configuration is complete—such as installing the Firebase SDK for Unity, setting up OAuth2 in the Google Cloud Console, and configuring deep linking for Android and iOS—the solution works smoothly across both platforms, with minimal impact on the user experience.

## Features

- **Cross-Platform Support**: Works seamlessly on both Android and iOS.
- **Minimal User Impact**: The browser-based authentication flow is quick and requires little effort from users.
- **Token Management**: Automatically retrieves authentication tokens for use with Firebase.
- **Easy Setup**: Clear configuration steps for deep linking and OAuth.
- **Local Testing in Unity Editor**: You can set up an **HttpListener** to localhost in the Unity Editor to test the redirect functionality without deploying to a device. This makes testing more efficient during development.

## Setup Instructions

### Step 1: Install Firebase SDK for Unity

Ensure you have the Firebase SDK installed in your Unity project. You can follow the official Firebase Unity documentation to set it up:
- [Firebase Unity Setup](https://firebase.google.com/docs/unity/setup)

### Step 2: Configure OAuth2 in Google Cloud Console

1. Navigate to the [Google Cloud Console](https://console.cloud.google.com/).
2. Create a new project or select an existing one.
3. Go to **APIs & Services** > **Credentials**.
4. Click **Create Credentials** and choose **OAuth 2.0 Client IDs**.
5. Set the application type as **Web Application** and add your **redirect URIs**:
    - For iOS: `yourapp://auth`
    - For Android: Your custom URL like `yourapp://auth`

6. Note down the **Client ID** and **Redirect URI** for later use.

### Step 3: Configure Android/iOS Deep Linking

Deep linking is essential for sending authentication tokens back to the Unity app. Here's how to set it up for both platforms:

#### Android Deep Linking Configuration

1. Open your **AndroidManifest.xml** file located in `Assets/Plugins/Android/`.
2. Add an intent filter inside your `<activity>` tag:

   ```xml
   <intent-filter>
      <action android:name="android.intent.action.VIEW" />
      <category android:name="android.intent.category.DEFAULT" />
      <category android:name="android.intent.category.BROWSABLE" />
      <data android:scheme="yourapp" android:host="auth" />
   </intent-filter>
   ```

   - Replace `"yourapp"` with your custom scheme, if necessary.

#### iOS Deep Linking Configuration

1. You can follow Unity's official documentation [here]([https://console.cloud.google.com/](https://docs.unity3d.com/Manual/deep-linking-ios.html)).

### Step 4: Implement the Script

Copy and paste the `FirebaseGoogleSignIn.cs` script into your Unity project. Set the required fields in the Unity Inspector, such as:

- `webClientId`: The client ID you retrieved from the Google Cloud Console.
- `redirectUri`: The redirect URI you configured for the OAuth client.

### Usage

1. Call the `StartGoogleSignIn` method to initiate the authentication process. The user will be redirected to a browser to authenticate via Google.
2. After authentication, Google will send the tokens to the configured URL, and the script will handle the token retrieval.
3. The script will authenticate the user with Firebase using the retrieved tokens, allowing you to access user information.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Why This Script Exists

After years of struggling with the complexity and evolving requirements of Google Sign-In integration in Unity, I built this web-based solution to simplify the process. Google's Unity SDKs can be frustrating, especially when dealing with native-level configurations on both Android and iOS.

The goal of this script is to provide an **easy-to-implement** and **cross-platform** solution that works reliably. It removes the need for complex native integrations by leveraging a web-based authentication flow. After configuring your project in the **Google Cloud Console** and setting up deep linking for iOS and Android, this solution will handle user authentication with minimal impact on the user experience.

It works well for projects requiring Firebase-based authentication and significantly reduces the time needed to implement this crucial feature in your Unity apps.
