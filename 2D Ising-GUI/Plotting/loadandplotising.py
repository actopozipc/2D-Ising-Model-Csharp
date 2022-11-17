import numpy as np
import matplotlib.pyplot as plt
arr = np.genfromtxt("17.11.2022 20-38-14.csv",
                    delimiter=",", dtype=(int, float, float), )
time = []
hamiltons = []
magnet = []
for i in range(len(arr)):
    time.append(arr[i][0])
    hamiltons.append(arr[i][1])
    magnet.append(arr[i][2])


plt.plot(time, hamiltons)
plt.plot(time, magnet)
plt.ylabel("Energy / Magnetization")
plt.xlabel('Iterationen')
plt.show();