## Prerequisites needed to run code

1. SSMS  
2. SQLExpress  
3. Visual Studio (2026 recommended) or Rider   
4. .NET 10 Runtime and SDK
5. Angular CLI  

---

## Steps to run Backend

1. Clone or extract PolicyStreetAssessment  
2. Get `PolicyStreetAssessment.bacpac` from project root  
3. Open SSMS → Object Explorer → Databases → Right click → Import Data-tier Application  
4. Import `PolicyStreetAssessment.bacpac`  
5. Update connection string in `appsettings.json` pointing to your sql engine and database: e.g below

```json
"DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=PolicyStreetAssessment;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;"
```
## Steps to run Frontend
1. Clone repository:
2. https://github.com/Kabiir-saidi/PolicyStreetAssessmentFrontEnd
3. Navigate to project folder
4. Run: npm install
5. Run: ng serve -o
