#!/usr/bin/env python
# -*- coding: utf-8 -*-

"""
This is the entry point for the command-line interface (CLI) application.
.. note::

    To learn more about Click visit the `project website <http://click.pocoo.org/5/>`_.  There is
    also a very helpful `tutorial video <https://www.youtube.com/watch?v=kNke39OZ2k0>`_.
"""

from .model import Country
from .sql import CooperSqlGenerator, SqlOutput
import click
import os
import sys
from pathlib import Path
from typing import List


class Info(object):
    """
    This is an information object that can be used to pass data between CLI functions.
    """
    def __init__(self):  # Note that this object must have an empty constructor.
        pass


pass_info = click.make_pass_decorator(Info, ensure=True)  \
    #: a decorator for functions that pass 'Info' objects


@click.group()
@pass_info
def cli(_: Info):
    """
    This is a sample Click command-line application.
    """
    pass


@cli.command()  # This is a sub-command of 'cli'.
@click.option('--country', type=str, metavar='<country>',
              help="the two-letter (alpha-2-code) that identifies the country")
@click.option('--outdir', type=click.Path(), default='dist', metavar='<outdir>')
@click.option('--combined/--separate', default=False,
              help='Combine all the SQL in a single file?')
@click.option('--sequence', type=int, default='0', metavar='<startat>',
              help="the initial sequence number for output file names")
@click.argument('datafile', type=click.Path(exists=True), required=True)
@pass_info
def sqlize(_: Info,
           country: str,
           outdir: os.PathLike,
           combined: bool,
           sequence: int,
           datafile: os.PathLike):

    # Make sure the output directory is available.
    try:
        os.makedirs(outdir, exist_ok=True)
    except OSError:  # pragma: no cover
        click.echo(click.style(
            'Cannot create or access {outdir}.'.format(outdir=outdir),
            fg='red')
        )
        # We can't go on!
        sys.exit(1)

    # Load all the data.
    _country = Country(alpha_2_code=country)
    click.echo(click.style('Loading {}'.format(datafile), fg='magenta'))
    try:
        _country.load(datafile)
    except Exception:  # pragma: no cover
        click.echo(click.style(
            'An error occurred while attempting to load {}'.format(datafile),
            fg='red')
        )
        # There's no going on.
        sys.exit(1)

    # We'll need a generator for this.  (In the future we may need to specify which generator
    # from the command line, but at present, there's only the one.)
    sql_generator = CooperSqlGenerator()

    # Create a list that will contain the unmodified output
    sqlouts_raw: List[SqlOutput] = list(sql_generator.all(_country))

    # If we haven't been asked to put all the output into one honking big file...
    if not combined:
        # Get all the outputs and put 'em in a list.
        sqlouts_final = [
            SqlOutput(
                filename='{n}-create-{country}-{filename}'.format(
                    n="%04d" % (sequence + i,),
                    country=country,
                    filename=sqlouts_raw[i].filename),
                sql=sqlouts_raw[i].sql
            )
            for i in range(0, len(sqlouts_raw))
        ]
    else:
        # Otherwise, get all the outputs and join them into one large string.
        all_sql = '\n\n'.join([sqlo.sql for sqlo in sqlouts_raw])
        # We'll now produce a list (but it just contains the one item).
        sqlouts_final = [
            SqlOutput(
                filename='{n}-create-{country}-all'.format(
                    country=country,
                    n="%04d" % (sequence,)),
                sql=all_sql)
        ]

    # Write all the files.
    for sqlo in sqlouts_final:
        # Get the filename where the output will go.
        outfile_path = os.path.join(outdir, sqlo.filename)
        click.echo(click.style("Writing {}".format(sqlo.filename), fg='magenta'))
        try:
            with open(outfile_path, 'w') as outfile:
                outfile.write(sqlo.sql)
        except IOError:  # pragma: no cover
            # What happened?
            click.echo(click.style(
                "An error occurred while writing to {}.".format(outfile_path),
                fg='red')
            )
            # That's that.
            sys.exit(1)
    # It looks like everything worked out.
    click.echo(click.style(
        'Created {count} files in {outdir}'.format(
            count=len(sqlouts_final),
            outdir=os.path.abspath(os.path.expanduser(outdir))
        ), fg='green'))



