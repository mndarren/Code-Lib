# find the number of 500th in fib list

def print_num_th_fib(num):
    x = 1
    y = 1
    for i in range(3, num+1):
        x, y = y, x+y
    print(y)

print_num_th_fib(500)
