UI Deployment
-------------

Esquio UI is a regular ASP.NET Core project as Blazor Wasm, you can get the code and deploy as you want, by default you need to configure the Database to be used ( Sql Server, Postress or MySql ) and the OpenId provider.

To simplify a local deployment we provide different *Docker compose* files located on **build** directory::

  docker-compose -f ./build/docker-compose-demo-with-ui-sqlserver up --build
  docker-compose -f ./build/docker-compose-demo-with-ui-npgsql up --build
  docker-compose -f ./build/docker-compose-demo-with-ui-mysql up --build

By default, a OpenId provider with ``https://demo.identityserver.io`` is used but you can change to use any other standard OpenId ( Auth0 etc ) or Azure AD.

Docker
^^^^^^

On ``https://hub.docker.com/r/xabarilcoding/esquioui`` you can find also Esquio UI docker images.

To run locally, pull it typing::

  docker pull xabarilcoding/esquioui:4.0.0

To run this image, you have to set several environment variables, indicating some database and OpenId server properties::

  docker run xabarilcoding/esquioui:4.0.0 -p 9000:80 \
   -e Data__ConnectionString=<your-connection-string> \
   -e Data__Store=[SqlServer|MySql|NpgSql]
   -e Security__DefaultUsers__0__ApplicationRole=[Contributor|Reader|Management]
   -e Security__DefaultUsers__0__SubjectId=<your-subject-id>
   -e Security__OpenId__ClientId=<openid-clientid> \
   -e Security__OpenId__Audience=<openid-audience> \
   -e Security__OpenId__Scope=<openid-scope> \
   -e Security__OpenId__Authority=<openid-authority> \
   -e Security__OpenId__ResponseType=<openid-response-type> \
   -e Security__OpenId__IsAzure=<IsAzure>

* Security__OpenId__ClientId: your client id ``interactive.public on https://demo.identityserver.io``
* Security__OpenId__Audience: your security audience, this value is used on AddJwtBeaer to validate Beaer Tokens ``api on https://demo.identityserver.io``
* Security__OpenId__Scope: your security audience ``api on https://demo.identityserver.io``
* Security__OpenId__Authority: your authority  ``https://demo.identityserver.io``
* Security__OpenId__ResponseType: your openid flow response type to use ``code`` 
* Security__OpenId__IsAzure: If you are using azure Ad as your provider this needs to be ``true``

To configure Azure AD please follow existing docs on this like  `this
<https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/hosted-with-azure-active-directory?view=aspnetcore-3.1>`_. An example of AAD configuration is:

* Security__OpenId__ClientId: ``"762ed2b1-8107-44d5-9c9b-72c75749fb35"``
* Security__OpenId__Audience: ``"c1cc1b6a-7580-49e5-8ab8-0f3cc9f97127"`` *remove the url prefix that AAD use api://*
* Security__OpenId__Scope: ``c1cc1b6a-7580-49e5-8ab8-0f3cc9f97127/ui``
* Security__OpenId__Authority: your authority  ``https://login.microsoftonline.com/bf3ae910-a895-44e6-aed1-0ed9601d9035``
* Security__OpenId__ResponseType: ``code`` 
* Security__OpenId__IsAzure: ``true``


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
            image: "xabarilcoding/esquioui:3.0.0"
            imagePullPolicy: IfNotPresent
            env:
              - name: ASPNETCORE_ENVIRONMENT
                value: "Development"
              - name: DATA__CONNECTIONSTRING
                valueFrom:
                  secretKeyRef:
                    name: esquio-ui-secret
                    key: connection-string
              - name: DATA__STORE
                value: "[SqlServer|NpgSql|MySql]"
              - name: SECURITY__DEFAULTUSERS__0__APPLICATIONROLE
                value: "[Contributor|Reader]"
              - name: SECURITY__DEFAULTUSERS__0__SUBJECTID
                value: "<your-subject-id>"
              - name: DATA__STORE
                value: "[SqlServer|NpgSql|MySql]"
              - name: SECURITY__OPENID__CLIENTID
                value: "<your-openid-clientid>"
              - name: SECURITY__OPENID__AUDIENCE
                value: "<openid-audience>"
              - name: SECURITY__OPENID__SCOPE
                value: "<openid-scope>"
              - name: SECURITY__OPENID__AUTHORITY
                value: "<openid-authority>"                 
              - name: SECURITY__OPENID__RESPONSETYPE
                value: "<openid-response-type>"
              - name: Security__OpenId__IsAzure
                value: "<true/false>"
            ports:
              - name: http
                containerPort: 80
                protocol: TCP

And apply it with the command::

  kubectl apply -f esquio-ui.yaml