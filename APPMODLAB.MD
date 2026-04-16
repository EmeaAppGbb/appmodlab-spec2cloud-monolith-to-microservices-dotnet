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

---

## CLI Walkthrough — Spec2Cloud with GitHub Copilot CLI

**Executed by:** Ash (via GitHub Copilot CLI)
**Branch:** `solution-final`
**Tool:** `gh copilot` with `--allow-all-tools --yolo`

### Prerequisites

- Git, .NET SDK (10.0+), GitHub Copilot CLI extension
- Clone this repo and checkout `main`

### Setup

```powershell
cd appmodlab-spec2cloud-monolith-to-microservices-dotnet
git checkout main && git checkout -b solution-final
New-Item -ItemType Directory -Force -Path assets/outputs
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")
```

### Step 1: Explore Monolith (`step-01-explore-monolith`)

Analyzed the UrbanBites codebase structure. Ran `dotnet build` — both projects compile with NETSDK1138 warning (netcoreapp3.1 out of support). Identified 10 entities, 6 services, 6 controllers, 13 API endpoints, SignalR hub, Hangfire jobs, and 8 architectural anti-patterns.

→ [View output](assets/outputs/step-01.txt)

### Step 2: Spec2Cloud Analysis (`step-02-spec2cloud-analysis`)

Identified 6 microservice decomposition candidates ranked by extraction priority. Mapped the current dependency graph, problematic couplings, and recommended extraction order using the Strangler Fig pattern: Notification → Restaurant → Customer → Payment → Delivery → Order.

→ [View output](assets/outputs/step-02.txt)

### Step 3: Generate Domain Specs (`step-03-domain-specs`)

Defined 6 bounded contexts with aggregate roots, domain events, business rules, and data store assignments. Created context map showing relationships: conformist (Restaurant↔Order), customer-supplier (Order↔Payment, Order↔Delivery), and event-driven (→Notification).

→ [View output](assets/outputs/step-03.txt)

### Step 4: Generate API Contracts (`step-04-api-contracts`)

Created OpenAPI 3.0 specifications for all 6 microservices with full endpoint definitions, request/response schemas, and error codes. Specs stored in `assets/specs/api/`.

→ [View output](assets/outputs/step-04.txt) | [View API specs](assets/specs/api/)

### Step 5: Design Service Architecture (`step-05-service-architecture`)

Designed the inter-service communication architecture: Azure API Management gateway, Azure Service Bus event bus with topic/subscription model, synchronous HTTP calls with Polly circuit breakers, Azure Container Apps deployment, and distributed tracing with Application Insights.

→ [View output](assets/outputs/step-05.txt)

### Step 6: Scaffold Microservices (`step-06-scaffold-services`)

Defined the target project structure using clean architecture (Api/Domain/Infrastructure layers per service). Technology stack: .NET 8, EF Core 8, Azure Service Bus SDK, Refit+Polly for HTTP, Docker multi-stage builds, Azure Bicep for IaC.

→ [View output](assets/outputs/step-06.txt)

### Step 7: Migration Strategy (`step-07-migration-strategy`)

Created a 22-week Strangler Fig migration plan across 7 phases: Infrastructure setup → Notification (stateless, lowest risk) → Restaurant (read-heavy) → Customer (simple CRUD) → Payment (PCI compliance) → Delivery (real-time) → Order (orchestrator, last) → Decommission. Includes risk mitigation with Saga pattern, feature flags, and automated rollback triggers.

→ [View output](assets/outputs/step-07.txt)

### Step 8: Validate Decomposition (`step-08-validate`)

Verified 100% coverage across all dimensions: 10/10 entities, 13/13 API endpoints, 6/6 services, 2/2 background jobs, 1/1 real-time hub, 2/2 external integrations, and 8/8 anti-patterns resolved. All monolith functionality is fully mapped to microservices with no gaps.

→ [View output](assets/outputs/step-08.txt)

### Tags

```
step-01-explore-monolith
step-02-spec2cloud-analysis
step-03-domain-specs
step-04-api-contracts
step-05-service-architecture
step-06-scaffold-services
step-07-migration-strategy
step-08-validate
```
