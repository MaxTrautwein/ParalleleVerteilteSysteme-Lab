# ParalleleVerteilteSysteme-Lab
Repo for Lab of "Parallele und Verteilte Systeme"

## Sample use
Create a `compose.yaml`File: 
```yml
﻿services:
  backend:
    image: ghcr.io/maxtrautwein/paralleleverteiltesysteme-lab:master
    ports: 
        - 8080:8080 # Expose Port 8080
```

Start the container with `docker compose up`
You may test if it is running by navigating to http://localhost:8080/items


