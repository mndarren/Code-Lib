# Kubernetes Lib
====================================<br>
1. One Practice Process
```
Create Kube cluster -> Deploy an app -> Explore you app -> Expose you app publicly -> Scale up your app -> Update your app
```
2. Nodes
```
Master node: control non-master nodes. Can be replicated!
             3 processes: kube-apiservice, kube-controller-manager, kube-scheduler
Non-master node: 2 processes(kubelet, kube-proxy)
```
3. Basic Concepts
```
Kubernetes -> Pod -> Container
Pod: smallest and simplest uint. can have one container or more than one container in one pod.
     storage, unique IP. Container can be Docker container
Service: REST object. load balance. make frontend pods and backend pods decoupling. cluster IP
Volume: solve shared files in one pod with more than one containers, solve on-disk file gone when restarting pod
Namespace: each kebernete only in one namespace. solve saperate users problem.
Ctl -> Maser node -> non-mater nodes
```
4. Other objects
```
Deployment
DaemonSet
StateSet
ReplicaSet
Job
```