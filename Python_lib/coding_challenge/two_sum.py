# 2 sum problem (no duplicate in the list) time=n, space=n
def twosum(nums=(6, 7, 1, 11, 15, 3, 16, 5, 10, 2, 4), target=6):
    lookup = dict(((v, i) for i, v in enumerate(nums)))
    found = []  # contain found values
    i_pairs = []  # contian found index pairs
    count = 0
    for i, v in enumerate(nums):
        if lookup.get(target-v, i) != i and v not in found:
            found.extend([v, target-v])
            i_pairs.append((i, lookup.get(target-v)))
            count += 1
    print(f'count = {count}')
    print(i_pairs)

twosum()