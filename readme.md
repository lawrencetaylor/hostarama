Master of the House
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
