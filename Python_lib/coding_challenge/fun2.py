

def generate_rule_nums():
	base1 = 3
	base2 = 5
	my_sum = 0
	my_max = 10000
	for i in range(1, 10000):
		if i%base1 == 0 or i%base2 == 0:
			my_sum += i
	print(my_sum)
generate_rule_nums()
