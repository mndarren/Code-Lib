# CronTab
========================================
1. example1
```
from crontab import CronTab

cron = CronTab(user='username')  
job = cron.new(command='python example1.py')  
job.minute.every(1)

cron.write()
```
2. example2
```
from crontab import CronTab

cron = CronTab(user='username')
job1 = cron.new(command='python example1.py')
job1.hour.every(2)
job2 = cron.new(command='python example1.py')  
job2.every(2).hours()

for item in cron:  
    print item
# * */2 * * * python /home/eca/cron/example1.py
# 0 */2 * * * python /home/eca/cron/example1.py
cron.write() 
```
3. example3
```
job.hour.every(15)  
job.hour.also.on(3)  # set the schedule as every 15 hours, and at 3 AM.
job.month.during('APR', 'NOV')
job.dow.on('SUN', 'FRI')
job.every_reboot()  # every reboot job
job.clear()  # clear all job restrictions

from crontab import CronTab

cron = CronTab(user='username')
job = cron.new(command='python example1.py', comment='comment')  
job.minute.every(5)

for item in cron:  
    print item

job.clear()

for item in cron:  
    print item
cron.write()
# */5 * * * * python /home/eca/cron/example1.py # comment
# * * * * * python /home/eca/cron/example1.py # comment
```
4. example4
```
job.enable()
job.enable(False)  # disable
job.is_enabled() 

from crontab import CronTab

cron = CronTab(user='username')
job = cron.new(command='python example1.py', comment='comment')  
job.minute.every(1)

cron.write()

print job.enable()  
print job.enable(False)
```
5. example5 (is_valid)
```
from crontab import CronTab

cron = CronTab(user='username')
job = cron.new(command='python example1.py', comment='comment')  
job.minute.every(1)

cron.write()

print job.is_valid() 
```
6. example6
```
cron.find_command("command name")
cron.find_comment("comment")
cron.find_time(time schedule)

from crontab import CronTab

cron = CronTab(user='username')
job = cron.new(command='python example1.py', comment='comment')  
job.minute.every(1)

cron.write()

iter1 = cron.find_command('exam')  
iter2 = cron.find_comment('comment')  
iter3 = cron.find_time("*/1 * * * *")

for item1 in iter1:  
    print item1

for item2 in iter2:  
    print item2

for item3 in iter3:  
    print item3
```
7. example 7 (remove job)
```
cron.remove_all(comment='my comment') 
cron.remove(job)
cron.remove_all()  # clear all jobs

from crontab import CronTab

cron = CronTab(user='username')
job = cron.new(command='python example1.py')  
job.minute.every(1)

cron.write()  
print "Job created"

# list all cron jobs (including disabled ones)
for job in cron:  
    print job

cron.remove(job)  
print "Job removed"

# list all cron jobs (including disabled ones)
for job in cron:  
    print job
```
8. environmental variables
```
job.env['VARIABLE_NAME'] = 'Value'

from crontab import CronTab

cron = CronTab(user='username')
job = cron.new(command='python example1.py')  
job.minute.every(1)  
job.env['MY_ENV1'] = 'A'  
job.env['MY_ENV2'] = 'B'

cron.write()

print job.env
```
