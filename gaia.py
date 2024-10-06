import pandas as pd
import matplotlib.pyplot as plt
from astropy.coordinates import SkyCoord
from astropy import units as u
from astroquery.gaia import Gaia

# Load the exoplanet data from your local dataset
exoplanet_data = pd.read_csv("data/exoplanets.csv")

# Function to query Gaia for stars closest to the planet's coordinates
def get_planet_and_closest_stars(planet_name, n_closest):
    # Filter the exoplanet data for the specific planet
    planet_row = exoplanet_data[exoplanet_data['pl_name'] == planet_name]
    
    if planet_row.empty:
        print(f"No planet {planet_name}.")
        return pd.DataFrame(), pd.DataFrame()  
    
    # Obtaining the RA and Dec of the planet
    planet_ra = planet_row['ra'].values[0]
    planet_dec = planet_row['dec'].values[0]
    
    # ADQL query to get the closest stars from Gaia based on planet's coordinates
    query = f"""
    SELECT source_id, ra, dec, parallax, phot_g_mean_mag, 
           DISTANCE(POINT({planet_ra}, {planet_dec}), POINT(ra, dec)) AS angular_distance
    FROM gaiadr3.gaia_source
    WHERE 1=CONTAINS(
        POINT({planet_ra}, {planet_dec}), 
        CIRCLE(POINT(ra, dec), 0.5)
    )
    ORDER BY angular_distance ASC
    LIMIT {n_closest};
    """

    # Launch the query to Gaia archive
    job = Gaia.launch_job(query)
    results = job.get_results()

    # Convert results to pandas DataFrame for further processing
    closest_stars = results.to_pandas()

    # Return the planet row and closest stars
    return planet_row, closest_stars
