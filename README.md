# 📝 TodoList Clean Architecture

A simple TodoList API using .NET 8, EF Core, and SQL Server in Docker.

---

## 🚀 Quick Start

1. **Create a `.env` file** in the project root with your database password:

```env
SA_PASSWORD=YourStrong!Passw0rd
```
Run the application using Docker Compose:
```
docker compose up --build
```
Access Swagger UI in your browser to test the API:
```
http://localhost:5000/swagger/index.html
```
🛠️ Notes
The API container connects to the todolist-db container automatically.
Default ports:
API: http://localhost:5000
SQL Server: 1433 (inside container)
