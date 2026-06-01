Prerequisites needed to run code
1. SSMS
2. SQLExpress
3. Visual Studio (if 2020 does not work, may require visual studio 2026)
4. .Net 10 Runtime
5. Angular cli

Steps to run Backend 
1.Clone or Extract PolicyStreetAssessment
2. Get PolicyStreetAssessment.bacpac from \PolicyStreetAssessment-master\PolicyStreetAssessment-master 
3. Go to SSMS, Go to Database in Object Explorer, Right click Databases and Import Data-tier Application
4. Import PolicyStreetAssessment.bacpac
5. Go to appsettings.json, point ConnectionString -> DefaultConnection to your SQL Express and Database name 
e.g : "DefaultConnection": "Server=localhost\\SQLEXPRESS01;Database=PolicyStreetAssessment;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;",
6. Run IISExpress on Visual Studio and Run frontend 

Steps to run FrontEnd
1.Clone or Extract zip https://github.com/Kabiir-saidi/PolicyStreetAssessmentFrontEnd
2.Go location of \PolicyStreetAssessmentFrontEnd-master\PolicyStreetAssessmentFrontEnd-master 
3. run npm install
4. run ng serve -o

API Swagger : https://localhost:44386/swagger/index.html
