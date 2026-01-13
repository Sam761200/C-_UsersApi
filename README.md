# Users API - ASP.NET Core REST API

[![.NET](https://img.shields.io/badge/.NET-9.0-blue)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-purple)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![SQLite](https://img.shields.io/badge/SQLite-3.0-green)](https://www.sqlite.org/)
[![Swagger](https://img.shields.io/badge/Swagger-UI-orange)](https://swagger.io/)
[![Entity Framework](https://img.shields.io/badge/Entity_Framework_Core-9.0-blue)](https://docs.microsoft.com/en-us/ef/)

Une API REST complète de gestion d'utilisateurs construite avec ASP.NET Core, Entity Framework Core et SQLite. Implémente une architecture Clean Architecture avec séparation des responsabilités et DTOs.

## 📋 Table des matières

- [Fonctionnalités](#-fonctionnalités)
- [Architecture](#-architecture)
- [Technologies](#-technologies)
- [Installation](#-installation)
- [Configuration](#-configuration)
- [Utilisation](#-utilisation)
- [API Endpoints](#-api-endpoints)
- [Tests](#-tests)
- [Structure du projet](#-structure-du-projet)
- [Déploiement](#-déploiement)
- [Contribuer](#-contribuer)
- [Licence](#-licence)

## ✨ Fonctionnalités

- ✅ **CRUD complet** : Création, lecture, mise à jour, suppression d'utilisateurs
- ✅ **Validation automatique** : DataAnnotations côté client et serveur
- ✅ **DTOs** : Séparation parfaite entre API et domaine
- ✅ **Mises à jour partielles** : PATCH-like avec PUT sélectif
- ✅ **Documentation Swagger** : Interface interactive auto-générée
- ✅ **Base de données SQLite** : Facile à utiliser, pas de configuration complexe
- ✅ **Migrations EF Core** : Versionning automatique du schéma DB
- ✅ **Gestion d'erreurs** : Codes HTTP appropriés et messages explicites
- ✅ **Architecture Clean** : Séparation Controller/Service/Repository
- ✅ **Dependency Injection** : Injection automatique des services

## 🏗️ Architecture

Ce projet suit les principes de **Clean Architecture** :

```
🌐 HTTP Request
     ↓
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  CONTROLLER     │───▶│   SERVICE       │───▶│  REPOSITORY     │───▶│   DATABASE      │
│                 │    │                 │    │                 │    │                 │
│ • UsersController│    │ • UserService   │    │ • UserRepository│    │ • users.db      │
│ • DTOs mapping   │    │ • Business Logic│    │ • EF Core       │    │ • SQLite        │
│ • HTTP responses │    │ • Validations   │    │ • Queries       │    │ • Tables        │
│ • Error handling │    │ • Rules métier  │    │ • SaveChanges   │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘    └─────────────────┘
         ↑                      ↑                      ↑
   UserDto/CreateDto     User (entity)         DbSet<User>       SQL
   UpdateDto
```

### Couches

- **Controllers** : Points d'entrée HTTP, mapping DTOs ↔ entités
- **Services** : Logique métier, validations, règles business
- **Repositories** : Accès aux données, abstraction EF Core
- **Models** : Entités de domaine
- **DTOs** : Objets de transfert API (séparation API/domaine)

## 🛠️ Technologies

- **Framework** : [.NET 9.0](https://dotnet.microsoft.com/)
- **Language** : [C# 12](https://docs.microsoft.com/en-us/dotnet/csharp/)
- **Web Framework** : [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
- **ORM** : [Entity Framework Core 9](https://docs.microsoft.com/en-us/ef/core/)
- **Database** : [SQLite](https://www.sqlite.org/)
- **API Documentation** : [Swashbuckle/Swagger](https://swagger.io/)
- **Dependency Injection** : Framework natif ASP.NET Core
- **Validation** : [DataAnnotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations)

## 🚀 Installation

### Prérequis

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Git](https://git-scm.com/) (optionnel, pour cloner)

### Installation rapide

```bash
# Clone le repository
git clone https://github.com/your-username/UsersApi.git
cd UsersApi

# Restaure les packages NuGet
dotnet restore

# Applique les migrations de base de données
dotnet ef database update

# Lance l'application
dotnet run
```

L'API sera disponible sur :
- **HTTP** : http://localhost:5011
- **Swagger UI** : http://localhost:5011/swagger

## ⚙️ Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "UsersDatabase": "Data Source=users.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Variables d'environnement

Pour la production, configurez :

```bash
# Connection string de base de données
ConnectionStrings__UsersDatabase="Data Source=/app/data/users.db"

# Environnement
ASPNETCORE_ENVIRONMENT=Production

# Port (optionnel)
ASPNETCORE_URLS=http://+:8080
```

## 📖 Utilisation

### Démarrage rapide

```bash
# Depuis le répertoire du projet
dotnet run

# Ou en mode watch (recompilation automatique)
dotnet watch run
```

### Interface Swagger

1. Ouvrez http://localhost:5011/swagger
2. Explorez et testez tous les endpoints interactivement
3. Les DTOs sont automatiquement documentés

## 🔌 API Endpoints

### Base URL
```
http://localhost:5011/api
```

### Endpoints disponibles

#### 👥 Utilisateurs

| Méthode | Endpoint | Description | Corps de requête | Réponse |
|---------|----------|-------------|------------------|---------|
| `GET` | `/users` | Liste tous les utilisateurs | - | `UserDto[]` |
| `GET` | `/users/{id}` | Récupère un utilisateur | - | `UserDto` |
| `POST` | `/users` | Crée un utilisateur | `CreateUserDto` | `UserDto` (201) |
| `PUT` | `/users/{id}` | Met à jour un utilisateur | `UpdateUserDto` | `UserDto` (200) |
| `DELETE` | `/users/{id}` | Supprime un utilisateur | - | - (204) |

### Exemples de requêtes

#### Créer un utilisateur
```bash
curl -X POST "http://localhost:5011/api/users" \
     -H "Content-Type: application/json" \
     -d '{
       "name": "John Doe",
       "email": "john@example.com"
     }'
```

#### Récupérer tous les utilisateurs
```bash
curl -X GET "http://localhost:5011/api/users" \
     -H "Accept: application/json"
```

#### Mise à jour partielle
```bash
curl -X PUT "http://localhost:5011/api/users/1" \
     -H "Content-Type: application/json" \
     -d '{
       "email": "john.doe@example.com"
     }'
```

### DTOs

#### UserDto (réponse)
```json
{
  "id": 1,
  "name": "John Doe",
  "email": "john@example.com",
  "createdAt": "2026-01-13T15:30:45.123Z"
}
```

#### CreateUserDto (création)
```json
{
  "name": "John Doe",
  "email": "john@example.com"
}
```

#### UpdateUserDto (mise à jour)
```json
{
  "name": "Johnny Doe",     // Optionnel
  "email": "johnny@example.com"  // Optionnel
}
```

### Codes de statut HTTP

- `200` : Succès (GET, PUT)
- `201` : Créé (POST)
- `204` : Pas de contenu (DELETE)
- `400` : Requête invalide (validation DTO)
- `404` : Ressource non trouvée
- `409` : Conflit (email dupliqué)
- `500` : Erreur serveur

## 🧪 Tests

### Tests API (manuel)

Utilisez le fichier `test-api.http` avec l'extension REST Client de VS Code :

```bash
# Ouvre test-api.http dans VS Code
# Clique "Send Request" sur chaque requête
```

### Tests unitaires (futur)

```bash
# Structure prévue pour les tests
dotnet test
```

### Tests d'intégration

```bash
# Tests end-to-end avec base de données de test
dotnet test --filter "Integration"
```

## 📁 Structure du projet

```
UsersApi/
├── Controllers/
│   └── UsersController.cs      # Endpoints REST API
├── Services/
│   ├── IUserService.cs         # Interface service métier
│   └── UserService.cs          # Implémentation logique métier
├── Repositories/
│   ├── IUserRepository.cs      # Interface accès données
│   └── UserRepository.cs       # Implémentation EF Core
├── Models/
│   └── User.cs                 # Entité domaine
├── DTOs/
│   ├── UserDto.cs              # DTO réponses
│   ├── CreateUserDto.cs        # DTO création
│   └── UpdateUserDto.cs        # DTO mise à jour
├── Data/
│   └── UsersDbContext.cs       # Contexte EF Core
├── Migrations/                 # Migrations EF Core
├── Properties/
│   └── launchSettings.json     # Configuration lancement
├── appsettings.json            # Configuration application
├── Program.cs                  # Point d'entrée + DI
├── UsersApi.csproj             # Manifest projet
├── users.db                    # Base SQLite (auto-généré)
├── test-api.http               # Tests API
├── .gitignore                  # Fichiers ignorés Git
└── README.md                   # Documentation
```

## 🚢 Déploiement

### Docker (recommandé)

```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["UsersApi.csproj", "."]
RUN dotnet restore "UsersApi.csproj"
COPY . .
RUN dotnet build "UsersApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UsersApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UsersApi.dll"]
```

```bash
# Build et run
docker build -t users-api .
docker run -p 8080:80 users-api
```

### Azure App Service

1. Créez une Web App Azure
2. Configurez la connection string dans les paramètres d'application
3. Déployez via Git ou ZIP

### Railway / Render

1. Connectez votre repo GitHub
2. Configurez les variables d'environnement
3. Déployez automatiquement

## 🤝 Contribuer

1. Fork le projet
2. Créez une branche (`git checkout -b feature/AmazingFeature`)
3. Committez vos changements (`git commit -m 'Add some AmazingFeature'`)
4. Push vers la branche (`git push origin feature/AmazingFeature`)
5. Ouvrez une Pull Request

### Standards de code

- Utilisez les conventions C# de Microsoft
- Documentez avec des commentaires XML (`///`)
- Écrivez des tests pour les nouvelles fonctionnalités
- Respectez l'architecture Clean Architecture

## 📄 Licence

Ce projet est sous licence MIT - voir le fichier [LICENSE](LICENSE) pour plus de détails.

## 🙏 Remerciements

- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) pour le framework web
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) pour l'ORM
- [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle) pour Swagger
- [SQLite](https://www.sqlite.org/) pour la base de données

---

**Développé avec ❤️ en ASP.NET Core**
