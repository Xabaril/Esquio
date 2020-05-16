Http Store
==========

Esquio Http store connects against a Http Api that has been previously deployed on a Esquio UI.

> In `samples/GettingStarted.AspNetCore.Mvc.HttpStore <https://github.com/Xabaril/Esquio/tree/master/samples/GettingStarted.AspNetCore.Mvc.HttpStore>`_ you'll find a full example of this store.

UI Deployment
-------------

You need a database to store the information. For demo usage, you can use `build/docker-compose-infraestructure.yaml` to run a SQL image on 5433 port::

  docker-compose -f ./build/docker-compose-infraestructure.yaml up

You need a OpenId provider. For demo usage, you can use ``https://demo.identityserver.io/``.

On ``https://hub.docker.com/r/xabarilcoding/esquioui`` you can find Esquio UI docker images. 

Docker
^^^^^^

To run locally, pull it typing::

 docker pull xabarilcoding/esquioui:3.0.100006574-preview

To run this image, you have to set several environment variables, indicating some database and OpenId server properties::

  docker run xabarilcoding/esquioui:3.0.100006574-preview -p 9000:80 \
   -e ConnectionStrings__Esquio=<your-connection-string> \
   -e Security__OpenId__ClientId=<openid-clientid> \
   -e Security__OpenId__Audience=<openid-audience> \
   -e Security__OpenId__Authority=<openid-authority> \
   -e Security__OpenId__ResponseType=<openid-response-type> 

* Security__OpenId__ClientId: your client id (i.e. ``interactive.public``)
* Security__OpenId__Audience: your security audience (i.e. ``api``)
* Security__OpenId__Authority: your authority (i.e. ``https://demo.identityserver.io``)
* Security__OpenId__ResponseType: your openid flow response type to use (i.e. ``code``) 

Kubernetes
^^^^^^^^^^

Alternatively, you can deploy it on your kubernetes cluster in a simple way. 

Save your connection string into a file::

  echo '<your-connection-string>' > ./connection-string.txt

And create a secret using this file::

  kubectl create secret generic esquio-ui-secret --from-file=connection-string=./connection-string.txt

Save the following yaml in a file named `esquio-ui.yaml`::

  apiVersion: v1
  kind: Service
  metadata:
    name: esquio-ui-release
    labels:
      app.kubernetes.io/name: esquio-ui
      app.kubernetes.io/instance: esquio-ui-release
  spec:
    type: LoadBalancer
    ports:
      - port: 80
        targetPort: http
        protocol: TCP
        name: http
    selector:
      app.kubernetes.io/name: esquio-ui
      app.kubernetes.io/instance: esquio-ui-release
  ---
  apiVersion: apps/v1beta2
  kind: Deployment
  metadata:
    name: esquio-ui-release
    labels:
      app.kubernetes.io/name: esquio-ui
      app.kubernetes.io/instance: esquio-ui-release
  spec:
    replicas: 1
    selector:
      matchLabels:
        app.kubernetes.io/name: esquio-ui
        app.kubernetes.io/instance: esquio-ui-release
    template:
      metadata:
        labels:
          app.kubernetes.io/name: esquio-ui
          app.kubernetes.io/instance: esquio-ui-release
      spec:
        containers:
          - name: esquio-ui
            image: "xabarilcoding/esquioui:3.0.100006574-preview"
            imagePullPolicy: IfNotPresent
            env:
              - name: ASPNETCORE_ENVIRONMENT
                value: "Development"
            - name: CONNECTIONSTRINGS__ESQUIO
              valueFrom:
                secretKeyRef:
                  name: esquio-ui-secret
                  key: connection-string
              - name: SECURITY__OPENID__CLIENTID
                value: "<your-openid-clientid>"
              - name: SECURITY__OPENID__AUDIENCE
                value: "<openid-audience>"
              - name: SECURITY__OPENID__AUTHORITY
                value: "<openid-authority>"                 
              - name: SECURITY__OPENID__RESPONSETYPE
                value: "<openid-response-type>"                                            
            ports:
              - name: http
                containerPort: 80
                protocol: TCP

And apply it with the command::

  kubectl apply -f esquio-ui.yaml


Installation
-------------

Install ``Esquio.AspNetCore`` package, typing the following command using the .NET Core CLI::

        dotnet add package Esquio.Http.Store

or using Powershell or Package Manager::

        Install-Package Esquio.Http.Store

or install via NuGet.


In the ``ConfigureServices`` method of the ``Startup`` class, register the specific service for this store::

                AddEsquio()
                .AddHttpStore(options =>
                {
                    options
                        .UseBaseAddress("http://localhost:1368/") //this is Esquio UI base address
                        .UseApiKey("b6+KYpSY8VPMBmHLNJ00z80aPOe+Li4EGe4idoKKI1A=") // this is a Api Key on Esquio UI (only Reader permission is Required);
                });

``AddHttpStore`` method registers the http store to use. in this case, based on the default configuration system of `ASP.NET Core <https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-2.2>`_

And that's all. Log in Esquio UI and configure your toggles as you need.