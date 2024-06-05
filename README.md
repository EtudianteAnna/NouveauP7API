## Description du Projet

Ce projet concerne le développement d'une application appelée PostTrades pour le groupe Trésorerie et marchés financiers de Findexium. L'objectif de cette application est de simplifier la communication et l'utilisation des informations post-transaction entre le front et le back-office.

Le projet principal de Findexium est un logiciel d'entreprise en ligne qui sert à générer davantage de transactions pour les entreprises institutionnelles à revenu fixe, qu'elles soient vendeuses ou acheteuses.

## Tâches à réaliser

Les tâches principales à réaliser dans le cadre de ce projet sont les suivantes :

1. Corriger et implémenter les fonctionnalités de l'application en utilisant ASP.NET Core, API Web, Entity Framework et API Web Security.
2. Implémenter l'authentification JWT.

## Normes et réglementations

Le groupe Findexium accorde une grande importance aux normes OWASP (Open Application Security Project) et RGPD (Règlement général sur la protection des données) afin d'assurer la sécurité de ses applications, de renforcer la confiance de ses clients et d'être conforme aux réglementations.

Les ressources suivantes doivent être consultées pour respecter ces normes :

- REST Security Cheat Sheet
- Guide RGPD pour le développement API
- Checklist de recommandations générales
- Normes OWASP
-
- Recommandations pour la conception des API (issues de la documentation officielle de Microsoft sur la conception des API RESTful et sur ASP.NET Core)
Ce fichier de projet définit les propriétés suivantes :
TargetFramework : La version cible de .NET Framework/Core pour le projet, ici net6.0.
Nullable : Active la prise en charge des types de référence nullable.
ImplicitUsings : Active les using implicites pour certains espaces de noms courants.

Packages NuGet
Authentification et Identité
Microsoft.AspNetCore.Authentication.JwtBearer : Permet l'authentification JWT dans l'application.
Microsoft.AspNetCore.Identity.EntityFrameworkCore : Fournit une implémentation d'Entity Framework Core pour la gestion des identités.
Microsoft.IdentityModel.Tokens et System.IdentityModel.Tokens.Jwt : Packages pour la gestion des jetons JWT.
Entity Framework Core
Microsoft.EntityFrameworkCore : Package principal pour utiliser Entity Framework Core.
Microsoft.EntityFrameworkCore.SqlServer : Fournit un fournisseur de base de données SQL Server pour Entity Framework Core.
Microsoft.EntityFrameworkCore.Tools : Outils pour Entity Framework Core, y compris les migrations de base de données.

Swagger
Swashbuckle.AspNetCore et Swashbuckle.AspNetCore.Filters : Packages pour générer une documentation Swagger pour l'API.

Tests
xunit : Framework de tests unitaires.
Prérequis
Logiciels nécessaires
Visual Studio (version X.X ou supérieure)
.NET 6.0 SDK ou ultérieur
SQL Server (ou une autre base de données compatible avec Entity Framework Core)
Connaissances requises

