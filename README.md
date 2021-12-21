# BDSAProject

## Getting Started

1. Clone the repository using:
```
git clone https://github.com/JacobMoller/BDSAProject.git
```
2. Follow the [Setting up Docker](https://github.com/JacobMoller/BDSAProject/wiki/Setting-up-docker#setup) article
3. Go to the ProjectBank.Server folder and run the program with:
```
dotnet run
```
4. Open the localhost connection in a browser

## User information
To illustrate how the system works with specific roles assigned to specific users, we have created two user accounts.
Use these accounts to be authenticated by the system. 

### Alice The Supervisor
Alice is assigned the supervisor role, which means that she can access the parts of the system that requires Supervisor authorization. 

**E-mail:** alicethebdsasupervisor@gmail.com

**Password:** bdsaSupervisor1!

### Bob The Student
Bob is assigned the student role, which means that he can access the parts of the system that requires Student authorization (and won't have access to the parts that require Supervisor authorization)

**E-mail:** bobthebdsastudent@gmail.com

**Password:** bdsaStudent1!
