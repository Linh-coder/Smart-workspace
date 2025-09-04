# SmartWorkspace 🚀

Một ứng dụng web API được xây dựng với .NET 9, tuân theo kiến trúc Clean Architecture, bao gồm quản lý người dùng, workspace, và hệ thống phân quyền dựa trên role-based authentication.

## 📋 Yêu cầu hệ thống

- **.NET 9 SDK** - [Tải về](https://dotnet.microsoft.com/download/dotnet/9.0)
- **PostgreSQL 15+** - [Tải về](https://www.postgresql.org/download/)
- **Docker & Docker Compose** (tùy chọn) - [Tải về](https://www.docker.com/)
- **Visual Studio 2022** hoặc **VS Code** (khuyến nghị)

## 🏗️ Kiến trúc dự án

```
SmartWorkspace/
├── src/
│   ├── SmartWorkspace.API/           # Web API - Controllers & Endpoints
│   ├── SmartWorkspace.Application/   # Business Logic - CQRS với MediatR
│   ├── SmartWorkspace.Domain/        # Domain Models & Interfaces
│   ├── SmartWorkspace.Infrastructure/# External Services (JWT, Email...)
│   └── SmartWorkspace.Persistence/   # Data Access - Entity Framework Core
├── tests/
│   └── SmartWorkspace.Tests/         # Unit & Integration Tests
├── docker-compose.yml               # Docker configuration
└── SmartWorkspace.sln              # Solution file
```

## 🛠️ Công nghệ sử dụng

### Core Framework
- **.NET 9** - Framework chính
- **ASP.NET Core** - Web API framework
- **Entity Framework Core 9** - ORM
- **PostgreSQL** - Cơ sở dữ liệu chính

### Authentication & Security
- **JWT Bearer Authentication** - Xác thực token
- **ASP.NET Core Identity** - Quản lý mật khẩu
- **Refresh Token** - Làm mới token tự động

### Architecture & Patterns
- **Clean Architecture** - Kiến trúc sạch
- **CQRS với MediatR** - Command Query Responsibility Segregation
- **Repository Pattern** - Truy xuất dữ liệu
- **Unit of Work Pattern** - Quản lý transaction

### Libraries
- **AutoMapper** - Object mapping
- **FluentValidation** - Validation
- **Serilog** - Logging
- **Swagger/OpenAPI** - API Documentation

## 🚀 Cách chạy dự án

### Option 1: Sử dụng Docker (Khuyến nghị) 🐳

1. **Clone repository**
   ```bash
   git clone https://github.com/Linh-coder/Smart-workspace.git
   cd SmartWorkspace
   ```

2. **Chạy với Docker Compose**
   ```bash
   docker-compose up -d
   ```

   Lệnh này sẽ khởi động:
   - PostgreSQL database (port 5432)
   - Redis cache (port 6379)

3. **Cấu hình connection string**
   
   Cập nhật `src/SmartWorkspace.API/appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=smartworkspace;Username=postgres;Password=postgres"
     },
     "Jwt": {
       "Key": "your-super-secret-jwt-key-here-must-be-at-least-256-bits",
       "Issuer": "SmartWorkspace",
       "Audience": "SmartWorkspace.Client",
       "AccessTokenMinutes": 30,
       "RefreshTokenDays": 7
     }
   }
   ```

4. **Chạy ứng dụng**
   ```bash
   cd src/SmartWorkspace.API
   dotnet run
   ```

### Option 2: Cài đặt thủ công 🔧

1. **Clone repository**
   ```bash
   git clone https://github.com/Linh-coder/Smart-workspace.git
   cd SmartWorkspace
   ```

2. **Cài đặt PostgreSQL**
   - Tạo database mới tên `SmartWorkspaceDb`
   - Tạo user với quyền truy cập

3. **Cấu hình appsettings**
   
   Cập nhật `src/SmartWorkspace.API/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=SmartWorkspaceDb;Username=your_username;Password=your_password"
     },
     "Jwt": {
       "Key": "your-super-secret-jwt-key-here-must-be-at-least-256-bits",
       "Issuer": "SmartWorkspace",
       "Audience": "SmartWorkspace.Client",
       "AccessTokenMinutes": 30,
       "RefreshTokenDays": 7
     }
   }
   ```

4. **Restore packages**
   ```bash
   dotnet restore
   ```

5. **Tạo và chạy migrations**
   ```bash
   cd src/SmartWorkspace.API
   dotnet ef database update --project ../SmartWorkspace.Persistence
   ```

6. **Build solution**
   ```bash
   dotnet build
   ```

7. **Chạy ứng dụng**
   ```bash
   dotnet run
   ```

## 🌐 Truy cập ứng dụng

- **API Base URL**: `https://localhost:7229` hoặc `http://localhost:5232`
- **Swagger UI**: `https://localhost:7229/swagger`
- **API Documentation**: Tự động tạo với OpenAPI 3.0

## 🔐 Authentication Endpoints

### Đăng ký người dùng
```http
POST /auth/register
Content-Type: application/json

{
  "userName": "johndoe",
  "email": "john@example.com",
  "password": "Password123!",
  "confirmPassword": "Password123!"
}
```

### Đăng nhập
```http
POST /auth/login
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "Password123!"
}
```

### Refresh Token
```http
POST /auth/refresh-token
Content-Type: application/json

{
  "refreshToken": "your-refresh-token-here"
}
```

### Revoke Token
```http
POST /auth/revoke-token
Content-Type: application/json

{
  "token": "your-refresh-token-here"
}
```

## 📊 Database Schema

Ứng dụng bao gồm các bảng chính sau:

- **Users** - Thông tin người dùng
- **RefreshTokens** - Quản lý refresh token
- **Workspaces** - Quản lý workspace
- **Roles** - Định nghĩa các role
- **Permissions** - Các quyền trong hệ thống
- **RolePermissions** - Mapping role-permission
- **UserWorkspaceRoles** - Role của user trong workspace

### Tự động Seed Data

Khi chạy trong môi trường Development, hệ thống sẽ tự động tạo:
- Dữ liệu mẫu cho Users
- Workspace mặc định
- Roles và Permissions cơ bản
- Mapping role-permission

## 🧪 Chạy Tests

```bash
# Chạy tất cả tests
dotnet test

# Chạy tests với coverage
dotnet test --collect:"XPlat Code Coverage"

# Chạy specific test project
dotnet test tests/SmartWorkspace.Tests/
```

## 🛠️ Các lệnh phát triển

### Entity Framework Commands

```bash
# Tạo migration mới
dotnet ef migrations add MigrationName --project src/SmartWorkspace.Persistence --startup-project src/SmartWorkspace.API

# Cập nhật database
dotnet ef database update --project src/SmartWorkspace.Persistence --startup-project src/SmartWorkspace.API

# Xem danh sách migrations
dotnet ef migrations list --project src/SmartWorkspace.Persistence --startup-project src/SmartWorkspace.API

# Rollback migration
dotnet ef database update PreviousMigrationName --project src/SmartWorkspace.Persistence --startup-project src/SmartWorkspace.API

# Xóa database (cẩn thận!)
dotnet ef database drop --project src/SmartWorkspace.Persistence --startup-project src/SmartWorkspace.API
```

### Build Commands

```bash
# Build solution
dotnet build

# Build specific project
dotnet build src/SmartWorkspace.API

# Clean solution
dotnet clean

# Restore packages
dotnet restore
```

## 📝 Cấu trúc API Response

### Success Response
```json
{
  "isSuccess": true,
  "data": {
    "accessToken": "jwt-token-here",
    "refreshToken": "refresh-token-here",
    "expiresAt": "2025-09-04T10:30:00Z",
    "user": {
      "id": "uuid-here",
      "userName": "johndoe",
      "email": "john@example.com"
    }
  },
  "errorMessage": null
}
```

### Error Response
```json
{
  "isSuccess": false,
  "data": null,
  "errorMessage": "Invalid credentials"
}
```

## 🔧 Configuration

### JWT Settings
- **AccessTokenMinutes**: 30 (thời gian sống của access token)
- **RefreshTokenDays**: 7 (thời gian sống của refresh token)
- **MaxActiveTokenPerUser**: 5 (số token tối đa mỗi user)

### Database Settings
- **Connection Timeout**: 30 giây
- **Command Timeout**: 30 giây
- **Auto Migration**: Bật trong Development

## 🐛 Troubleshooting

### Lỗi thường gặp

1. **Connection to database failed**
   - Kiểm tra PostgreSQL service đã chạy chưa
   - Kiểm tra connection string trong appsettings.json
   - Kiểm tra firewall/port 5432

2. **JWT token invalid**
   - Kiểm tra JWT Key trong appsettings.json (phải ít nhất 256 bits)
   - Kiểm tra thời gian hệ thống có chính xác không

3. **Migration errors**
   - Xóa folder `Migrations` và tạo lại migration đầu tiên
   - Kiểm tra database có tồn tại không

4. **Build errors**
   - Chạy `dotnet clean` rồi `dotnet restore`
   - Kiểm tra .NET 9 SDK đã được cài đặt

### Logs

Ứng dụng sử dụng Serilog để ghi log. Logs sẽ được ghi vào:
- Console (Development)
- File logs (Production)

## 🤝 Contributing

1. Fork the project
2. Create feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License.

## 👥 Authors

- **Linh-coder** - *Initial work* - [GitHub](https://github.com/Linh-coder)

## 🙏 Acknowledgments

- Clean Architecture template
- .NET Community
- Entity Framework Core team
