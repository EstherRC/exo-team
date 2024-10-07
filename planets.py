import requests
import pandas as pd

# API URL
url = "https://exoplanetarchive.ipac.caltech.edu/TAP/sync?query=select+pl_name,hostname,ra,dec+from+ps+where+default_flag=1&format=csv"
response = requests.get(url)

# Verify if the request was successful
if response.status_code == 200:
    # Read the data and save it to a CSV file
    data = pd.read_csv(url)
    data.to_csv("data/exoplanets.csv", index=False)

    print(data.head())
else:
    print(f"Error al hacer la solicitud: {response.status_code}")
