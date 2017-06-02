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
