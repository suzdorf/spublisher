## What is spublisher? 

Spublisher is a user-friendly deployment configuration running tool for developers and DevOps engineers who want to automate their development lifecicle.

- [Documentation](https://github.com/suzdorf/spublisher/wiki)
- [Installers](https://github.com/suzdorf/spublisher/releases)

## What can you do with spublisher? 
Spublisher can run configurations written in the Json format which allow you to perform in any order the following operations:

* Run commands from the command line
* Deploy applications to Internet Information Services (IIS)
* Create databases
* Execute sql scripts
* Restore backup files

## Getting started

* Get the latest installer from the [releases](https://github.com/suzdorf/spublisher/releases) page.
* Proceed with the installing and rebooting.
* That's it now you can execute spublisher configurations by running a command below in the command prompt.

```
spublisher

```

## "Hello world" configuration

In the first configuration we will create an IIS site with the name "helloworld"

* Check that IIS components are enabled
* Create an empty folder C:\Temp\SrcFolder
* Add to that folder an empty html file index.html with the content

```

 <h2>Hello world!</h2>
 
```

* Add to some folder a spublisher.json file with the content:

```
{
  "BuildSteps": [
    {
      "Name": "Your first IIS site created through spublisher",
      "Type": "iis",
      "Applications": [
        {
          "Name": "helloworld",
          "Path": "C:\\Temp\\SrcFolder"
        }
      ]
    }
  ]
}

```

* Run cmd within the folder with spublisher.json file with Administrator permissions and execute the following command:

```

spublisher

```

* spublisher has created for you a site in IIS with the name "helloworld"

* Add to the hosts file the following line:

```
127.0.0.1 helloworld

```

* Your site is available in the browser by the address 

```
http:\\helloworld

```

## Build steps
* [Command line](https://github.com/suzdorf/spublisher/wiki/Command-line-build-step)
* [Internet Information Services (IIS)](https://github.com/suzdorf/spublisher/wiki/IIS-management-build-step)
* [Database Management](https://github.com/suzdorf/spublisher/wiki/Sql-build-step)
## Examples
### Command line
* [Executing batch and powershell scripts](https://github.com/suzdorf/spublisher/wiki/Executing-batch-and-powershell-scripts)
### Internet Information Services (IIS)
* [Deploy a simple site with a static html page](https://github.com/suzdorf/spublisher/wiki/Deploy-a-simple-site-with-a-static-html-page)
* [Deploy a site with virtual directories and .NET application](https://github.com/suzdorf/spublisher/wiki/Deploy-a-site-with-virtual-directories-and-.NET-application)
* [Create site with different bindings](https://github.com/suzdorf/spublisher/wiki/Create-site-with-different-bindings)
### Database Management
* [Create database](https://github.com/suzdorf/spublisher/wiki/Create-database)
* [Create database and execute scripts for it](https://github.com/suzdorf/spublisher/wiki/Create-database-and-execute-scripts-for-it)
* [Restore database](https://github.com/suzdorf/spublisher/wiki/Restore-database)
### Full deploy configuration examples
* [Deploy a site with an Angular client, ASP.NET core server application and a PostgreSQL database](https://github.com/suzdorf/spublisher/wiki/Deploy-a-site-with-an-Angular-client,-ASP.NET-core-server-application-and-a-PostgreSQL-database)

