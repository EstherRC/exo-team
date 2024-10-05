# pip install astroquery
from astroquery.gaia import Gaia
import pandas as pd

# ADQL query to get the data
query = """
SELECT source_id, ra, dec, parallax, phot_g_mean_mag 
FROM gaiadr3.gaia_source
"""
job = Gaia.launch_job(query)
results = job.get_results()

results_df = results.to_pandas()
results_df.to_csv("data/gaia_stars.csv", index=False)
