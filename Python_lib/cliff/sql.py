#!/usr/bin/env python
# -*- coding: utf-8 -*-


"""
SQL Generation happens here.
"""

from abc import ABC, abstractmethod
from .model import Country, County, City, PostalZone
from typing import List, Set, Iterable, NamedTuple


class SqlOutput(NamedTuple):
    """
    This is a simple tuple that relates a filename to SQL contents.
    """
    filename: str  #: the filename
    sql: str  #: the SQL


class SqlGenerator(ABC):

    @abstractmethod
    def states(self, country: Country) -> SqlOutput:
        """
        Generate a SQL statement for the states within a country.

        :param country: the country for which you want to generate the SQL INSERT command
        :return: a SQL INSERT command string
        """
        pass  # pragma: no cover

    @abstractmethod
    def cities(self, country: Country) -> SqlOutput:
        """
        Generate a SQL statement for all the cities within a country.

        :param country: the country for which you want to generate the SQL INSERT command
        :return: a SQL INSERT command string
        """
        pass  # pragma: no cover

    @abstractmethod
    def counties(self, country: Country) -> SqlOutput:
        """
        Generate a SQL statement for all the counties within a country.

        :param country: the country for which you want to generate the SQL INSERT command
        :return: a SQL INSERT command string
        """
        pass  # pragma: no cover

    @abstractmethod
    def cities_counties(self, country: Country) -> SqlOutput:
        """
        Generate a SQL statement for the relationships between cities and counties in a
        state.

        :param country: the country for which you want to generate the SQL INSERT command
        :return: a SQL INSERT command string
        """
        pass  # pragma: no cover

    @abstractmethod
    def postal_zones(self, country: Country) -> SqlOutput:
        """
        Generate a SQL statement for the postal zones.

        :param country: the country for which you want to generate the SQL INSERT command
        :return: a SQL INSERT command string
        """
        pass  # pragma: no cover

    @abstractmethod
    def place_names(self, country: Country) -> SqlOutput:
        """
        Generate a SQL statement for the "place names" assigned to postal zones.

        :param country: the country for which you want to generate the SQL INSERT command
        :return: a SQL INSERT command string
        """
        pass  # pragma: no cover

    def all(self, country: Country) -> Iterable[SqlOutput]:
        """
        Generate all the SQL statements.

        :param country: the country for which you want to generate the SQL INSERT command
        :return: an iteration of all the SQL statements
        """
        yield self.states(country)
        yield self.cities(country)
        yield self.counties(country)
        yield self.cities_counties(country)
        yield self.postal_zones(country)
        yield self.place_names(country)


class CooperSqlGenerator(SqlGenerator):
    """
    Use a SQL generator to produce Cooper SQL commands from data objects.
    """

    def states(self, country: Country) -> SqlOutput:
        """
        Generate a cooper SQL statement for a set of states.

        :param country: the country for which you want to generate the States SQL INSERT command
        :return: a SQL INSERT command string
        """
        # Start the SQL command off...
        sql = [
            '-- Insert State Seed Data ({})'.format(country.alpha_2_code),
            'INSERT INTO postaddr.states(code, country, alpha_code,' +
            'subdivision_category, name)',
            'VALUES'
        ]
        # Add all the lines.
        states_sql = [
            ("('{code}', '{country}', '{alpha_2_code}', " +
             "'{subdivision_category}', '{name}')").format(
                code=s.code,
                country=s.country.alpha_2_code,
                alpha_2_code=s.alpha_code,
                subdivision_category=s.subdivision_category,
                name=s.name.replace("'", "''")  # (escape single quotes)
            ) for s in country.states
        ]
        # Combine the individual INSERT tuples and add them to the SQL.
        sql.append(',\n'.join(states_sql))
        # Put it all together and terminate the statement.
        return SqlOutput('states.sql', ''.join(['\n'.join(sql), ';']))

    def cities(self, country: Country) -> SqlOutput:
        """
        Generate a cooper SQL statement for all the cities within a country.

        :param country: the country for which you want to generate the cities SQL INSERT command
        :return: a SQL INSERT command string
        """
        # We want to create a flattened list of all the counties so we can alphabetize them as
        # a group.
        flat: List[City] = []
        for state in country.states:
            for city in state.cities:
                flat.append(city)
        flat.sort(key=lambda c: c.name)
        # Now create a list to hold the SQL.
        cities: List[str] = []
        for city in flat:
            cities.append(
                "('{id}', '{state_code}', '{name}')".format(
                    id=city.id,
                    state_code=city.state.code,
                    name=city.name.replace("'", "''")
                ))
        # Now we can get down to creating the total SQL command.
        sql = [
            '-- Insert City Seed Data ({})'.format(country.alpha_2_code),
            'INSERT INTO postaddr.cities(id, state, name)',
            'VALUES',
            ',\n'.join(cities)
        ]
        # Put it all together and terminate the statement.
        return SqlOutput('cities.sql', ''.join(['\n'.join(sql), ';']))

    def counties(self, country: Country) -> SqlOutput:
        """
        Generate a cooper SQL statement for all the counties within a country.

        :param country: the country for which you want to generate the counties SQL INSERT command
        :return: a SQL INSERT command string
        """
        # We want to create a flattened list of all the counties so we can alphabetize them as
        # a group.
        flat: List[County] = []
        for state in country.states:
            for county in state.counties:
                flat.append(county)
        flat.sort(key=lambda c: c.name)
        # Now create a list to hold the SQL.
        counties: List[str] = []
        # Create a list of individual SQL tuples for each city.
        for county in flat:
            counties.append(
                "('{id}', '{state_code}', '{name}')".format(
                    id=county.id,
                    state_code=county.state.code,
                    name=county.name.replace("'", "''")))
        # Now we can get down to creating the total SQL command.
        sql = [
            '-- Insert County Seed Data ({})'.format(country.alpha_2_code),
            'INSERT INTO postaddr.counties(id, state, name)',
            'VALUES',
            ',\n'.join(counties)
        ]
        # Put it all together and terminate the statement.
        return SqlOutput('counties.sql', ''.join(['\n'.join(sql), ';']))

    def cities_counties(self, country: Country) -> SqlOutput:
        """
        Generate a cooper SQL statement for the relationships between cities and counties in a
        state.

        :param country: the country for which you want to generate the States SQL INSERT command
        :return: a SQL INSERT command string
        """
        # Let's start by creating a list of all the individual relationships.
        relationships: List[str] = set()
        for state in country.states:
            for city in state.cities:
                for county in city.counties:
                    relationships.add(
                        "('{}', '{}')".format(
                            city.id, county.id
                        )
                    )
        # Now we can get down to creating the total SQL command.
        sql = [
            '-- Insert City-County Relationship Seed Data ({})'.format(country.alpha_2_code),
            'INSERT INTO postaddr.cities_counties(city, county)',
            'VALUES',
            ',\n'.join(relationships)
        ]
        # Put it all together and terminate the statement.
        return SqlOutput('cities_counties.sql', ''.join(['\n'.join(sql), ';']))

    def postal_zones(self, country: Country) -> SqlOutput:
        """
        Generate a cooper SQL statement for the postal zones.

        :param country: the country for which you want to generate the SQL INSERT command
        :return: a SQL INSERT command string
        """
        # We want to create a flattened list of all the postal zones so we can alphabetize them as
        # a group.
        flat: List[PostalZone] = []
        for state in country.states:
            for city in state.cities:
                for pz in city.postal_zones:
                    flat.append(pz)
        flat.sort(key=lambda p: int(p.postal_code))
        # Create a set to hold all the place name relationship entries.
        pz_sqls: List[str] = []
        # Now let's populate the set...
        for postal_zone in flat:
            pz_sqls.append(
                "('{id}', '{city_id}', '{postal_code}') /* {city_name}, {state_abbrev} */".format(
                    id=postal_zone.id,
                    city_id=postal_zone.city.id,
                    postal_code=postal_zone.postal_code,
                    city_name=postal_zone.city.name,
                    state_abbrev=postal_zone.city.state.code
                )
            )
        # Set up the SQL command.
        sql = [
            '-- Insert Postal Zones Seed Data ({})'.format(
                country.alpha_2_code),
            'INSERT INTO postaddr.postal_zones(id, city, postal_code)',
            'VALUES',
            ',\n'.join(pz_sqls)
        ]
        # Put it all together and terminate the statement.
        return SqlOutput('postal_zones.sql', ''.join(['\n'.join(sql), ';']))

    def place_names(self, country: Country) -> SqlOutput:
        """
        Generate a cooper SQL statement for the "place names" assigned to postal zones.

        :param country: the country for which you want to generate the SQL INSERT command
        :return: a SQL INSERT command string
        """
        # Create a set to hold all the place name relationship entries.
        relationships: Set[str] = set()
        # Now let's populate the set...
        for state in country.states:
            for postal_zone in state.postal_zones:
                for place_name in postal_zone.place_names:
                    relationships.add(
                        "('{pzid}', '{place_name}') /* {city}, {state}, {postal_code} */".format(
                            pzid=postal_zone.id,
                            place_name=place_name.replace("'", "''"),
                            city=postal_zone.city.name,
                            state=state.code,
                            postal_code=postal_zone.postal_code
                        )
                    )
        # Set up the SQL command.
        sql = [
            '-- Insert Place Names Seed Data ({})'.format(country.alpha_2_code),
            'INSERT INTO postaddr.postal_zone_place_names(postal_zone,' +
            ' place_name)',
            'VALUES',
            ',\n'.join(relationships)
        ]
        # Put it all together and terminate the statement.
        return SqlOutput('place_names.sql', ''.join(['\n'.join(sql), ';']))
