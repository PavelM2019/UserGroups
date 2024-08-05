# User Group Management System

This project consists of two parts: a .NET Core API for the backend and an Angular application for the frontend.

## Installation and Setup

### Prerequisites

Before you begin, make sure you have the following tools installed:

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Node.js and npm](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)

### Clone the Repository

```bash
git clone https://github.com/PavelM2019/UserGroups

### Setting up the backend part.
You need to import the database into your MS SQL Studio  https://github.com/PavelM2019/UserGroups/blob/master/UsersGroups.bak
There is also a script for initializing database tables and stored procedures. The database should be called UsersGroups, if not, you can change it in the appsetting.json file
https://github.com/PavelM2019/UserGroups/blob/master/SQL_Script_init.sql

