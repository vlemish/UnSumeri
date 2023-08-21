# UnSumeri

## How to deploy locally

You need a docker to work with it, you can use the latest version of Docker Desktop or any other option you like (WSL, Rancher, you name it). https://www.docker.com/products/docker-desktop/.

There is a docker-compose file which deploys all applications just by calling the script, to do so
* open a terminal of your choice
* jump to root directory of this repository
* call `docker compose build`
* after a successfull build, call `docker compose up`
* if there is no errors then you good to go

## How to access deployed services locally

There are two services:

* Authorization
* Service

Both services have configured swagger, which you can access by next URLs:

* Authorization: http://localhost:5002/swagger/index.html
* Service: http://localhost:5000/swagger/index.html
