import seaborn as sns
import matplotlib.pyplot as plt
import ReadDataFromCSV
import numpy as np
sns.set_theme()
sns.set_context("paper")
filename = "13.05.2023"
traj = ReadDataFromCSV.LoadCSV(filename)
lastWorkValues = []

for t in traj:
    lastWorkValues.append(t.work[-1])

print(np.max(lastWorkValues))
plt.hist(np.array(lastWorkValues), bins = 30, density=True)
plt.hist(-np.array(lastWorkValues), bins = 30, density=True)
plt.show()