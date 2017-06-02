#r "./packages/FAKE/tools/FakeLib.dll"
#load "./paket-files/lawrencetaylor/83d214421c9c6abcbf9bbecb727b4a26/FsiSessionBuilder.fsx"

open Fake

// The folder containing host file configuration scripts for each environment
let environmentsDir = getBuildParamOrDefault "envDir" "environments"
/// The name of the file in the environments folder you wish to read host file entries from
let environment = getBuildParamOrDefault "env" "development"


/// Labeled pairs of (IPAddress, HostName)
type HostEntries = (string * (string * string) list) list

/// Location of hosts file
let hostsFileLocation = @"C:\Windows\System32\drivers\etc\hosts"

open FsiSessionBuilder


Target "Run" (fun () -> 

    // Print out environment directory
    environmentsDir
    |> System.IO.Path.GetFullPath
    |> sprintf "Reading environments from directory %s"
    |> traceImportant

    // Print out environment to write to config
    environment
    |> sprintf "Writing hosts file entries for environment \"%s\"" 
    |> traceImportant

    // Path to file containing hosts file configuration
    let environmentHostsEntries = environmentsDir </> (sprintf "%s.fsx" environment)

    // Dynamically load host file entries in to an F# interactive session
    match
      fsiEval 
        {
          load environmentHostsEntries
          openModule "Development"
          let! (hostEntries : HostEntries) = evaluate "hosts"
          return hostEntries
        } |> execute (createFsiSession defaultConfig ) with 
    | Success groupedHostEntries -> 
      // If successful, write entries to hosts file
      use sw = new System.IO.StreamWriter(hostsFileLocation, false)
      groupedHostEntries
      |> List.iter(
          fun (label, hostEntries) -> 
            sw.WriteLine(sprintf "# %s" label)
            hostEntries |> List.iter(fun (ip, host) -> sw.WriteLine(sprintf "%s    %s" ip host))
            sw.WriteLine())
    | Failure message -> 
      traceError message
)

RunTargetOrDefault "Run"