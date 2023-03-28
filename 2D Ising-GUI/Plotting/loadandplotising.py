import numpy as np
import matplotlib.pyplot as plt
import csv
# importing the required modules
import glob
import seaborn as sns
sns.set_theme()
sns.set_context("paper")
# csv files in the path
files = glob.glob("*.csv")
files = list(filter(lambda f: "51" in f, files))
#
temps = [1.0,1.1,1.2,1.3,1.4,1.5,1.6,1.7,1.8,1.9,2.0,2.1,2.26]
names = [files[0]]  
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
                #spins.append(row[4])

    plt.subplot(2,1,1)
    plt.ylabel("Energy / Work")
    plt.xlabel('Time')
    plt.plot(time, hamiltons, label="Energy")

    

    plt.plot(time, work, label="Work")
    plt.legend(["Energy","Work"])
    plt.subplot(2,1,2)
    plt.ylabel("Magnet per spin")
    plt.xlabel('Time')
    plt.plot(time, np.array(magnet) / size, label = temp)



plt.legend()


plt.show();