# Clarity CRM

Enterprise Customer Relationship Management system built with .NET Core and Entity Framework.

*Originally developed 2017-2018, migrated from internal TFS repository.*

## Features

- **Customer Management** - Full CRUD operations for customer data
- **Sales Pipeline** - Track leads through stages to closed deals
- **Activity Tracking** - Log calls, emails, meetings, and tasks
- **Reporting Dashboard** - Real-time analytics and KPIs
- **User Authentication** - Role-based access control with JWT
- **Email Integration** - Automated email campaigns and templates
- **REST API** - Full API for mobile and third-party integrations

## Technology Stack

- .NET Core 2.1 / 3.0
- Entity Framework Core
- SQL Server
- Redis Cache
- SignalR for real-time updates
- Angular 6 frontend

## Getting Started

```bash
# Restore packages
dotnet restore

# Run migrations
dotnet ef database update

# Run the application
dotnet run --project src/Clarity.Web
```

## Project Structure

```
src/
├── Clarity.Core/           # Domain models and interfaces
├── Clarity.Data/           # Entity Framework, repositories
├── Clarity.Services/       # Business logic services
├── Clarity.Web/            # ASP.NET Core Web API
├── Clarity.Auth/           # Authentication service
└── Clarity.Email/          # Email service

tests/
├── Clarity.Core.Tests/
├── Clarity.Services.Tests/
└── Clarity.Integration.Tests/
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/customers | List customers |
| POST | /api/customers | Create customer |
| GET | /api/leads | List leads |
| POST | /api/leads | Create lead |
| GET | /api/activities | List activities |
| GET | /api/reports/dashboard | Dashboard data |

## License

Proprietary - Internal Use Only
