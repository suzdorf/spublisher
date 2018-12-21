# spublisher 

An application which provides you to automatically build and deploy your application to IIS. This a pre-alpha version which has a lack of features and was not tested enough so there might be some bugs and lack of a user expirience.

## Getting started

* Get the latest installer from the [releases](https://github.com/suzdorf/spublisher/releases) page
* Proceed with the installing and reboot.
* Enable IIS and required IIS components on Windows.
* Now you can run your configurations from the command prompt with Administrator permissions by command

```
spublisher

```

## "Hello world" configuration

In our first configuration we will create an IIS site with the name "helloworld"
* Create an empty folder C:\SrcFolder
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
          "Path": "C:\\SrcFolder"
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

IIS management build step allows you to create an application tree in IIS. The configuration below would deploy a web site with two applications withing "FileServer" and "Api". And also two virutal directories within "FileServer".

```

{
  "BuildSteps": [
    {
      "Name": "A Site with two applications within will be created",
      "Type": "iis",
      "Applications": [
        {
          "Name": "site",
          "Path": "C:\\sitesrc",
          "Applications": [
            {
              "Name": "api",
              "Path": "..\\ApiSrc"
            },
            {
              "Name": "files",
              "Path": "..\\FileServerSrc",
              "Applications": [
                {
                  "Name": "images",
                  "Path": "..\\ImagesFolder",
                  "IsVirtualDirectory": true
                },
                {
                  "Name": "videos",
                  "Path": "..\\VideosFolder",
                  "IsVirtualDirectory": true
                }
              ]
            }
          ]
        }
      ]
    }
  ]
}
```

Applications would be available by the urls:

```

http:\\site
http:\\site\api
http:\\site\files
http:\\site\files\images
http:\\site\files\videos

```
- **"Name"** parameter

In this parameter you provide an application name which will be used as an application name and its url binding.

```

{
  "BuildSteps": [
    {
      ...
      "Applications": [
        {
          ...
          "Name": "yourappname",
          ...
        }
      ]
    }
  ]
}

```

This parameter is required. 

- **"Path"** parameter

In this parameter you provide a storage path to the folder of you application. You can specify a relative path. 

```

{
  "BuildSteps": [
    {
      ...
      "Applications": [
        {
          ...
          "Path": "..\\SrcPath",
          ...
        }
      ]
    }
  ]
}

```

This parameter is required. 

- **"Applications"** parameter

In this parameter you provide an array of the appilications which application, site or virtual directory contains.

```

{
  "BuildSteps": [
    {
      ...
      "Applications": [
        {
          ...
          "Applications": [
            {...},
            {...},
            {
              ...
              "Applications": [
              ]
            }
          ]
          ...
        }
      ],
      ...
    }
  ]
}


```

This parameter is not required. 


- **"AppPoolName"** parameter

In this parameter you provide an application pool name which you want to use for an application. If an application pool with the provided name does not exists spublisher will create apppool with that name.

```

{
  "BuildSteps": [
    {
      ...
      "Applications": [
        {
          ...
          "AppPoolName": "YourAppPoolName",
          ...
        }
      ]
    }
  ]
}

```

This parameter is not required. If you do not provide it for a root appilication "DefaultAppPool" will be used. If you don't provide it for a nested application site level application pool will be used.

- **"ManagedRuntimeVersion"** parameter

You should specify this parameter in case you want to deploy a .NET Framework asp.net application. Add "v4.0" or "v2.0" values to that parameter depending on .NET Framework version you want to use. 

```

{
  "BuildSteps": [
    {
      ...
      "Applications": [
        {
          ...
          "ManagedRuntimeVersion": "v4.0",
          ...
        }
      ]
    }
  ]
}

```

This parameter is not required. In case you provide it AppPool with that version will be created and assigned to your application. Otherwise "No Managed Code" value will be used.

- **"IsVirtualDirectory"** parameter

You should specify this parameter in case you want to create a virtual directory.

```

{
  "BuildSteps": [
    {
      ...
      "Applications": [
        {
          ...
          "IsVirtualDirectory": true,
          ...
        }
      ]
    }
  ]
}

```

This parameter is not required. By default the value is "false".