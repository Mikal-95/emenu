# EmenuQuiz

Steps to the run:
1 - Clone the repo using git clone url from the github.
2 - Open the project using Visual studio. The project should restore all packages automatically but if it fails, here's the required packages:
- CloudinaryDotNet
- Microsoft.AspNetCore.Mvc.NewtonsoftJson
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Rational
- Microsoft.EntityFrameworkCore.Design
- MySql.EntityFrameworkCore
- Newtonsoft.Json
- Swashbuckle.AspNetCore

3 - Setting up the database:
- The application uses MySql database, so first you should download the MySql from the official site (Note: the version I used is 8.1.0).
- After installing the server, you should add password to your root user in order to access the database.
- Create a database named "emenu"
- Add your db username, password, db name and the server host in the appsettigns in the ConnetionStrings, example:

```
"ConnectionStrings": {
    "Default": "server=127.0.0.1:3306;database=emenu;uid=root;pwd=password;"
  }
```
- Migration is provided to populate the database, to run the migration:
In the terminal of the visual studio project:

1 - Run:    

```
dotnet tool install -g dotnet-ef
```

2 - Now from inside the project run: 


```
dotnet ef database update
```

Your databse should be populated by now.


4 - Cloudinary settings:
- Sign up for cloudinary and get your CloudName, APIKey and APISecret. Add them in the appsettings in the CloudinarySetting section


5 - Run the application:
Now, a swagger page should open
