import numpy as np
import matplotlib.pyplot as plt
import seaborn as sns
import ReadDataFromCSV
sns.set_theme()
sns.set_context("paper")
filename = "500-10-1024-26.04.2023 18-48-38"
traj = ReadDataFromCSV.LoadCSV(filename)[0]
time, hamiltons, magnet, work, spins = traj.time, traj.hamiltons, traj.magnet, traj.work, traj.spins

temps = [1.0,1.1,1.2,1.3,1.4,1.5,1.6,1.7,1.8,1.9,2.0,2.1,2.26]

data = filename.split("-")
temp = float(data[1].replace(",","."))
size = int(data[2])
    

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