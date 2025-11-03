import json

#Not impt, I'm using this cause I'm lazy manual edit json

save_slots = 5
fields = ["Slot_Name", "Current_Day", "Current_Rent", "Money", "Inventory", "Upgrades", "Relationships", "Alive", "New",]
value = ["Empty Save Slot", 1, 2000, 0, {}, {}, {}, True, True,]

with open("Assets\\Scripts\\data.json", "r") as f: 
    data = json.load(f)

for i in range(save_slots): 
    slot = f"Slot_{i}"
    data[slot] = {}
    for j, field in enumerate(fields): 
        data[slot][field] = value[j]

with open("Assets\\Scripts\\data.json", "w") as f: 
    json.dump(data, f, indent = 4)



        