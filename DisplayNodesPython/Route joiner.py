import os

input_directory = r'C:\Users\Angel\source\repos\VRPdiss\DisplayNodes\Customer nodes\Solution to Routes'
output_directory = r'C:\Users\Angel\source\repos\VRPdiss\DisplayNodes\Customer nodes\Routes for auction'

os.makedirs(output_directory, exist_ok=True)

# get a list of all files in the input directory
files = [f for f in os.listdir(input_directory) if f.endswith('.txt')]

# sort the files in numerical order
files.sort(key=lambda x: int(x.split('Route')[1].split('Solution')[0]))

# concatenate the files and write them to separate output files
num_files = 30
num_files_per_batch = len(files) // num_files + int(len(files) % num_files > 0)
for i in range(num_files):
    start = i * num_files_per_batch
    end = min((i+1) * num_files_per_batch, len(files))
    output_filename = f'Route{i+1}.txt'
    output_filepath = os.path.join(output_directory, output_filename)

    with open(output_filepath, 'w') as output_file:
        for j in range(start, end):
            input_filepath = os.path.join(input_directory, files[j])
            with open(input_filepath, 'r') as input_file:
                content = input_file.read()
                output_file.write(content)
            if j < end - 1:
                output_file.write('NEXT\n')
