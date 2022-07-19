Project class outline

Generating the level

LevelData
{
	LevelData mirrors the structure of the Json files that are within the project
}

JsonLoader
{
	Loads in the Json file parsing through the LevelData class
	offsets the position coordinates in the LevelData so all X and Y coordinates are above 0
	gets the max and min values from the coordinates
}
