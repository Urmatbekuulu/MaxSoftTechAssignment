# MaxSoftTechAssignment
This project is created for company MaxSoft as a technical assignment.


1) To run this project locally fill appsettings.json or create userSecrets for Web layer and fill. And make sure connection string is correct.

Example of settings.

{
  "ConnectionStrings:DefaultConnection": "Data Source=DESKTOP-CEH28SA\\SQLEXPRESS;Initial Catalog=maxsoftdb;Integrated Security=True",
  "Admin": {
    "Username": "Admin@Admin",
    "Password": "Admin@Admin",
    "Email": "admin@admin.com"
  },
  "JwtConfiguration": {
    "Issuer": "issuer",
    "Audience": "audience",
    "Secret": "DDEJ6vQon6plUlznjUIsP4xYBnIjznsd",
    "ValidFor":60
  }
}

2) Update database. Command for dotnet-ef tool is: "dotnet-ef database update  -s MaxSoftTechAssignment.WEB -p MaxSoftTechAssignment.DAL"

3) Build and Run MaxSoftTechAssignment.WEB project
