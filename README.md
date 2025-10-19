🧩 API1 – Authentication & Authorization API
📌 Overview

This project is a .NET Web API that handles user authentication and authorization.
It supports traditional email/password login, Google Sign-In, and uses JWT tokens with refresh tokens for secure session management.

🚀 Features

✅ User Registration with Email Confirmation
✅ User Login (with JWT + Refresh Token)
✅ Refresh Token endpoint
✅ Google Sign-In Integration
✅ Logout (clear user refresh tokens)
✅ Forgot Password & Reset Password
✅ Email Confirmation via MailKit
✅ ASP.NET Identity Integration

🧱 Tech Stack

ASP.NET Core 8.0

Entity Framework Core

Microsoft Identity

JWT Authentication

Google Auth API

MailKit (for email confirmation)

SQL Server (as the main database)

⚙️ Endpoints Summary
🔹 Authentication
Method	Endpoint	Description
POST	/api/account/register	Register a new user
POST	/api/account/login	Login with username & password
POST	/api/account/refresh	Refresh JWT using refresh token
POST	/api/account/logout	Logout and clear all refresh tokens
POST	/api/account/resend-email	Resend email confirmation
POST	/api/account/confirm-email	Confirm user email
POST	/api/account/forgot-password	Send password reset email
POST	/api/account/reset-password	Reset user password
🔹 Google Sign-In
Method	Endpoint	Description
POST	/api/account/google-signin	Login or register using Google account
🔐 Authentication Flow

User registers with email & password → receives a confirmation email.

Once confirmed, the user logs in and receives:

Access Token (JWT) → short-lived

Refresh Token → long-lived

When the JWT expires, the client calls /refresh to get a new token pair.

On logout, all refresh tokens are revoked (user must re-login).

Google Sign-In users authenticate using their Google ID token.

🧩 Google Sign-In Setup

To enable Google Sign-In:

Go to Google Cloud Console
.

Create a OAuth 2.0 Client ID for your platform (Web or Mobile).

Add the Client ID to the backend validation logic (in GoogleSignInAsync).

🧠 Example Response

Login Success:

{
  "isSuccess": true,
  "jwtToken": "<access_token>",
  "refreshToken": "<refresh_token>",
  "expiration": "2025-10-20T12:00:00Z",
  "message": "Login Success"
}


Refresh Token Response:

{
  "isSuccess": true,
  "jwtToken": "<new_access_token>",
  "refreshToken": "<new_refresh_token>",
  "message": "Token refreshed"
}

📁 Project Structure
Api1/
│
├── Controllers/
│   └── AccountController.cs
│
├── Services/
│   ├── AccountServices.cs
│   ├── JwtServices.cs
│   ├── EmailServices.cs
│
├── Models/
│   ├── ApplicationUser.cs
│   └── RefreshTokens.cs
│
├── DTO/
│   ├── RegisterModel.cs
│   ├── LoginModel.cs
│   ├── GoogleSignInDto.cs
│   └── GeneralResponse.cs
│
├── Context/
│   └── Api1Context.cs
│
└── Program.cs

🧾 License

This project is open source and free to use for learning or development purposes.
