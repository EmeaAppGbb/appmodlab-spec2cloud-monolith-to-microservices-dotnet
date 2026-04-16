---
title: "Monolith to Microservices with Spec2Cloud"
description: "Decompose .NET monolith to microservices using spec-driven analysis with UrbanBites food delivery platform"
authors: ["marconsilva"]
category: "Spec-Driven Development"
industry: "Cross-Industry"
services: ["Azure Container Apps", "Azure SQL Database", "Azure Service Bus", "Azure API Management"]
languages: [".NET", "C#"]
frameworks: ["ASP.NET Core", "Entity Framework Core", "SignalR"]
modernizationTools: ["Spec2Cloud"]
agenticTools: []
tags: ["microservices", "monolith-decomposition", "bounded-contexts", "saga-pattern", "event-driven"]
extensions: ["github.copilot"]
thumbnail: ""
video: ""
version: "1.0.0"
---

## QA Review — Lambert (Tester/QA)

**Review Date:** 2025-07-18
**Reviewer:** Lambert (QA)
**Requested by:** madasi

### Build & Run Status

| Check | Result | Notes |
|-------|--------|-------|
| NuGet Restore | ✅ Pass | All 8 packages restored (EF Core 3.1.32, Hangfire, Stripe, SendGrid, SignalR) |
| dotnet build | ✅ Pass | Both projects compile. 1 warning: NETSDK1138 (netcoreapp3.1 out of support) |
| dotnet run | ❌ Blocked | .NET Core 3.1 runtime not installed; only .NET 10.0.5 available |
| dotnet test | ❌ Blocked | Same runtime issue; testhost.exe requires AspNetCore.App 3.1.0 |

### Project Structure

The UrbanBites monolith solution contains the web application and test projects, with all domain entities, services, and controllers in a single deployable unit.

[View: Project Structure](assets/screenshots/01-project-structure.txt)

*Open the text file to view the full project tree with endpoints.*

### Build Results

Both projects compile successfully, though .NET Core 3.1 is out of support and the runtime is unavailable in the current environment.

[View: Build Results](assets/screenshots/02-build-results.txt)

*Open the text file to view the complete build output and runtime analysis.*

### Architecture Summary

The monolith is a well-structured **UrbanBites food delivery platform** targeting `netcoreapp3.1`:

- **10 entity classes** in a single `Entities.cs` (Restaurant, MenuItem, Order, OrderItem, Payment, Delivery, Driver, Customer, Review, Promotion)
- **1 God DbContext** (`UrbanBitesDbContext`) with 10 DbSets — all domain data in one database
- **6 tightly-coupled services**: OrderService, PaymentService, DeliveryService, NotificationService, PricingService, ReviewService
- **6 API controllers** covering restaurant browsing, ordering, payments, delivery, customers, reviews
- **SignalR hub** for real-time delivery tracking
- **Hangfire** background jobs for order timeout and driver assignment
- **External integrations**: Stripe (payments), SendGrid (email)

### Anti-Patterns for Decomposition

The architecture analysis identifies 5 critical anti-patterns that drive the decomposition strategy. See the full analysis for bounded context mapping.

[View: Architecture Analysis](assets/screenshots/03-architecture-analysis.txt)

*Open the text file to view the anti-patterns and bounded context mapping.*

1. God DbContext (10 DbSets, one shared database)
2. Cross-domain synchronous calls in OrderService (touches Customer, Restaurant, MenuItem, Pricing, Notification)
3. Blocking `.Result` calls on async methods in Hangfire jobs
4. All entities in one file with no bounded context separation
5. Shared connection strings for app + Hangfire

### Spec2Cloud Target

Decompose into 6 independent microservices (Restaurant, Order, Payment, Delivery, Customer, Notification) using the Strangler Fig pattern across 6 step-branches.

[View: Spec2Cloud Configuration](assets/screenshots/04-spec2cloud-config.txt)

*Open the text file to view the decomposition plan and branch workflow.*

### Screenshots

See `assets/screenshots/` for structural captures:
- `01-project-structure.txt` — Full project tree with endpoints
- `02-build-results.txt` — Build output and runtime analysis
- `03-architecture-analysis.txt` — Anti-patterns and bounded contexts
- `04-spec2cloud-config.txt` — Decomposition plan and branch workflow
