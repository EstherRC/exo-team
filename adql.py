# pip install astroquery
from astroquery.gaia import Gaia
import pandas as pd

# Coordinates for the planet

planet_ra = 270.0
planet_dec = 66.56070833

# ADQL query to get the data
query = """
SELECT source_id, ra, dec, parallax, phot_g_mean_mag, 
       DISTANCE(POINT({planet_ra}, {planet_dec}), POINT(ra, dec)) AS angular_distance
FROM gaiadr3.gaia_source
WHERE 1=CONTAINS(
    POINT({planet_ra}, {planet_dec}), 
    CIRCLE(POINT(ra, dec), 0.5)
)
"""
job = Gaia.launch_job(query)
results = job.get_results()

results_df = results.to_pandas()
results_df.to_csv("data/gaia_stars.csv", index=False)
