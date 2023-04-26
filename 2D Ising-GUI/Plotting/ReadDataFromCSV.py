import numpy as np
import matplotlib.pyplot as plt
import csv
# importing the required modules
import glob

class Trajectory:
     def __init__(self, time, hamiltons, magnet, work, spins):
          self.time=time
          self.hamiltons=hamiltons
          self.magnet = magnet
          self.work = work
          self.spins = spins
          
def LoadCSV(filekey):
    # csv files in the path
    files = glob.glob("*.csv")
    files = list(filter(lambda f: filekey in f, files))
    trajectories = []  
    for name in files:
        filename = name
        time = []
        hamiltons = []
        magnet = []
        work = []
        spins = []
        # Open the CSV file for reading
        with open(filename, 'r') as csv_file:
            # Create a CSV reader
            next(csv_file)
            reader = csv.reader(csv_file, delimiter=';')
            # Iterate over the rows in the file
            for row in reader:
                    # Get the values from the row
                    time.append(int(row[0]))
                    hamiltons.append(float(row[1].replace(',','.')))
                    magnet.append(float(row[2].replace(',','.')))
                    work.append(float(row[3].replace(',','.')))
                    spins.append(row[4])
        trajectories.append(Trajectory(time,hamiltons, magnet, work, spins))
    return trajectories