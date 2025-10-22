# Loan Applications Platform

This project is a small full-stack demo platform for managing loan applications.  
It includes a .NET 8 backend with PostgreSQL, a Vue 3 + TypeScript frontend, and full Docker setup for easy local or cloud deployment.  
Users can create loan applications, view offers, toggle their publication status, and filter or sort data.  
The project demonstrates clean container orchestration, EF Core database migrations, and a modular Vue architecture that mirrors real production practices.

---

# What this project contains

**Backend**
- .NET 8 Web API (Kestrel)
- Entity Framework Core (Postgres provider)
- DTOs, AutoMapper mapping profile, validation, middleware (error handling / request logging)
- Endpoints:
  - `GET /api/applications` — list applications (supports filters: `status`, `minAmount`, `maxAmount`, `minTerm`, `maxTerm`)
  - `POST /api/applications` — create application (validates amount, term, interest > 0)
  - `POST /api/applications/{number}/toggle` — toggle status between `Published` and `Unpublished`
- Database migrations

**Database**
- PostgreSQL (image: `postgres:15`) driven by Docker Compose
- Data persisted to a named volume `pgdata` (so restarting containers keeps data)

**Frontend**
- Vue 3 with Composition API + TypeScript
- Element Plus for UI primitives (optional parts)
- Pinia store `stores/applications.ts`
- Components:
  - `FiltersBar.vue`
  - `LoanTable.vue`
  - `ApplicationFormStyled.vue` / `CreateApplication.vue`
  - `SiteHeader.vue`


**Containerization**
- `docker-compose.yml` with services:
  - `postgres` (5432)
  - `backend` (container port 80 → host 5000)
  - `frontend` (nginx serving built Vue → container 80 → host 8080)
- `frontend/nginx.conf` proxies `/api` to the `backend` container on the internal container port (not `localhost:5000`)

---

# How to run (development)

Assuming Docker & Docker Compose are installed.

1. Build & start everything:
```bash
docker-compose up --build -d
```
2. If backend is down after first build up
```bash
docker-compose down
docker-compose up
```

