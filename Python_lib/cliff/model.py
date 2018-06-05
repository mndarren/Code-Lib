#!/usr/bin/env python
# -*- coding: utf-8 -*-

"""
This module contains helpful model objects.
"""

import os
import pandas as pd
import math
from typing import Dict, Iterable, Set, Tuple
import uuid


class PostalZone(object):
    """
    A postal zone.
    """
    def __init__(self, postal_code: str, city: 'City'):
        """

        :param postal_code: the postal code
        :param city: the city in which the postal zone resides
        """
        self.id: uuid.UUID = uuid.uuid4()  #: the postal zone's ID
        self.postal_code = postal_code.upper()  #: the postal code
        self.city: City = city  #: the city in which the postal zone resides
        self.place_names: Set[str] = set()  #: names associated with the postal zone


class County(object):
    """
    A country.
    """
    def __init__(self, name: str, state: 'State'):
        """

        :param name: the name of the state
        :param state: the state in which the county resides
        """
        self.id: uuid.UUID = uuid.uuid4()  #: the county's ID
        self.state: 'State' = state  #: the state in which the county resides
        self.name: str = name  #: the county's name
        self._cities: Set['City'] = set()  #: a set of cities that reside within the county

    def associate(self, city: 'City', reflexive: bool=True):
        """
        Associate this county with a city.

        :param city: the city which which the county is associated
        :param reflexive: if `True` the call will also attempt to create a back-reference to this
            county within the city
        """
        self._cities.add(city)
        if reflexive:
            city.associate(self, reflexive=False)


class City(object):
    """
    A city.
    """
    def __init__(self, name: str, state: 'State'):
        """

        :param name: the name of the city
        :param state: the state in which the city resides
        """
        self.id: uuid.UUID = uuid.uuid4()  #: the city's ID
        self.state: 'State' = state  #: the state in which the city resides
        self.name: str = name  #: the name of the city
        self._counties: Set['County'] = set()  #: the associated counties
        self._postal_zones: Dict[str, PostalZone] = {}  #: postal zones within the city

    def associate(self, county: County, reflexive: bool=True):
        """
        Associate this city with a county.

        :param county: the county which which the city is associated
        :param reflexive: if `True` the call will also attempt to create a back-reference to this
            city within the county
        """
        self._counties.add(county)
        if reflexive:
            county.associate(self, reflexive=False)

    @property
    def counties(self) -> Iterable[County]:
        """
        Get the counties associated with this city.

        :return: the counties associated with this city
        """
        return self._counties

    def get_postal_zone(self, postal_code: str) -> PostalZone:
        """
        Get the postal zones associated with this city.

        :param postal_code: the postal zone's code
        :return: the postal zone
        """
        key = postal_code.lower()
        try:
            return self._postal_zones[key]
        except KeyError:
            postal_code = PostalZone(postal_code=postal_code, city=self)
            self._postal_zones[key] = postal_code
            return postal_code

    @property
    def postal_zones(self) -> Iterable[PostalZone]:
        """
        Get the postal zones associated with this city.

        :return: the postal zones
        """
        return self._postal_zones.values()


class State(object):
    """
    A state.
    """
    def __init__(self,
                 code: str,
                 country: 'Country',
                 alpha_code: str,
                 name: str,
                 subdivision_category: str = 'STATE'):
        """

        :param code: the state's ID code
        :param country: the country in which the state resides
        :param alpha_code: the state's alpha code
        :param name: the state's name
        :param subdivision_category: the state's subdivision category
        """
        self.code: str = code  #: the state's ID code
        self.country: Country = country  #: the country in which the state resides
        self.alpha_code: str = alpha_code  #: the state's alpha code
        self.name: str = name  #: the name of the state
        self.subdivision_category: str = subdivision_category  #: the state's subdivision category
        self._counties: Dict[str, County] = {}  #: the counties that reside within the state
        self._cities: Dict[str, City] = {}  #: the citeis that reside within the state

    @property
    def counties(self) -> Iterable[County]:
        """
        Get the counties that reside within the state

        :return: the counties that reside within the state
        """
        return self._counties.values()

    def get_county(self, name: str):
        """
        Get a county that resides within the state.

        :param name: the name of the county
        :return: the county
        """
        key = name.lower()
        try:
            return self._counties[key]
        except KeyError:
            county = County(name=name, state=self)
            self._counties[key] = county
            return county

    @property
    def cities(self) -> Iterable[City]:
        """
        Get the cities that reside within the state.

        :return: the cities that reside within the state
        """
        return self._cities.values()

    def get_city(self, name: str):
        """
        Get a city that resides within the state.

        :param name: the name of the city
        :return: the city

        .. note::

            If the requested object does not exist, it is created upon request.
        """
        key = name.lower()
        try:
            return self._cities[key]
        except KeyError:
            city = City(name=name, state=self)
            self._cities[key] = city
            return city

    @property
    def postal_zones(self) -> Iterable[PostalZone]:
        for city in self._cities.values():
            for postal_code in city.postal_zones:
                yield postal_code


class Country(object):
    """
    A whole country.
    """
    def __init__(self, alpha_2_code: str):
        self.alpha_2_code: str = alpha_2_code
        self._states: Dict[Tuple[str, str], State] = {}

    def get_state(self,
                  name: str,
                  abbrev: str):
        key = (name, abbrev)
        try:
            return self._states[key]
        except KeyError:
            state = State(code='{}-{}'.format(self.alpha_2_code, abbrev),
                          country=self,
                          alpha_code=abbrev,
                          name=name)
            self._states[key] = state
            return state

    @property
    def states(self) -> Iterable[State]:
        """
        Get the states.

        :return: the states
        """
        return self._states.values()

    def load(self, path: str or os.PathLike):
        """
        Load the data from a file.

        :param path: the file path
        """
        reader = pd.read_csv(path, encoding="ISO-8859-1", low_memory=False)
        for index, row in reader.iterrows():
            # We may have more items in the data frame than there are
            _pc = row['Postal Code']
            if _pc is None or math.isnan(_pc):
                # ...just move along.
                continue
            postal_code = str(int(_pc))
            # Get the state for the current row.
            state = self.get_state(name=row['State'],
                                   abbrev=row['State Abbreviation'])
            # Get the county for the current row.
            county = state.get_county(name=row['County'])
            # Get the city for the current row.
            _city_name = row['City']
            if _city_name is None \
                or not isinstance(_city_name, str) \
                or len(_city_name.strip()) == 0:
                _city_name = row['County']
            city = state.get_city(name=_city_name)
            # Associate the city and the county.
            county.associate(city)
            # Retrieve the postal code object from the city.
            postal_zone = city.get_postal_zone(postal_code=postal_code)
            # Now let's see if we have a "place name".
            place_name = row['Place Name']
            if isinstance(place_name, str):
                place_name = place_name.strip()
                if len(place_name) != 0:
                    # It looks like there's a place name, so let's add it.
                    postal_zone.place_names.add(place_name)
