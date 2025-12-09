### Employee Management System - TalentoPlus S.A.S.

This is a comprehensive Human Resources and Catalog Management System developed in ASP.NET Core 8.0 using a Clean Architecture approach. The project features full staff administration (CRUD) and the crucial Mass Employee Import functionality from an Excel file.Key functionalities include dynamic column mapping, transactional catalog management (Departments, Positions), and advanced features like an AI-powered dashboard and secure employee authentication.This system was developed by Mariana Quintero Cardona (Clan: Hopper).🛠️ Technologies UsedBackend: ASP.NET Core 8.0Language: C#Database: MySQLORM: Entity Framework CoreExcel Import: EPPlusContainerization: Docker⚙️ How to Run the Solution (From Scratch)The recommended way to run this project is using Docker and Docker Compose (which handles both the API and the MySQL database).PrerequisitesGit: Installed to clone the repository.Docker Desktop: Installed and running..NET 8.0 SDK: Required only for running tests or local development.Step 1: Clone the RepositoryBashgit clone https://github.com/MarianaQC/Sistema-de-Gesti-n-de-Empleados-TalentoPlus-S.A.S..git
cd Sistema-de-Gesti-n-de-Empleados-TalentoPlus-S.A.S.
Step 2: Configure Environment VariablesThe project uses an environment file (.env in the root) to manage sensitive secrets like connection strings and JWT keys. You must create this file.Create a file named .env in the root directory and paste the following content, replacing the placeholders (<YOUR_...>):Fragmento de código# -------------------------------------------------------------------
# Database Configuration (for MySQL container)
# -------------------------------------------------------------------
MYSQL_ROOT_PASSWORD=<YOUR_MYSQL_ROOT_PASSWORD>
MYSQL_DATABASE=talento_plus
MYSQL_USER=talento_user
MYSQL_PASSWORD=<YOUR_TALENTO_USER_PASSWORD>

# -------------------------------------------------------------------
# Connection String (Used by the API)
# -------------------------------------------------------------------
ConnectionStrings__DefaultConnection=Server=mysql_db;Port=3306;Database=talento_plus;Uid=talento_user;Pwd=<YOUR_TALENTO_USER_PASSWORD>;

# -------------------------------------------------------------------
# JWT Secrets
# -------------------------------------------------------------------
Jwt__Key=<YOUR_VERY_SECURE_JWT_KEY_OF_AT_LEAST_32_CHARS>
Jwt__Issuer=TalentoPlus
Jwt__Audience=TalentoPlus

# -------------------------------------------------------------------
# SMTP Service (for real email sending)
# -------------------------------------------------------------------
Smtp__Host=<YOUR_SMTP_HOST>
Smtp__Port=<YOUR_SMTP_PORT>
Smtp__Username=<YOUR_SMTP_USERNAME>
Smtp__Password=<YOUR_SMTP_PASSWORD>
Note: The ConnectionStrings__DefaultConnection variable uses mysql_db as the server name, which is the service name defined in the Docker Compose network.Step 3: Run with Docker ComposeIf you have a docker-compose.yml file, run the following command. If not, you may need to add one to handle both the API and the database containers. Assuming a docker-compose.yml exists:Bashdocker-compose up -d --build
Step 4: Access the APIThe API will be available at:Swagger UI (Documentation): http://localhost:5162/swaggerAPI Root: http://localhost:5162/apiEndpoints and Functionality OverviewThis table summarizes the functionality implemented in the system as required by the workshop:#RequisiteEndpoint/FunctionStatusAPPLICATION WEB (ADMINISTRATOR)1Create employeePOST /api/employeesImplemented2Edit employeePUT /api/employees/{id}Implemented3List employeesGET /api/employeesImplemented4Delete employeeDELETE /api/employees/{id}Implemented5Import ExcelPOST /api/excel/importImplemented6Generate PDF CVGET /api/employees/{id}/pdfImplementedDASHBOARD WITH AI7Total employeesGET /api/dashboardImplemented8Employees on vacationGET /api/dashboardImplemented9Third card (Active)GET /api/dashboardImplemented10Natural language AI QueryPOST /api/ai/queryImplementedPUBLIC REST API11List departments (Public)GET /api/departmentsImplemented12Employee Self-RegistrationPOST /api/employeeregistrationImplemented13Real email sending ServiceService (SMTP)Implemented14Employee Login (JWT)POST /api/auth/loginImplementedPROTECTED REST API15Consult MY informationGET /api/employees/meImplemented16Download MY PDFGET /api/employees/me/pdfImplementedTESTING172 Unit Testsdotnet testImplemented182 Integration Testsdotnet testImplementedAccess CredentialsTo access the protected administrator endpoints, JWT authentication is required.RoleEmailDefault PasswordNotesAdministratoradmin@talentoplus.comAdmin123!Used to obtain a token for all CRUD and mass import operations.Employee(Employee's Email)(Employee's Document ID)Imported employees use their document ID as the default password.Repository LinkGitHub Repository:https://github.com/MarianaQC/Sistema-de-Gesti-n-de-Empleados-TalentoPlus-S.A.S.
