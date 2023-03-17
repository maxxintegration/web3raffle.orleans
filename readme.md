# VeeFriends

## Project Structure & Conventions

### Namespaces

`VeeFriends.{Type}.{Name}.{RelatedSubProjectName}`

- **Type** = Type of project (eg. Web|Api|Shared|Utilities|Data|Blockchain)
- **Name** = Name of project (eg. WWW == "Website at root of domain")
- **RelatedSubProjectName** = A child project of the parent level namespace

#### Example:

**VeeFriends.Web.Www.Client** - `Web` implies that it lives on a web-host (or web server) and `Www` implies that this particular project lives in the root of the domain (eg. google.com - no subdomain) and `client` implies that it is related to the parent namespace and should be a direct dependency of that project. In this case `client` is a SPA (single page application) where there are front-end only assets and static files.

## Running Your Code

This project requires Node.js (`node`) and Microsoft .NET (`dotnet`) command line tools to run.

- Install `dotnet` (https://dotnet.microsoft.com/en-us/download)
- Install `node` (https://nodejs.org/en/)

### Veefriends.com

The Veefriends.com website project is represented in this code-base as two main projects `VeeFriends.Web.Www` & `VeeFriends.Web.Www.Client`. To run or debug this project you can either use the command line directly or use Microsoft Visual Studio.

#### Using the command line:

From the command line navigate to the `VeeFriends.Web.Www.Client` folder and make sure all the node.js dependencies are installed. Execute the following:

`$ npm i`

You'll only need to run this once or whenever there is a new NPM package installed.

Next **go back** a directory to the `VeeFriends.Web.Www` and execute the dotnet watcher (this will turn on hot-reload as well as build and compile the project):

`$ dotnet watch run`

Your default web browser will open up to the web application.

### meet.veefriends.com

The meet.veefriends.com website project is represented in this code-base as two main projects `VeeFriends.Web.Meet` & `VeeFriends.Web.Meet.Client`. To run or debug this project you can either use the command line directly or use Microsoft Visual Studio.

#### Using the command line:

From the command line navigate to the `VeeFriends.Web.Meet.Client` folder and make sure all the node.js dependencies are installed. Execute the following:

`$ npm i`

You'll only need to run this once or whenever there is a new NPM package installed.

Next **go back** a directory to the `VeeFriends.Web.Meet` and execute the dotnet watcher (this will turn on hot-reload as well as build and compile the project):

`$ dotnet watch run`

Your default web browser will open up to the web application.
