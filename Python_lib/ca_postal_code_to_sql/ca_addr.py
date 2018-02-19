#!/usr/bin/env python
# -*- coding: latin-1 -*-

import csv
import uuid
import codecs
from model_ca_addr import CAPostalCode
"""
This file will generate SQL insert code for Cooper-core-sql, including state, city,
zipcode and place names of Canada. The input file will be got from Aggdata
"""

# Canadian provinces Abbr
abbr_provinces = {'Alberta': 'AB', 'Saskatchewan': 'SK',
                  'Prince Edward Island': 'PE', 'British Columbia': 'BC',
                  'Manitoba': 'MB', 'New Brunswick': 'NB',
                  'Nova Scotia': 'NS', 'Quebec': 'QC',
                  'Ontario': 'ON', 'Newfoundland and Labrador': 'NL',
                  'Yukon': 'YT', 'Northwest Territory': 'NT',
                  'Nunavut Territory': 'NU'}


def double_quotes(quote_str: str) -> str:
    """single quote to double quote mark in the string"""
    list_str = list(quote_str)
    new_list = list()
    for x in list_str:
        if x != '\'':
            new_list.append(x)
        else:
            new_list.extend(['\'', '\''])
    return ''.join(new_list)


# generator
def organize_data(orig):
    """read data from data file and generate and return model"""
    reader = csv.reader(orig)
    next(reader, None)   # ignore the first
    for line in reader:
        stripped_line = [x.strip() for x in line]
        ca_pastal_code = CAPostalCode(postal_code=stripped_line[0],
                                      city='',
                                      place_names=[],
                                      province=stripped_line[2])
        city_places = double_quotes(stripped_line[1])
        if '(' in city_places:
            split_city_places = city_places.split('(')
            ca_pastal_code.city = split_city_places[0].strip()
            raw_list = split_city_places[1][:-1].split('/')
            ca_pastal_code.place_names = [x.strip() for x in raw_list]
        else:
            ca_pastal_code.city = city_places
        yield ca_pastal_code


def run():
    """open source file to read, and open 4 output files to write"""
    with codecs.open(u'/home/dxie/Downloads/ca_postal_codes_orig.csv', 'r', encoding='latin-1') as orig:
        count = 0
        province_set = set({})
        city_set = set({})
        uuid_city = None
        uuid_code = None
        with codecs.open(r'/home/dxie/code/output/provinceSQL.dat', 'w', encoding='latin-1') as province_sql, open(r'/home/dxie/code/output/citySQL.dat', 'w', encoding='latin-1') as city_sql, open(r'/home/dxie/code/output/codeSQL.dat', 'w', encoding='latin-1') as code_sql, open(r'/home/dxie/code/output/placeSQL.dat', 'w', encoding='latin-1') as place_sql:
            for ca_postal_code in organize_data(orig):
                # check if abbr for province
                if ca_postal_code.province in abbr_provinces.keys():
                    abbr_prov = abbr_provinces[ca_postal_code.province]
                else:
                    abbr_prov = 'NONE'
                # generate province SQL file
                if ca_postal_code.province not in province_set:
                    province_sql.write(u"\t('CA-%s', 'CA', '%s', 'PROVINCE', '%s'),\n" % (abbr_prov, abbr_prov, ca_postal_code.province))
                    print("('CA-%s', 'CA', '%s', 'PROVINCE', '%s')," % (abbr_prov, abbr_prov, ca_postal_code.province))
                    province_set.add(ca_postal_code.province)
                # generate city SQL file
                if ca_postal_code.city not in city_set:
                    uuid_city = uuid.uuid4()
                    city_sql.write(u"\t('%s', 'CA-%s', '%s'),\n" % (uuid_city, abbr_prov, ca_postal_code.city))
                    print("('%s', 'CA-%s', '%s')," % (uuid_city, abbr_prov, ca_postal_code.city))
                    city_set.add(ca_postal_code.city)
                # generate postal code SQL file
                uuid_code = uuid.uuid4()
                code_sql.write(u"\t('%s', '%s', '%s'),\n" % (uuid_code, uuid_city, ca_postal_code.postal_code))
                print("('%s', '%s', '%s'),\n" % (uuid_code, uuid_city, ca_postal_code.postal_code))
                # generate place name SQL file
                for place_item in ca_postal_code.place_names:
                    place_sql.write(u"\t('%s', '%s'),\n" % (uuid_code, place_item))
                    print(u"('%s', '%s')," % (uuid_code, place_item))
                count += 1
                # print('count = %s' % count)
            print('count = %s' % count)


if __name__ == '__main__':
    run()
