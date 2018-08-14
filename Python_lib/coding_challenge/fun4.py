# find largest palindrom numbers
numbers = [12345, 6754, 9087, 1234562349240234234235645754632, 12123344213, 12359235934894378437238923892398575624]

def isPalindrom(num):
    if len(num) < 3:
        return true
    num_str = str(num)
    num_enum = enumerate(num_str)
    index_last = len(num_str) - 1
    for i, v in num_enum:
        if v != num_str[index_last-i]:
            return False
    return True

def getPalindrom(num):
    num_str = str(num)
    middle_i = int(len(num_str)/2)
    return num_str[:middle_i] + num_str[::-1][middle_i:]


def findPalindrom(p_list):
    retrun_l = []
    for i in range(0, len(p_list)):
        if isPalindrom(p_list[i]):
            retrun_l.append(str(p_list[i]))
        else:
            retrun_l.append(getPalindrom(p_list[i]))

    return retrun_l

print(findPalindrom(numbers))
print(isPalindrom(1234321))