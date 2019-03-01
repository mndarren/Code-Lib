# Good Points Python
==========================================
1. De-dup List[Dict]
```
[dict(t) for t in {tuple(d.items()) for d in rel_lsit}]
```
2. List IntEnum class numbers
```
list(map(int, <IntEnumClass>))
```
3. csv to json
```
def csv_to_json(csv_file, json_file, sample_data_dict):
    import csv
    import json

    csvfile = open(csv_file, 'r')
    jsonfile = open(json_file, 'w')

    fieldnames = tuple(list(sample_data_dict.keys()))
    next(csvfile)
    reader = csv.DictReader(csvfile, fieldnames)
    jsonfile.write('[')
    for row in reader:
        json.dump(row, jsonfile)
        jsonfile.write(',\n')
    jsonfile.write(']')

    csvfile.close()
    jsonfile.close()
```
