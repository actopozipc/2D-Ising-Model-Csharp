import numpy as np
import matplotlib.pyplot as plt
filename = "500-10-1024-09.12.2022 19-29-51.csv";
import csv
B0 = 20
data = filename.split("-")
temp = float(data[1])
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



m = (1-np.sinh((2*B0/temp)*-1)**(-4))**(1/8)
print(m)
m = np.average(np.array(magnet)) / size
print(m)
plt.plot(time, hamiltons, label="Energy")

plt.plot(time, np.array(magnet), label = "M")

plt.plot(time, work, label="Work")
plt.legend()

plt.ylabel("Energy / Magnetization / Work")
plt.xlabel('Iterationen')
plt.show();