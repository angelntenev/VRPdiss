import os

dir_path = r'C:\Users\Angel\source\repos\VRPdiss\DisplayNodes\Customer nodes\Solution to Routes'

for filename in os.listdir(dir_path):
    if filename.endswith('.txt'):
        file_path = os.path.join(dir_path, filename)
        with open(file_path, 'r') as f:
            lines = f.readlines()
        seen_points = set()
        unique_points = []
        for line in lines:
            if line not in seen_points:
                unique_points.append(line)
                seen_points.add(line)
        with open(file_path, 'w') as f:
            f.writelines(unique_points)
