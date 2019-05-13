## OpenShift Training
======================================================
1. Overview
```
1 master nodes
1 infrastructure nodes
24 "application" nodes
An NFS server
```
2. Tools installation
```
Download tool and copy it to the path
export PATH=$PATH:~/OpenShift
oc version
```
3. CLI and Web Console
```
oc login master.xv33.acumena-os.com
oc login master.xv33.acumena-os.com --insecure-skip-tls-verify=true
oc project explore-XX
https://master.xv33.acumena-os.com
```
4. Basic Commands
```
oc get pod parksmap-1-hx0kv -o yaml
oc get services
oc get service parksmap -o yaml
oc get pods
# The Service has selector stanza that refers to deploymentconfig=parksmap.
The Pod has multiple Labels:
   deploymentconfig=parksmap
   app=parksmap
   deployment=parksmap-1
```
5. Scaling
```
oc describe service parksmap
oc get dc  # DeploymentConfig
oc get rc  # ReplicationController
oc scale --replicas=2 dc/parksmap
oc get rc
oc get pods
oc describe svc parksmap
oc get endpoints parksmap
```
6. Self Healing 
```
oc delete pod parksmap-1-hx0kv && oc get pods
```
7. Route
```
oc get routes
oc get services
oc expose service parksmap
oc get route
```
8. Logs
```
Applications → Pods → specific pod -> logs
oc get pods
oc logs parksmap-1-hx0kv
# in Kibana
"View Archive" in web
kubernetes.pod_name:"parksmap-1-hx0kv" AND kubernetes.namespace_name:"explore-XX"
kubernetes.namespace_name:"explore-XX"
kubernetes.namespace_name:"explore-XX" AND message:"Failure executing"  # search message
```
9. grant user
```
oc project explore-05
oc policy add-role-to-user view -z default
# -z, --serviceaccount=[]: service account in the current namespace to use as a user
# The -z flag will only work for service accounts in the current namespace. Beware of this if you attempt to use it in the future.
oc policy add-role-to-user view userXX
# re-deploy app
click "Applications" and then "Deployments". You’ll see your only application, parksmap, listed. Click that. click Deploy
```
10 remote operations
```
oc get pods
oc rsh parksmap-2-tegp4
ls /
# exit first from remote shell
oc exec parksmap-2-tegp4 -- ls -l /parksmap.jar
oc rsh parksmap-2-tegp4 whoami
```
11. Source-to-Image (S2I)
```
OpenShift runs the S2I process inside a special Pod, called a Build Pod
 With Git as our version control system (VCS), we are using the concept of Branches/Tags.
 oc get builds
 oc logs -f builds/nationalparks-1
 ## After the build has completed and successfully:
The S2I process will push the resulting Docker-formatted image to the internal OpenShift registry
The DeploymentConfiguration (DC) will detect that the image has changed, and this will cause a new deployment to happen.
A ReplicationController (RC) will be spawned for this new deployment.
The RC will detect no Pods are running and will cause one to be deployed, as our default replica count is just 1.
oc get pods
oc get routes
http://nationalparks-explore-XX.cloudapps.xv33.acumena-os.com/ws/info/
```
12. Add MongoDB after done
```
oc get dc
oc env dc nationalparks -e MONGODB_USER=mongodb -e MONGODB_PASSWORD=mongodb -e MONGODB_DATABASE=mongodb -e MONGODB_SERVER_HOST=mongodb
oc get dc nationalparks -o yaml
oc env dc/nationalparks --list
oc get dc  # version incremented
# query data
http://nationalparks-explore-XX.cloudapps.xv33.acumena-os.com/ws/data/all
http://nationalparks-explore-XX.cloudapps.xv33.acumena-os.com/ws/data/load
http://gitlab-ce-workshop-infra.cloudapps.xv33.acumena-os.com/userXX/nationalparks/blob/1.2.0/src/main/java/com/openshift/evg/roadshow/parks/db/MongoDBConnection.java#L44-l48
```
13. labels
```
oc describe route nationalparks
oc label route nationalparks type=parksmap-backend
http://parksmap-explore-05.cloudapps.xv33.acumena-os.com/
```
14. Configuration
```
Most applications require configuration using environment variables, configuration files and command line arguments
there is a convenient and platform-independent mechanism in OpenShift to configure applications, which is called ConfigMap.
A ConfigMap can be used to store key-value properties, configuration files, JSON blobs and alike.
# Create config file first
$ oc create configmap nationalparks --from-file=application.properties=./application-dev.properties
# In the above command, the content of application-dev.properties file will be provided to the application container as a properties file called application.properties
oc describe configmap nationalparks
oc get configmap nationalparks -o yaml
oc set volumes dc/nationalparks --add -m /deployments/config --configmap-name=nationalparks
# remove env vars
oc env dc/nationalparks MONGODB_USER- MONGODB_PASSWORD- MONGODB_DATABASE- MONGODB_SERVER_HOST-
```
15. App Health
```
Liveness Probe
A liveness probe checks if the container in which it is configured is still running. If the liveness probe fails, the kubelet kills the container, which will be subjected to its restart policy. Set a liveness check by configuring the template.spec.containers.livenessprobe stanza of a pod configuration.
Readiness Probe
A readiness probe determines if a container is ready to service requests. If the readiness probe fails a container, the endpoints controller ensures the container has its IP address removed from the endpoints of all services. A readiness probe can be used to signal to the endpoints controller that even though a container is running, it should not receive any traffic from a proxy. Set a readiness check by configuring the template.spec.containers.readinessprobe stanza of a pod configuration.
```
16. Jenkins
```
Clone the code from Git repo
Build the code and run unit tests
Build a docker image from the code (S2I)
Deploy the docker image into Dev
Run automated tests against the Dev deployment
Run manual tests against the Dev deployment
Wait for the Deployment Manager to either approve or reject the deployment (e.g. manual tests have revealed an unacceptable number of bugs)
If approved, deploy to Live
# permission
oc policy add-role-to-user edit -z jenkins
# Remove Dev from parksmap
oc label route nationalparks type-
# Create Live Environment
oc create configmap nationalparks-live --from-file=application.properties=./application-live.properties
oc tag nationalparks:latest nationalparks:live
Since we do not want to immediately run or deploy the Live version of nationalparks when the image changes, we want the ability for the Dev and Live deployments to run different versions of the nationalparks image simultaneously. This will allow developers to continue changing and deploying Dev without affecting the Live environment. In order to achieve that, you will create a new Docker image tag using the CLI. This new tag will be what the Live deployment will look for changes to
This command says "please use the existing image that the tag nationalparks:latest points to and also point it at nationalparks:live." Or, in other words "create a new tag (live) that points to whatever latest points to.
# link the ConfigMap
oc set volumes dc/nationalparks-live --add -m /deployments/config --configmap-name=nationalparks-live
# Add Route populate the database by pointing your browser to the nationalparks-live route url
http://nationalparks-live-explore-XX.cloudapps.xv33.acumena-os.com/ws/data/load/
# Label service
oc label route nationalparks-live type=parksmap-backend
After creating the pipeline, parksmap should use the Live container instead of the Dev container so that deployments to the Dev container does not disrupt the parksmap application. You can do that by removing the type label from the Dev route and adding it to the Live route
# Disable Automatic Deployment of nationalparks (dev)
https://master.xv33.acumena-os.com/console/project/explore-XX/edit/dc/nationalparks
uncheck the first Automatically start ...
Pipeline workflows are defined in a Jenkinsfile, either embedded directly in the build configuration, or supplied in a Git repository and referenced by the build configuration.
```

17. Create Openshift Pipeline
```
# Web Hooks
http://gitlab-ce-workshop-infra.cloudapps.xv33.acumena-os.com/userXX/nationalparks
paste URL of generic hook
uncheck SSL
add hook
```
18. Rollback
```
oc get dc
oc rollback nationalparks-live
Automatic deployment of new images is disabled as part of the rollback to prevent unwanted deployments soon after the rollback is complete. To re-enable the automatic deployments run this
oc set triggers dc/nationalparks-live --auto
oc rollback nationalparks-live-4  # roll forward
```
19. Templates
```
oc create -f https://raw.githubusercontent.com/openshift-roadshow/mlbparks/1.0.0/ose3/application-template-eap.json
oc get template
oc new-app mlbparks -p APPLICATION_NAME=mlbparks -p GIT_REF=1.0.0
oc get template mlbparks -o yaml
```

20. Clustering Stateful Java EE Applications JBoss EAP Clustering
```
Clustering in JBoss EAP is achieved using the Kubernetes discovery mechanism for finding other JBoss EAP containers and forming a cluster. This is done by configuring the JGroups protocol stack in standalone-openshift.xml with <openshift.KUBE_PING/> elements.
1) The OPENSHIFT_KUBE_PING_NAMESPACE environment variable must be set. If not set, the server will act as if it is a single-node cluster (a "cluster of one").
2) The OPENSHIFT_KUBE_PING_LABELS environment variables should be set. If not set, pods outside of your application (albeit in your namespace) will try to join.
3) Authorization must be granted to the service account the pod is running under to be allowed to access Kubernetes' REST api.
oc scale dc/mlbparks --replicas=2
pplications → Pods page -> Detail -> Open Java Console
jgroups -> channel -> ee -> view
```
21. resources
```
# our Workshop
http://labs-workshop-infra.cloudapps.xv33.acumena-os.com/#/workshop/roadshow/module/further-resources
# Interactive learning portal
https://learn.openshift.com/
# Openshift Origin
https://www.openshift.org/
# Minishift
https://www.openshift.org/minishift/
# Openshift.io
https://openshift.io/
# Openshift Dedicated
https://www.openshift.com/dedicated
# container platform
https://www.openshift.com/
# Openshift for developers
https://www.openshift.com/promotions/for-developers.html
# DevOps with Openshift
https://www.openshift.com/promotions/devops-with-openshift.html
```
