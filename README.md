# Personal Notes App

A full-stack personal notes application with .NET Core API and React frontend.

![React](https://img.shields.io/badge/React-19-61DAFB?logo=react)
![.NET](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet)

## 🌐 Live Demo

| Version | URL |
|---------|-----|
| **React** | [https://longsmoke1001.github.io/DotNetRepo/](https://longsmoke1001.github.io/DotNetRepo/) |
| **API (Swagger)** | [https://personal-notes-api-eeh2exdphmcccaat.germanywestcentral-01.azurewebsites.net/swagger](https://personal-notes-api-eeh2exdphmcccaat.germanywestcentral-01.azurewebsites.net/swagger) |

**Login**: `admin` / `password`

## ✨ Features

- 🔐 JWT Authentication (Login / Logout)
- 📝 Notes CRUD (Create / Read / Update / Delete)
- 📄 Pagination
- 🏷️ Categories (General / Diary / Password)
- 📱 Responsive Design (Tailwind CSS)

## 🛠️ Tech Stack

### Backend
- C# / .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- JWT Authentication
- Swagger

### Frontend
- React 19
- React Router
- Tailwind CSS
- Axios

### Deployment
- **.NET API**: Azure App Service
- **React**: GitHub Pages (`gh-pages`)

## 📁 Project Structure

```
DotNetRepo/
  ├── PersonalNotesAPI/       ← .NET Core API
  │     ├── Controllers/
  │     ├── Services/
  │     ├── Models/
  │     └── Program.cs
  ├── notes-frontend/         ← React App
  │     ├── src/
  │     └── package.json
  └── .github/workflows/
        └── deploy.yml        ← Deploy .NET API
```

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js 20+
- Git

### 1. Clone the repository

```bash
git clone https://github.com/longsmoke1001/DotNetRepo.git
cd DotNetRepo
```

### 2. Start the backend

```bash
cd PersonalNotesAPI
dotnet run
```

API runs at `http://localhost:8080`.

### 3. Start the frontend

```bash
cd notes-frontend
npm install
npm start
```

React app runs at `http://localhost:3000`.

## 📡 API Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| `POST` | `/api/Auth/login` | Login | ❌ |
| `GET` | `/api/Notes` | Get notes (paginated) | ✅ |
| `POST` | `/api/Notes` | Create note | ✅ |
| `PUT` | `/api/Notes/{id}` | Update note | ✅ |
| `DELETE` | `/api/Notes/{id}` | Delete note | ✅ |

## 🔑 Environment Variables

### React (`notes-frontend/.env`)

```
REACT_APP_API_URL=https://personal-notes-api-eeh2exdphmcccaat.germanywestcentral-01.azurewebsites.net
```

## 🚢 Deployment

### .NET API

```bash
git push
# GitHub Actions auto-deploys to Azure
```

### React

```bash
cd notes-frontend
npm run deploy
```

## 📝 License

MIT License

## 👤 Author

**Huang Long Yin, Leo**
- GitHub: [@longsmoke1001](https://github.com/longsmoke1001)
