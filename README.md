# Firebase Authentication Integration with .NET(8)

## Overview

This project demonstrates how to integrate Firebase Authentication with a .NET backend using JWT token validation, allowing users to securely interact with a .NET API. It uses Firebase Admin SDK for validating Firebase ID tokens, ensuring a seamless authentication process for your application.

## Features

- **Firebase Authentication**: User authentication using Firebase's identity system.
- **JWT Token Validation**: Validating Firebase-generated JWT tokens on the backend.
- **PostgreSQL Integration**: Configured with PostgreSQL for storing application data.
- **Swagger UI**: API documentation using Swagger.

## Setup

### 1. Clone the repository
```bash
git clone https://github.com/aok-ponlork/firebase-core.git
cd firebase-core
```

### 2. Install dependencies
Make sure you have the required dependencies installed:

```bash
dotnet restore
```

### 3. Firebase Setup

- Go to [Firebase Console](https://console.firebase.google.com/).
- Create a Firebase project or use an existing one.
- Download the **Firebase service account JSON file** and store it in a secure location.
- Add the path to the file in your `appsettings.json` under `"Firebase:CredentialsPath"`.

### 4. Update `appsettings.json`

In the `appsettings.json` file, update the following configurations with your Firebase project details:

```json
"Firebase": {
    "CredentialsPath": "path/to/your/firebase-service-account.json",
    "ProjectId": "your-project-id",
    "signInWithPassword": "your-firebase-api-key",
    "RefreshTokenUri": "your-refresh-token-uri",
    "ValidIssuer": "https://securetoken.google.com/your-project-id",
    "JwksUrl": "https://www.googleapis.com/service_accounts/v1/jwk/securetoken@system.gserviceaccount.com"
}
```

### 5. Configure Database & Run Migrations
Ensure you have a PostgreSQL database setup and provide the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
    "DefaultConnection": "Host=your_host;Database=your_db;Username=your_user;Password=your_password"
}
```

Once the connection string is configured, you'll need to run the Entity Framework migrations to create the necessary tables (e.g., the user table). Run the following command:
```bash
dotnet ef database update
```

### 6. Run the Application

To run the application locally:

```bash
dotnet watch run
```

Once the app is running, you can access the Swagger UI at http://localhost:5244/swagger (port may vary).

### 7. Test Authentication

Use the Firebase Authentication system to sign in users, get their ID tokens, and pass those tokens as a Bearer token in the Authorization header when making requests to the protected API endpoints.

---

## Endpoints

### `GET /auth/test`

A simple endpoint to test if authentication is working:

- **Auth Required**: Yes (JWT Token)
- **Response**: `{ "message": "Success" }` (with a 1-second delay)

---


### Notes:

- The **CredentialsPath** should not be committed to version control (GitHub, GitLab, etc.).
- Make sure to follow the **.gitignore** approach to exclude any sensitive files such as `firebase-service-account.json`.
