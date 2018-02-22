import fuzzy
from typing import Any, Callable, NamedTuple

_PersonThing = NamedTuple(
    'PersonThing',
    [
        ('taxid', str),
        ('fname', str),
        ('lname', str)
    ]
)


class PersonThing(_PersonThing):

    _fuzzy: Callable[[str], Any] = fuzzy.DMetaphone()  #: our fuzzy-match generator

    def get_virtual_hash(self):
        # Since we're not looking for extreme efficiency, we'll just create a data structure that contains the base
        # information.
        datum = {
            'taxid': self.taxid,
            'fname': self._fuzzy(self.fname),
            'lname': self._fuzzy(self.lname)
        }
        # Convert the dictionary to a string.  That's our hash.
        return str(datum)


person_a = PersonThing('9597872216', 'Caleb', 'Gosiayak')
person_b = PersonThing('9597872216', 'Kaleeb', 'Gosayak')
person_c = PersonThing('9597872217', 'Kaleeb', 'Gosayak')

print(person_a.get_virtual_hash())
print(person_b.get_virtual_hash())
print('a==b? {}'.format(person_a.get_virtual_hash() == person_b.get_virtual_hash()))
print(person_c.get_virtual_hash())
print('a==c? {}'.format(person_a.get_virtual_hash() == person_c.get_virtual_hash()))

# Sorting by the hash...
peeps = sorted([person_a, person_c, person_b], key=lambda item: item.get_virtual_hash())
for peep in peeps:
    print(peep.get_virtual_hash())
