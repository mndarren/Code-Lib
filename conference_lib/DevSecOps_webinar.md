# DevSecOps Webinar with Azure (Brendan creator of Kubenetes)
================================
1. Keywords
```
	RBAC -- Role-Based Access Control (principal, objects, verbs)
	Role -- Collection of verbs and objects
	Binding -- Collection between Principal and Roles
```
2. RBAC Role & Binding
```
	kubectl get clusterrolebinding | grep xxxbinding
	kubectl delete xxbinding  # after this, the related user cannot do the role rights
	kubectl create -f cluster_binding.yaml  # run binding rules
	kubectl config use-context xxxbinding  # switch to some binding
	# using Group for role binding (many benefits)
```
3. Secure Cluster: Separate 'image build' from 'image deploy' (2 pipelines with 2 secure processes)
4. Secure code to cluster
```
	Source Repo <- Build Pipeline(k) -> Container Registry(lock)
	Container Registry(lock2) <- Deploy Pipeline(k1 & k2) -> Canary Cluster (lock1,k2), Pod1 cluster (lock1,k2)...
```
5. Nobody can create container, but only Robot. (make sure no vulnerability in your SW)
6. Roles: Developer, Delopyer, Operator, Cluster Admin
```
	Developer: able to read everything in cluster (except secrets)
	Deployer: ability to deploy code and services to the cluster (Robot)
	Operator: ability to delete Pods
	Cluster Admin: build-in role: access Nodes
```
7. Policy and RBAC roles
```
	Policy: inside of resources
	RBAC roles: inside kubenetes
	Policy 1: Dedicated image registry.
	Policy 2: Security review for public IP. (annotation, go through secure review)
	Policy 3: Team metadata
```
8. Cluster services (Kubenetes inside security)
```
	XSS scanning
	Intrusion detection
	Vulnerability scanning
```
