import numpy as np
import matplotlib.pyplot as plt
arr = np.genfromtxt("06.11.2022 13-38-57.csv",
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
plt.show();