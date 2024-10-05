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
def plot_planet_and_closest_stars(planet_name, n_closest=5):
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
    
    # Create the plot
    plt.figure(figsize=(10, 6))
    
    # Graph the planet
    plt.scatter(planet_ra, planet_dec, s=100, color='green', label=f'Planet: {planet_name}')
    
    # Graph the closest stars
    plt.scatter(closest_stars['ra'], closest_stars['dec'], s=50, color='blue', label=f'{n_closest} Closest Stars')
    
    # Add labels and title
    plt.xlabel('Right Ascension (RA)')
    plt.ylabel('Declination (Dec)')
    plt.title(f'Celestial Map for {planet_name} and Closest {n_closest} Stars')
    plt.legend()
    plt.grid(True)
    plt.gca().invert_xaxis()  
    plt.show()

# User input for the planet and number of closest stars
planet_input = input("Name of the planet: ")
n_closest_input = int(input("Number of closest stars to plot: "))

# Call the function with the user input
plot_planet_and_closest_stars(planet_input, n_closest=n_closest_input)
