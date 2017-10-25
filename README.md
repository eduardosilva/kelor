# Kelor

Kelor is a free .Net tool to help us with simple tasks that can be easily automatized.

I've created this tool because I work with an environment without CI and as I'm not good in power sheel and something like that so, I need to create simple tasks to help me speed up my deploy.

Feel free to download or clone the source code:

https://github.com/filoe/cscore.git

## Features

To see all available commands run:

```bash
kelor
List of commands

copyfolder: Copy folder or only its content
zipfolder: Compress a folder
```

### Compress a folder

```bash
kelor zipfolder -s "c:\temp\mySite" -d "C:\Temp\VersioningArchive" -n "bkp.zip"
```

#### Parameters

* **-s**: Folder to compress;
* **-d**: Zip file destination;
* **-n**: Zip file name;

### Copy folder or only its content

```bash
kelor copyfolder -s "C:\Temp\publish" -d "c:\temp\mySite" -oc
```

#### Parameters

* **-s**: Folder to copy;
* **-d**: Folder destination;
* **-oc**: Indicate only folder content must be copied;