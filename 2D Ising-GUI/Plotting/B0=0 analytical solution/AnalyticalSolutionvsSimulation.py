import numpy as np
import matplotlib.pyplot as plt
import csv
# importing the required modules
import glob

# csv files in the path
files = glob.glob("*.csv")
#
names = files  
for name in names:
    filename = name;

    B0 = 1
    data = filename.split("-")
    temp = float(data[1].replace(",","."))
    size = int(data[2])
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



    m = (1-np.sinh((2/temp)*-1)**(-4))**(1/8)
    print(temp)
    print("Analytical:",m)
    m = np.average(np.array(magnet)) / size
    print("Simulation:",m)
    # plt.plot(time, hamiltons, label="Energy")

    plt.scatter(temp, np.average(np.array(magnet)) / size, label = temp)

    # plt.plot(time, work, label="Work")

analyticalSolution = []
temps = [1.0,1.1,1.2,1.3,1.4,1.5,1.6,1.7,1.8,1.9,2.0,2.1,2.26]
for r in np.array(temps):
    analyticalSolution.append((1-np.sinh((2/r)*-1)**(-4))**(1/8))
plt.plot(temps,analyticalSolution)
plt.legend()

plt.ylabel("Analytical Solution / Simulationresult")
plt.xlabel('Temp')
plt.show();