# spublisher 

An application which provides you to automatically build and deploy your application to IIS. This a pre-alpha version which has a lack of features and was not tested enough so there might be some bugs and lack of a user expirience.

## Getting started

* Get the latest installer from the [releases](https://github.com/suzdorf/spublisher/releases) page
* Proceed with installing and reboot.
* Enable IIS and required IIS components on Windows.
* Now you can run your configurations from command prompt with Administrator permissions by command

```
spublisher

```

## Your first configuration

In our first configuration we will create an IIS site with name "test"
* Create an empty folder e.g. C:\Temp
* Add to that folder an empty html file index.html
* Add to that folder a spublisher.json file with content:

```
{
  "BuildSteps": [
    {
      "Name": "Your first IIS site created through spublisher",
      "Type": "iis",
      "Applications": [
        {
          "Name": "test",
          "AppPoolName": "testAppPool",
          "ManagedRuntimeVersion": "No Managed Code",
          "Path": "C:\\Temp",
          "Applications": []
        }
      ]
    }
  ]
}

```

* Run cmd with Administrator permissions and execute the following commands:
```
cd C:\Temp
spublisher

```

* spublisher created for you a site in IIS with the name "test" and an application pool with the name "testAppPool"

* Add to the hosts file the following line:

```
127.0.0.1 test

```

* Your site is available in browser by the address 

```
http:\\test

```

## Build Configuration file overview

Build configuration file is a file which contains the information about build steps are required to deploy your application. That file should have the name **spublisher.json** and a content of the json string. That string should have the root element with the name *"BuildSteps"* and the content of an array of objects. Each element of an array contains an information about a specific build step. 

Example:
```
{
  "BuildSteps": [
    {
      "Name": "Build server application", // The name of the build step which will be displayed in the command line
      "Type": "cmd" // Build step type
	  .. // Some other information about the build step
    },
	    {
      "Name": "Build Client application",
      "Type": "cmd"
	  ...
    }
    {
      "Name": "Deploy to IIS",
      "Type": "iis"
	  ...
    }
  ]
}

```
 
### Command line build step
 
Command line build step allows you to execute command lines one by one in the command prompt. If one line is succeeds another will be executed.
 
In the example below we build a dotnet core application in the specific folder:
```
{
  "BuildSteps": [
    {
      "Name": "Build dotnet core application",
      "Type": "cmd",
      "Commands": [
        "cd C:\\Temp",
        "dotnet build"
      ]
    }
  ]
}

```
- **"RunAsAdministrator"** parameter

If you want to run your command lines with administrator permissions you should add to your build step the following line:

```
"RunAsAdministrator": true

```

By deafult this parameter is set to **"false"**. When you enable this parameter you should run spublisher as administrator otherwise you will get an error.

### IIS management build step

IIS management build step allows you to create an application tree in IIS.

In the example below a site with two applications within will be created. During creation application pools with the specific names will  also be created:

```

{
  "BuildSteps": [
    {
      "Name": "A Site with two applications within will be created",
      "Type": "iis",
      "Applications": [
        {
          "Name": "site", // Site name. The site will be available at http://site
          "AppPoolName": "siteAppPool", // Apppool with that name will be created
          "ManagedRuntimeVersion": "No Managed Code", // CLR version 
          "Path": "C:\\site", // Path to the directory with application
          "Applications": [
			{
			  "Name": "firstapplication", // First application name. The application will be available at http://site/firstapplication
			  "AppPoolName": "firstApplicationPool", // Apppool with that name will be crated
			  "ManagedRuntimeVersion": "No Managed Code",
			  "Path": "C:\\firstapplication",
			  "Applications": []
			},
			{
			  "Name": "secondappliaction", // Second application name. The application will be available at http://site/secondappliaction
			  "AppPoolName": "secondAppliactionPool", // Apppool with that name will be crated
			  "ManagedRuntimeVersion": "No Managed Code",
			  "Path": "C:\\secondappliaction",
			  "Applications": []
			}
		  ]
        }
      ]
    }
  ]
}

```