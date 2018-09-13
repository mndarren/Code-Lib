# Grails
=======================================
1. install grails
```
curl -s https://get.sdkman.io | bash
source "$HOME/.sdkman/bin/sdkman-init.sh"
sdk install grails
grails -version
```
2. create grails app
```
grails create-app com.vogella.grails.guestbook  # @ grails-app/conf/application.yml/ to speccify a default package
grails list-profiles  # list all profiles
grails create-app myApp -profile rest-api
grails create-app myApp -profile org.grails.profiles:react:1.0.2
grails profile-info plugin  # get detaied info about a profile
```
3. run grails app
```
grailsw run-app  # without installed grails
grails run-app   # which installed grails
http://localhost:8080/com.vogella.grails.guestbook
```
4. 