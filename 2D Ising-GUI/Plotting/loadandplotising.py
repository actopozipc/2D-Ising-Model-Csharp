import numpy as np
import matplotlib.pyplot as plt
arr = np.genfromtxt("06.12.2022 14-29-06.csv",
                    delimiter=",", dtype=(int, float, float), )
temp = 2.0
size = 32*32
time = []
hamiltons = []
magnet = []
for i in range(len(arr)):
    time.append(arr[i][0])
    hamiltons.append(arr[i][1])
    magnet.append(arr[i][2])

m = (1-np.sinh((2/temp)*-1)**(-4))**(1/8)
print(m)
m = np.average(np.array(magnet)) / size
print(m)
plt.plot(time, hamiltons)
plt.plot(time, magnet)
plt.ylabel("Energy / Magnetization")
plt.xlabel('Iterationen')
plt.show();