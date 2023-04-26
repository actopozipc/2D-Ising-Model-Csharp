import seaborn as sns
import matplotlib.pyplot as plt
import ReadDataFromCSV
import numpy as np
sns.set_theme()
sns.set_context("paper")
filename = "26.04.2023"
traj = ReadDataFromCSV.LoadCSV(filename)
lastWorkValues = []

for t in traj:
    lastWorkValues.append(t.work[-1])

plt.hist(lastWorkValues, bins = np.linspace(np.min(lastWorkValues),np.max(lastWorkValues),20), density=True)
plt.show()