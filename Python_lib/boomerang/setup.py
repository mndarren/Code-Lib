#!/usr/bin/env python
# -*- coding: utf-8 -*-

# This file is part of boomerang.
# http://10.7.40.79:7990/projects/AFP/repos/boomerang

# Licensed under the ISC license:
# http://www.opensource.org/licenses/ISC-license
# Copyright (c) 2018, Stearns Financial Center <calebg@stearnsbank.com>

import os
from setuptools import setup, find_packages
import stearns_boomerang
from os import path

here = path.abspath(path.dirname(__file__))

# Get the long description from the relevant file
with open(path.join(here, 'README'), encoding='utf-8') as f:
    long_description = f.read()

version = stearns_boomerang.__version__

# If the environment has a build number set...
if os.getenv('buildnum') is not None:
    # ...append it to the version.
    version = "{version}.{buildnum}".format(version=version, buildnum=os.getenv('buildnum'))

setup(
    name='stearns_boomerang',
    version=version,
    description='An Activity Handler and Reporter',
    long_description=long_description,
    keywords='',
    author='Stearns Financial Center',
    author_email='calebg@stearnsbank.com',
    url='http://10.7.40.79:7990/projects/AFP/repos/boomerang',
    license='ISC',
    classifiers=[
        'Development Status :: 3 - Alpha',
        'Intended Audience :: Developers',
        'Topic :: Software Development :: Libraries',
        'License :: OSI Approved :: ISC License',
        'Natural Language :: English',
        'Programming Language :: Python :: 3.6',
                'Programming Language :: Python :: Implementation :: PyPy',
                
    ],
    packages=find_packages(),
    include_package_data=True,
    install_requires=[
        'PTable>=0.9.2,<1',
        'jsonpickle>=0.9.6,<1',
        'kafka-python>=1.4.1,<2',
        'python-interface==1.4.0',
        'elasticsearch==6.3.1',
        'dataclasses==0.6'
    ]
)
