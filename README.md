# ParalleleVerteilteSysteme-Lab
Repo for Lab of "Parallele und Verteilte Systeme"

## Docker Image

use in Dockerfile:
```
FROM ghcr.io/maxtrautwein/paralleleverteiltesysteme-lab:master
```

pull the Image
```
docker pull ghcr.io/maxtrautwein/paralleleverteiltesysteme-lab:master
```

_GitHub has a now reported bug where it recommends Installing/Pulling the Signature instead of the Image._
_For your convenience please use the here provided Tags_

## Sample use
Create a `compose.yaml`File.
You may use [compose.yaml](https://github.com/MaxTrautwein/ParalleleVerteilteSysteme-Lab/blob/master/compose.yaml) as an Example.

Start the container with `docker compose up`

You may test if it is running by navigating to http://localhost:8080/items

If you run the App in the `Development` mode you may acces the OpenAPI Spec under: http://localhost:8080/openapi/v1.json and the Swagger GUI Under http://localhost:8080/swagger/index.html

The Example Includes a ProstgresDB.
For Actual use I reccomend that the Data is mounted, so that it is not stored inside the container and that Docker Secrets are Used with actual Passwords.

The [Example Compose](https://github.com/MaxTrautwein/ParalleleVerteilteSysteme-Lab/blob/master/compose.yaml) Shows an sample for OTLP with Jaeger that may be enabled if so desired.

## Kubernetes (Minikube)
Current Setup

1. Start Minikube `minikube start`
2. Apply all Deployments
3. Apply all Services
4. `kubectl get service backend` Get the Port
5. `minikube ip`
6. Access the IP And Port

### Server Use
The Examples above use `localhost`. You can easily also host this on a Server.

Ensure that the Configued Port is not blocked by any Firewall, then replace `localhost` with the IP of the Server. You may also replace it with a Domain if you have mapped one.

For HTTPS I Suggest you use a Reverse Proxy in a Container such as Traefik. 

## 12 Factors
The Lab Requieres this Project to implement and Document the 12 factors and how they are use here.
I Applied the Factors where not already covered in a Spep by Step Process. In any case they will be documented here.

### I. Codebase
> One codebase tracked in revision control, many deploys

This one is implicitly coverd by the fact that the code is entierly located on this GitHub Repo.

###  II. Dependencies
>  Explicitly declare and isolate dependencies

All dependencies are defined in the [Backend.csproj](https://github.com/MaxTrautwein/ParalleleVerteilteSysteme-Lab/blob/master/Backend/Backend/Backend.csproj) file.
This defines all dependencies in an non code file.

### III. Config
> Store config in the environment

The System may be configured using Enviorment Variables.
The Possible Values and Effects are documented in the [Example Compose](https://github.com/MaxTrautwein/ParalleleVerteilteSysteme-Lab/blob/master/compose.yaml)

### IV. Backing services
> Treat backing services as attached resources

A Postgres DB may be Connected using a Connection String.
This String may be provided via an Enviorment Variable or a Docker Secret.

### V. Build, release, run
> Strictly separate build and run stages

The Project will be Build during the Release of a new Docker Image.
A multi step process is used to only publish the compiled Image. 
One could do so manually aswell.
The Project is then Run in the Container.

Those are seperate Stages.

### VI. Processes
> Execute the app as one or more stateless processes

The State is only ttored in the Database.
The Backend my connect to a Supported DB to get access to the state.

### VII. Port binding
> Export services via port binding

The Application Will bind by default to Port `8080`. In compliance with Factor III
that port may be canged.
Besides that Internal Port one may bind any desired and available Port form the Docker Compose.

### VIII. Concurrency
> Scale out via the process model

The Entity framework combined with the DB handels this aspect.

### IX. Disposability
> Maximize robustness with fast startup and graceful shutdown

The App can be easily stopped an Started via Docker. By using a precompiled build hosted on GitHub a Fast startup is guaranteed.

### X. Dev/prod parity
> Keep development, staging, and production as similar as possible

You may switch between development and production Use via a Enviorment Variable in the Compose file.
Other Deployments such as Staging may be done using the save Image and configuring it as requiered.
Should a Debug BUILD be requiered that may be chnged in the Docker file. (_Although that is not recommended for Performance and Security reasons_)

### XI. Logs
> Treat logs as event streams

Logs and Telemetry data are Published via OpenTelememetry.
There is the Option to use the Docker Logs Systems and or Puplish via OTLP

### XII. Admin processes
> Run admin/management tasks as one-off processes

There are currently no suche elements in the Code.
Should the become a relevant part I Will ensure that Factor 12 is followed
