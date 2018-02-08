def search4vowels(word):
    """Display any vowels found in the word"""
    vowels = set('aeiou')
    found = vowels.intersection(set(word))
    return bool(found)


def search4letters(phrase: str, letters: str='aeiou') -> set:
    """Return a set of the 'letters' found in 'phase'."""
    # return set(letters).intersection(set(phase))
    return {v for v in letters if v in phrase}

