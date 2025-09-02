# Patient Management System

## 📌 Overview
This is a **Patient Management System** built with **.NET 8 Core API**, **SQL Server**,   
It supports CRUD operations for patients, search with filters, reporting queries, JWT authentication, and unit tests.

---

## 🏗️ Project Structure

```
PatientManagement
├─ PatientManagement.sln
├─ src
│  └─ PatientManagement.Api
│     ├─ Program.cs
│     ├─ appsettings.json
│     ├─ Controllers/
│     ├─ Data/
│     ├─ Models/
│     ├─ DTOs/
│     ├─ Validators/
│     ├─ Repositories/
│     ├─ Services/
│     ├─ Middleware/
│     └─ Auth/
├─ tests
│  └─ PatientManagement.Tests
└─ db
   ├─ 01_schema.sql
   ├─ 02_indexes.sql
   ├─ 03_sample_data.sql (optional)
   ├─ 04_queries.sql
   └─ 05_sp_search_patients.sql
```

---

## ⚙️ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-in/sql-server/sql-server-downloads)
- (Optional) [Postman](https://www.postman.com/downloads/)

---

## 🚀 Getting Started

### 1️⃣ Database Setup
1. Create a database in SQL Server, e.g., `PatientDb`.
2. Run the scripts in `/db` in order:
   - `01_schema.sql`
   - `02_indexes.sql`
   - `03_sample_data.sql` (optional, for test data)
   - `04_queries.sql`
   - `05_sp_search_patients.sql`

### 2️⃣ Configure API
Update **connection string** and **JWT key** in `src/PatientManagement.Api/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=PatientDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
},
"Jwt": {
  "Key": "supersecretkey12345",
  "Issuer": "PatientManagement.Api",
  "Audience": "PatientManagement.Api",
  "ExpiresInMinutes": 60
}
```

### 3️⃣ Run the API
```bash
cd src/PatientManagement.Api
dotnet restore
dotnet build
dotnet run
```
API will run at: `https://localhost:5001`

Swagger UI available at: `https://localhost:5001/swagger`

---

## 🔑 Authentication

Use the **Auth endpoint** to get a JWT token.

```http
POST /api/auth/token
Content-Type: application/json

{
  "username": "admin",
  "password": "P@ssw0rd!"
}
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

Use this token in Authorization header:
```
Authorization: Bearer <token>
```

---

## 🧑‍⚕️ API Endpoints

### Patients
- `POST /api/patients` → Create patient  
- `GET /api/patients/{id}` → Get patient by Id  
- `GET /api/patients/search` → Search patients with filters  
- `PUT /api/patients/{id}` → Update patient  
- `DELETE /api/patients/{id}` → Delete patient  

### Auth
- `POST /api/auth/token` → Get JWT token  
---

## 📊 SQL Queries (Examples)

## -- Patients diagnosed in last 6 months
SELECT p.*
FROM dbo.Patients p
JOIN dbo.PatientConditions pc ON pc.PatientId = p.Id
WHERE pc.DiagnosedDate >= DATEADD(MONTH, -6, CAST(GETDATE() AS date));

## -- Top 3 cities with maximum patients
SELECT TOP (3) City, COUNT(*) AS PatientCount
FROM dbo.Patients
GROUP BY City
ORDER BY COUNT(*) DESC;

## -- Patients with more than 2 conditions
SELECT p.Id, p.FirstName, p.LastName, COUNT(*) AS ConditionCount
FROM dbo.Patients p
JOIN dbo.PatientConditions pc ON p.Id = pc.PatientId
GROUP BY p.Id, p.FirstName, p.LastName
HAVING COUNT(*) > 2;

## -- Average age grouped by condition
SELECT c.Id, c.Name,
       AVG(DATEDIFF(YEAR, p.DOB, CAST(GETDATE() AS date)) -
           CASE WHEN DATEADD(YEAR, DATEDIFF(YEAR, p.DOB, CAST(GETDATE() AS date)), p.DOB) > CAST(GETDATE() AS date) THEN 1 ELSE 0 END) AS AvgAge
FROM dbo.Conditions c
JOIN dbo.PatientConditions pc ON pc.ConditionId = c.Id
JOIN dbo.Patients p ON p.Id = pc.PatientId
GROUP BY c.Id, c.Name
ORDER BY c.Name;


---

## ✅ Evaluation Criteria

- Clean, modular .NET code (SOLID principles)  
- Error handling & validations  
- Unit test coverage  
- Optimized SQL schema & queries  
- Code readability + Git hygiene  

---

## 👨‍💻 Author

Patient Management System – BE Assignment Solution
