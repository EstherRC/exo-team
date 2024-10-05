import pandas as pd
import matplotlib.pyplot as plt
from astropy.coordinates import SkyCoord
from astropy import units as u

# Load the data
exoplanet_data = pd.read_csv("data/exoplanets.csv")
gaia_data = pd.read_csv("data/gaia_stars.csv")

# Create a SkyCoord object for the stars
stars_coords = SkyCoord(ra=gaia_data['ra'].values*u.deg, dec=gaia_data['dec'].values*u.deg)

# Function to plot the planet and its closest stars
def get_planet_and_closest_stars(planet_name, n_closest):
    # Filter the exoplanet data for the specific planet
    planet_row = exoplanet_data[exoplanet_data['pl_name'] == planet_name]
    
    if planet_row.empty:
        print(f"No se encontró el planeta {planet_name}. Verifica el nombre.")
        return
    
    # Obtaining the RA and Dec of the planet
    planet_ra = planet_row['ra'].values[0]
    planet_dec = planet_row['dec'].values[0]
    
    # Create a SkyCoord object for the planet
    planet_coord = SkyCoord(ra=planet_ra*u.deg, dec=planet_dec*u.deg)
    
    # Calculate the angular separations between the planet and the stars
    separations = planet_coord.separation(stars_coords)
    
    # Order the stars by their separation
    closest_indices = separations.argsort()[:n_closest]
    
    # Select the closest stars
    closest_stars = gaia_data.iloc[closest_indices]
    closest_stars.fillna(0, inplace=True)

    return planet_row, closest_stars
