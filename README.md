# Employee Management System - TalentoPlus S.A.S.

## 🌟 Estado del Proyecto y Madurez

Esta solución backend representa un sistema integral de gestión de recursos humanos y catálogos desarrollado con **ASP.NET Core 9.0** bajo el paradigma de **Clean Architecture**. El proyecto está configurado para un **despliegue en producción real** (production-ready) gracias a su completa containerización con Docker Compose y su estrategia de pruebas unitarias y de integración.

### Características Clave
* **Gestión Centralizada:** CRUD completo de empleados y catálogos transaccionales (Departamentos, Cargos).
* **Importación Masiva:** Funcionalidad crítica de importación de empleados mediante archivos Excel.
* **Arquitectura Sólida:** Separación estricta de responsabilidades (Dominio, Aplicación, Infraestructura).
* **Seguridad:** Autenticación basada en JWT y uso de contraseñas de aplicación para servicios SMTP.

| Autor Principal | Tecnología de Backend | Versión Final Dockerizada | Interfaz de Acceso |
| :--- | :--- | :--- | :--- |
| Mariana Quintero Cardona | ASP.NET Core | **.NET 9.0** | Swagger UI (Puerto 5162) |

***

## 🏗️ Arquitectura y Estructura del Código

El proyecto adhiere al principio de **Clean Architecture**, aislando la lógica de negocio central (Domain) de las preocupaciones externas (APIs, Bases de Datos, Servicios).

### Estructura de Capas
| Capa | Proyecto | Responsabilidad |
| :--- | :--- | :--- |
| **Presentation** | `TalentoPlus.Api` | **Punto de Entrada.** Configuración de Controllers, Endpoints, DI, y Swagger. |
| **Application** | `TalentoPlus.Application` | **Reglas de Negocio.** Implementa los casos de uso (Commands/Queries), DTOs y Servicios de Aplicación. |
| **Domain** | `TalentoPlus.Domain` | **Núcleo del Negocio.** Contiene Entidades, Value Objects, Enums y Contratos (Interfaces). |
| **Infrastructure** | `TalentoPlus.Infrastructure`| **Implementación Externa.** EF Core, Repositorios, Servicios de Identidad y Mantenimiento de Datos. |

***

## ⚙️ Guía de Despliegue Local (Docker Compose)

La solución está diseñada para levantarse sin fricción en cualquier entorno que soporte Docker, gracias al uso de `docker-compose.yml`, que orquesta la API y la base de datos MySQL.

### Prerrequisitos

1.  **Git:** Instalado en el sistema.
2.  **Docker Desktop:** Instalado y en ejecución (requerido para el orquestador).

### Paso 1: Clonar y Preparar el Entorno

```bash
# Clonar el repositorio
git clone [https://github.com/MarianaQC/Sistema-de-Gesti-n-de-Empleados-TalentoPlus-S.A.S..git](https://github.com/MarianaQC/Sistema-de-Gesti-n-de-Empleados-TalentoPlus-S.A.S..git)
cd Sistema-de-Gesti-n-de-Empleados-TalentoPlus-S.A.S.

# Crear el archivo de configuración de secretos
cp env.example .env
Paso 2: Configuración de Variables de Entorno (.env)Edite el archivo .env recién creado. Este archivo define las credenciales de los servicios externos y es fundamental para el arranque seguro de la aplicación.¡ATENCIÓN! Las variables MYSQL_ROOT_PASSWORD y MYSQL_PASSWORD deben ser reemplazadas por contraseñas fuertes y únicas para este entorno. El resto de las claves se proporcionan para un arranque inmediato.Paso 3: Arranque de la SoluciónEl comando a continuación construye la imagen de la API y levanta ambos servicios (API y MySQL) en red.Bash# Construir y levantar servicios en segundo plano (-d)
docker-compose up -d --build
ServicioPuerto de HostPuerto de ContenedorNotasTalentoPlus.Api516280Puerto de acceso a Swagger.MySQL Database33073306Puerto externo para herramientas de base de datos.Paso 4: Acceso y Pruebas (Swagger UI)Una vez que los contenedores estén activos (verifique con docker ps), la API es completamente funcional.Abra su navegador en la siguiente URL:http://localhost:5162/swagger
Utilice la credencial de administrador inicial para obtener un token de acceso (JWT):CredencialValorEmailadmin@talentoplus.comPasswordAdmin123!🔐 Ejemplo de Archivo de Configuración (env.example)Este archivo define todas las variables requeridas por el orquestador Docker y la aplicación ASP.NET Core.Fragmento de código# --------------------------------------------------------------------------
# CONFIGURATION FOR: Employee Management System - TalentoPlus S.A.S.
# --------------------------------------------------------------------------

# --- DATABASE CONFIGURATION: MySQL Service ---
# Required for initial database setup. MUST be unique and strong.
MYSQL_ROOT_PASSWORD=<ROOT_PASSWORD_HERE>
MYSQL_DATABASE=talento_plus
MYSQL_USER=talento_user
MYSQL_PASSWORD=<DB_USER_PASSWORD_HERE>

# Connection string used by the TalentoPlus.Infrastructure project (inside the container).
ConnectionStrings__DefaultConnection=Server=mysql_db;Port=3306;Database=talento_plus;Uid=talento_user;Pwd=${MYSQL_PASSWORD};

# --- SECURITY & AUTHENTICATION (JWT) ---
# Secret key used for signing JWT tokens. DO NOT CHANGE unless redeploying production secrets.
JWT_SECRET=a-string-secret-at-least-256-bits-long 
JWT_ISSUER=TalentoPlus
JWT_AUDIENCE=TalentoPlus

# --- EXTERNAL SERVICES: AI (Gemini) ---
# API Key for access to the Gemini model (used for the AI-powered dashboard).
GEMINI_API_KEY=AIzaSyBLA38WAFA5KJsRHLB0SynVBZU0f6GpYzI

# --- EXTERNAL SERVICES: SMTP Email ---
# Configuration for sending emails (e.g., password recovery, notifications).
# NOTE: This password is an App Password for increased security (STPM: iryn givo cygy ifwc).
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_USER=admin@talentoplus.com
SMTP_PASSWORD=iryn givo cygy ifwc
SMTP_FROM=no-reply@talentoplus.com
🛡️ Mantenimiento y ConfiabilidadPruebas Unitarias y de IntegraciónLa confiabilidad del sistema está respaldada por una amplia cobertura de pruebas. Antes de cualquier despliegue, se debe garantizar que todas las pruebas sean exitosas.Ejecutar todas las pruebas:Bash# Salir del modo de contenedores si está activo
docker-compose down

# Ejecutar el comando de pruebas en el host (requiere .NET SDK 9.0 local)
dotnet test
Gestión de Migraciones (Entity Framework Core)Las migraciones de EF Core se aplican automáticamente durante el arranque del contenedor. Sin embargo, para crear nuevas migraciones o realizar mantenimiento manual, utilice los siguientes comandos (ejecutados en el host):Bash# Comando de actualización de la base de datos (aplicación de migraciones)
dotnet ef database update --project TalentoPlus.Infrastructure --startup-project TalentoPlus.Api

# Comando para generar una nueva migración (si se realizaron cambios en las entidades)
dotnet ef migrations add <NombreDeLaMigracion> --project TalentoPlus.Infrastructure --startup-project TalentoPlus.Api
Detener ServiciosPara apagar la base de datos y la API de forma segura:Bashdocker-compose down
