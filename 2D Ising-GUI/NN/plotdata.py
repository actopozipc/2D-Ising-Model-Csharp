import matplotlib.pyplot as plt
class DataPoint:
    def __init__(self, spinzahl, bj, hidden, embedding, alpha):
        self.spinzahl = spinzahl
        self.bj = bj
        self.hidden = hidden
        self.embedding = embedding
        self.alpha = alpha

filename = "data.txt"

data_list = []
# Daten in verschiedenen Kategorien speichern
dataSortedBySpins = {}
dataSortedByBJ = {}
with open(filename, "r") as file:
    lines = file.readlines()

for line in lines[1:]:
    data = line.strip().split(" ")
    spinzahl = data[0]
    bj = data[1]
    hidden = int(data[2])
    embedding = int(data[3])
    alpha = float(data[4])

    data_point = DataPoint(spinzahl, bj, hidden, embedding, alpha)
    data_list.append(data_point)
    # Kategorie erstellen, falls noch nicht vorhanden
    #Nach Spins sortieren
    if spinzahl not in dataSortedBySpins:
        if hidden == 8 and embedding == 4:
            dataSortedBySpins[spinzahl] = {"bj": [], "alpha": []}
    #Nach hidden sortieren
    if bj not in dataSortedByBJ:
        dataSortedByBJ[bj] = {"hidden": [], "embedding":[], "alpha": []}
    # Daten zur entsprechenden Kategorie hinzufügen
    if hidden == 8 and embedding == 4:
        dataSortedBySpins[spinzahl]["bj"].append(bj)
        dataSortedBySpins[spinzahl]["alpha"].append(alpha)
    dataSortedByBJ[bj]["hidden"].append(hidden)
    dataSortedByBJ[bj]["embedding"].append(embedding)
    dataSortedByBJ[bj]["alpha"].append(alpha)


# Beispielhafter Zugriff auf die Werte
for data_point in data_list:
    print("Spinzahl:", data_point.spinzahl)
    print("BJ:", data_point.bj)
    print("Hidden:", data_point.hidden)
    print("Embedding:", data_point.embedding)
    print("Alpha:", data_point.alpha)
    print()

# Erstellen des 2x2-Plots
fig, axs = plt.subplots(2, 2, figsize=(10, 8))
# Subplot für Alpha vs. BJ
axs[0,0].set_xlabel("BJ")
axs[0,0].set_ylabel("Alpha")
axs[0,0].set_title("Alpha vs BJ")
filtered_data = [data_point for data_point in data_list if data_point.embedding == 4 and data_point.hidden == 8]
tens = [data_point for data_point in filtered_data if data_point.spinzahl == "10"]
twent = [data_point for data_point in filtered_data if data_point.spinzahl == "20"]
third = [data_point for data_point in filtered_data if data_point.spinzahl == "30"]
for e in [tens, twent, third]:
    bj = []
    alpha_values = []
    sp = e[0].spinzahl
    e.sort(key=lambda x: x.bj)
    for d in e:
        if d.bj not in bj:
            bj.append(d.bj)
            alpha_values.append(d.alpha)
 
    axs[0,0].plot(bj, alpha_values, label=f"N_sp = {sp}")

axs[0,0].legend()

axs[0,1].set_xlabel("N_hidden")
axs[0,1].set_ylabel("Alpha")
axs[0,1].set_title("Alpha vs N_hidden")
filtered_data = [data_point for data_point in data_list if data_point.embedding == 4 and data_point.spinzahl == "10"]
tens = [data_point for data_point in filtered_data if data_point.bj == "10"]
twent = [data_point for data_point in filtered_data if data_point.bj == "20"]
third = [data_point for data_point in filtered_data if data_point.bj == "30"]
fifth = [data_point for data_point in filtered_data if data_point.bj == "50"]
for e in [tens, twent, third, fifth]:
    hidden_values = []
    alpha_values = []
    bj = e[0].bj
    e.sort(key=lambda x: x.hidden)
    for d in e:
        if d.hidden not in hidden_values:
            hidden_values.append(d.hidden)
            alpha_values.append(d.alpha)
 
    axs[0,1].plot(hidden_values, alpha_values, label=f"BJ = {bj}")
axs[1,0].set_xlabel("N_embedding")
axs[1,0].set_ylabel("Alpha")
axs[1,0].set_title("Alpha vs N_embedding")
filtered_data = [data_point for data_point in data_list if data_point.hidden == 8 and data_point.spinzahl == "10"]
tens = [data_point for data_point in filtered_data if data_point.bj == "10"]
twent = [data_point for data_point in filtered_data if data_point.bj == "20"]
third = [data_point for data_point in filtered_data if data_point.bj == "30"]
fifth = [data_point for data_point in filtered_data if data_point.bj == "50"]
for e in [tens, twent, third, fifth]:
    embedding_values = []
    alpha_values = []
    bj = e[0].bj
    e.sort(key=lambda x: x.embedding)
    for d in e:
        if d.embedding not in embedding_values:
            embedding_values.append(d.embedding)
            alpha_values.append(d.alpha)
 
    axs[1,0].plot(embedding_values, alpha_values, label=f"BJ = {bj}")
# Anpassung des Layouts
axs[0,1].legend()
axs[1,0].legend()
plt.tight_layout()

# Anzeigen des Plots
plt.show()