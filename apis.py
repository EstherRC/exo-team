from fastapi import FastAPI, HTTPException
from gaia import get_planet_and_closest_stars, get_planets

app = FastAPI()

def clean_dataframe(df):
    return df.replace([float('inf'), float('-inf'), float('nan')], 0).fillna(0)

@app.get("/stars/planet")
def get_stars(planet_name: str, n_closest: int):
    planet_row, closest_stars = get_planet_and_closest_stars(planet_name)
    planet_row_cleaned = clean_dataframe(planet_row)
    closest_stars_cleaned = clean_dataframe(closest_stars)

    return {
        "planet": planet_row_cleaned.to_dict(orient="records"),
        "stars": closest_stars_cleaned.to_dict(orient="records")
    }
    
@app.get("/planets")
def get_all_planets(): 
    return get_planets()