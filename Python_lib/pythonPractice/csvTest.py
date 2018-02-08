import pprint, csv
from datetime import datetime

def convert2ampm(time24: str) -> str:
	return datetime.strptime(time24, '%H:%M').strftime('%I:%M%p')

# with open('buzzers.csv') as data:
# 	ignore = data.readline()
# 	flights = {}
# 	for line in data:
# 		k, v = line.strip().split(',')
# 		flights[convert2ampm(k)] = v.title()


with open('buzzers.csv') as data:
	ignore = data.readline()
	reader = csv.reader(data)
	flights = {convert2ampm(row[0]): row[1].title() for row in reader}

pprint.pprint(flights)

when = {dest: [k for k, v in flights.items() if v == dest]
		for dest in set(flights.values())}

pprint.pprint(when)
