# 🧩 API1 – Authentication & Authorization API

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](#license)
[![Dotnet](https://img.shields.io/badge/dotnet-9.0-blue)](#tech-stack)
[![Status](https://img.shields.io/badge/status-active-green)](#)

## 📌 Overview

API1 is a small, focused ASP.NET Core Web API that provides user authentication and authorization features.  
It supports email/password registration with confirmation, login using JWT access tokens + refresh tokens, Google Sign-In, and standard account flows (forgot/reset password, logout). Email delivery is handled via MailKit and user management uses ASP.NET Identity backed by Entity Framework Core and SQL Server.

## 🚀 Features

- User registration with email confirmation
- Email/password login with JWT access token + refresh token
- Refresh token rotation endpoint
- Google Sign-In (validate Google ID token on backend)
- Logout (revoke/clear refresh tokens)
- Forgot Password & Reset Password flows
- Resend email confirmation
- Email sending via MailKit
- ASP.NET Identity + EF Core integration

## 🧱 Tech stack

- ASP.NET Core 9.0
- Entity Framework Core
- ASP.NET Core Identity
- JWT Authentication
- Google Identity token verification
- MailKit (SMTP)
- SQL Server (primary DB)

## 🔧 Quickstart (local)

Prerequisites:
- .NET 9 SDK
- SQL Server (local or Docker)
- Optional: dotnet-ef (tools) for migrations

1. Clone
```bash
git clone https://github.com/Helmy-Ekramy/Api1-for-Authentication.git
cd Api1-for-Authentication
```

2. Configure environment / appsettings (see sample below)

3. Apply database migrations
```bash
dotnet ef database update
```

4. Run
```bash
dotnet run
```

The API will be available at the configured URL (e.g., https://localhost:5001).

## ⚙️ Configuration (appsettings.json)

Add the following keys (example values shown). You can also use environment variables.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Api1Db;Trusted_Connection=True;"
  },
  "JwtSettings": {
    "Key": "YourSuperSecretKeyHere_change_me",
    "Issuer": "Api1",
    "Audience": "Api1Clients",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "GoogleAuth": {
    "ClientId": "your-google-client-id.apps.googleusercontent.com"
  },
  "Smtp": {
    "Host": "smtp.example.com",
    "Port": 587,
    "User": "no-reply@example.com",
    "Pass": "smtp-password",
    "From": "no-reply@example.com",
    "FromName": "API1 Support"
  }
}
```

Security notes:
- Keep the JWT signing key and SMTP credentials secret (use secrets manager or environment variables in production).
- Use HTTPS in all environments.

## 🔐 Authentication flow

1. User registers (email & password) → API sends confirmation email with token link.
2. User confirms email → account activated.
3. User logs in → API returns:
   - Access Token (JWT, short-lived)
   - Refresh Token (long-lived)
4. When access token expires, the client calls POST /api/account/refresh with the refresh token to receive a new pair.
5. On logout, refresh tokens are revoked server-side.

Refresh tokens are stored per-user and can be rotated / revoked to protect sessions.

## 🧩 Google Sign-In setup

To enable Google Sign-In:
1. Go to Google Cloud Console → APIs & Services → Credentials.
2. Create an OAuth 2.0 Client ID (type: Web application or Mobile).
3. Copy the Client ID and set it in appsettings: GoogleAuth:ClientId.
4. The server validates the Google ID token (sent by the client) to authenticate or register the user.

## ⚙️ Endpoints

Authentication
- POST /api/account/register — Register a new user
- POST /api/account/login — Login with email & password
- POST /api/account/refresh — Refresh JWT using refresh token
- POST /api/account/logout — Revoke/clear refresh tokens
- POST /api/account/resend-email — Resend email confirmation
- POST /api/account/confirm-email — Confirm user email
- POST /api/account/forgot-password — Send password reset email
- POST /api/account/reset-password — Reset user password

Google Sign-In
- POST /api/account/google-signin — Login or register using Google ID token

### Example: Login response
```json
{
  "isSuccess": true,
  "jwtToken": "<access_token>",
  "refreshToken": "<refresh_token>",
  "expiration": "2025-10-20T12:00:00Z",
  "message": "Login Success"
}
```

### Example: Refresh response
```json
{
  "isSuccess": true,
  "jwtToken": "<new_access_token>",
  "refreshToken": "<new_refresh_token>",
  "message": "Token refreshed"
}
```

## 📁 Project structure

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

## 🧪 Testing

- For manual testing use tools like Postman or curl.
- Protect sensitive flows in tests (e.g., do not embed real SMTP credentials).

## 🤝 Contributing

Contributions, issues and feature requests are welcome. For major changes, please open an issue first to discuss what you'd like to change.

Suggested workflow:
1. Fork the repo
2. Create a feature branch (feature/my-change)
3. Open a PR describing the change and link any related issues

## 🧾 License

This project is open source and available for learning or development purposes. (Consider adding a LICENSE file; MIT is a common choice.)

---

If you'd like, I can:
- Add badges (CI, coverage, NuGet) with real links,
- Insert screenshots or sample Postman collection,
- Push this README to a new branch and open a pull request with the change.

Tell me which of those you'd like me to do next.
