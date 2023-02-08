
import numpy as np
import matplotlib.pyplot as plt
import seaborn as sns
sns.set_theme()
sns.set_context("paper")
analyticalSolution = []
temps = np.arange(-2.26,2.27,0.01)
for r in np.array(temps):
    analyticalSolution.append((1.0-np.sinh((2/r)*-1)**(-4))**(1/8))
plt.plot(temps,analyticalSolution, color="darkblue" )
plt.legend("m")
plt.xlabel("Temperature")
plt.ylabel("Spontaneous magnetization")
plt.show()