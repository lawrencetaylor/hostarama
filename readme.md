Hostarama
===

Simple host entry configuration using F#.  

* Store different hosts file configurations under the `environments` folder;
* Define the environment you want to run in `main.fsx`

  ```
    let environment = "development"
  ```

* Run 
  ```
  main.cmd
  ```
  from the command line.  In order to ensure it is able to write to your hosts file you will probably need to run this with elevated permissions.


## Specifying host file entries

Each file defining an enviroment of hosts file entries (such as [this one](environments/development.fsx)) should contain a `hosts` property, which specifies the host file entries.  These are grouped to make it easier to view logical sets of host file entries.   For example, the environment containing the code:

```
let localHost = "127.0.0.1"
let boot2Docker = "192.168.99.101"

let hosts = 
  [ "Docker", 
      [ boot2Docker, "boot2docker "]

    "Websites",
      [ localHost,   "mydomain.com"
        localHost,   "mydomain2.com"
      ]
  ]
```

is written to the hosts file as 

```
# Docker
192.168.99.101    boot2docker 

# Websites
127.0.0.1    mydomain.com
127.0.0.1    mydomain2.com
```

Defining the hosts file entries defined in F# means you can use use F# expressions within the definition of the entries to increase the maintainability of these configurations.
