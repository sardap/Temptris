# Temptirs
This is a block puzzle game where your local weather will create different game conditions including.
* Rain
	* Will cause massive rain drops to fall which you cannot control
* Wind
	* Will randomly blow blocks to one side 
* A couple of others which I can't remember

## Building / Running
1. Open Paultis/Paultis/sln
2. Go to [open Weather](https://openweathermap.org/) and get an api key.
3. Put said api key in a file called APIKeys.json in the Paultis folder with the following schema.
```
{
	"OpenWeather" : "API KEY HERE",
	"CityID" : "1871859"
}
```
Note: you should set your city ID to whatever your city is use this site to find out [here](https://openweathermap.org/city/1871859).

4. press f5

## Why?
I really like the open weather api