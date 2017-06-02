#r "./packages/FAKE/tools/FakeLib.dll"
#load "./paket-files/lawrencetaylor/83d214421c9c6abcbf9bbecb727b4a26/FsiSessionBuilder.fsx"

/// The name of the file in the environments folder you wish to read host file entries from
let environment = "development"

open Fake
open FsiSessionBuilder

/// Labeled pairs of (IPAddress, HostName)
type HostEntries = (string * (string * string) list) list

/// Location of hosts file
let hostsFileLocation = @"C:\Windows\System32\drivers\etc\hosts"

Target "Run" (fun () -> 

    // Path to file containing hosts file configuration
    let environmentHostsEntries = "environments" </> (sprintf "%s.fsx" environment)

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
            hostEntries |> List.iter(fun (ip, host) -> sw.WriteLine(sprintf "%s    %s" ip host)))
    | Failure message -> 
      traceError message
)

RunTargetOrDefault "Run"