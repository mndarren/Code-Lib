import logging
from stearns_pogo.cnx import Connection
from stearns_nucleus.postgres.model import *
from stearns_nucleus.postgres.models.orgs.orgs import Orgs

logging.basicConfig(level=logging.DEBUG)
logging.getLogger('sqlalchemy.engine').setLevel(logging.INFO)

# pragma: no cover
with Connection('postgresql://postgres:andybear123@localhost:5432/cooper_core') as cnx:
    session = cnx.load().session()

    rows = session.query(Orgs)

    for row in rows:
        print(row)