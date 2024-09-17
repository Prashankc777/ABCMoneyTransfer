@ECHO OFF
TYPE name.txt   
cd ..
cd ABCMoneyTransfer.App
dotnet ef dbcontext scaffold name=DefaultConnection Microsoft.EntityFrameworkCore.SqlServer -o Entities --context AppDbContext --project ../ABCMoneyTransfer.Data --force --no-build                                               
ECHO DATABASE SCAFFOLDING DONE...
PAUSE