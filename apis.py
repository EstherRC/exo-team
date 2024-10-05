from fastapi import FastAPI
from fastapi.responses import JSONResponse
from gaia import get_planet_and_closest_stars
app = FastAPI()

#Get the planet and the closest stars
#planet_name and n_closest are the parameters to be passed
@app.get("/stars/planet")
def get_stars(planet_name: str, n_closest: int):
    planetrow,closest_stars = get_planet_and_closest_stars(planet_name, n_closest)
    return {"planet": planetrow.to_dict(orient="records"), "stars": closest_stars.to_dict(orient="records")}
